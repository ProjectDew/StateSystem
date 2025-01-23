namespace StateSystem
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public class StateMachine : ManagedBehaviour
	{
		private readonly string wrongID = "There is no state with the ID \"{0}\" in this context. Object name: \"{1}\"";
		private readonly string wrongState = "The state provided doesn't exist in this context. Object name: \"{0}\", state ID: \"{1}\".";

		[SerializeField]
		private State[] states;

		public class BehaviourData
		{
			private List<StateBehaviour> behaviours;

			public void AddBehaviour (StateBehaviour behaviour)
			{
				if (behaviours == null)
					behaviours = new ();

				behaviours.Add (behaviour);
			}

			public void OnSetState ()
			{
				if (behaviours == null || behaviours.Count == 0)
					return;

				for (int i = 0; i < behaviours.Count; i++)
				{
					if (behaviours[i] == null)
						continue;

					behaviours[i].OnSetState ();
				}
			}

			public void ExecuteBehaviours ()
			{
				if (behaviours == null || behaviours.Count == 0)
					return;

				for (int i = 0; i < behaviours.Count; i++)
				{
					if (behaviours[i] == null)
					{
						Debug.LogError ("One of the behaviours that you are trying to execute is null.");
						continue;
					}

					behaviours[i].Execute ();
				}
			}

			public void OnClearState ()
			{
				if (behaviours == null || behaviours.Count == 0)
					return;

				for (int i = 0; i < behaviours.Count; i++)
				{
					if (behaviours[i] == null)
						continue;

					behaviours[i].OnClearState ();
				}
			}
		}

		private Dictionary<string, State> stateIDs;

		private Dictionary<State, EventHandler> setStateEvents;
		private Dictionary<State, EventHandler> clearStateEvents;

		private Dictionary<State, BehaviourData> behaviours;

		private Dictionary<string, State> StateIDs
		{
			get
			{
				if (stateIDs == null)
					CreateDictionary ();

				return stateIDs;
			}
		}
	
		public State PreviousState { get; private set; }
	
		public State CurrentState { get; private set; }

		public void Initialize ()
		{
			InitializeEventHandler ();
			InitializeBehaviours (StateBehaviourArgs.Empty);
		}

		public void Initialize (StateBehaviourArgs stateBehaviourArgs)
		{
			InitializeEventHandler ();
			InitializeBehaviours (stateBehaviourArgs);
		}

		public void SubscribeToSetState (string stateID, EventHandler setStateEvent)
		{
			if (!StateIDs.ContainsKey (stateID))
			{
				Debug.LogError (string.Format (wrongID, stateID, gameObject.name));
				return;
			}
			
			SubscribeToSetState (StateIDs[stateID], setStateEvent);
		}

		public void SubscribeToSetState (State state, EventHandler setStateEvent)
		{
			if (!StateIDs.ContainsValue (state))
			{
				Debug.LogError (string.Format (wrongState, gameObject.name, state.ID));
				return;
			}

			if (setStateEvents == null)
				setStateEvents = new ();

			if (setStateEvents.ContainsKey (state))
				setStateEvents[state] += setStateEvent;
			else
				setStateEvents.Add (state, setStateEvent);
		}

		public void SubscribeToClearState (string stateID, EventHandler clearStateEvent)
		{
			if (!StateIDs.ContainsKey (stateID))
			{
				Debug.LogError (string.Format (wrongID, stateID, gameObject.name));
				return;
			}
			
			SubscribeToClearState (StateIDs[stateID], clearStateEvent);
		}

		public void SubscribeToClearState (State state, EventHandler clearStateEvent)
		{
			if (!StateIDs.ContainsValue (state))
			{
				Debug.LogError (string.Format (wrongState, gameObject.name, state.ID));
				return;
			}

			if (clearStateEvents == null)
				clearStateEvents = new ();

			if (clearStateEvents.ContainsKey (state))
				clearStateEvents[state] += clearStateEvent;
			else
				clearStateEvents.Add (state, clearStateEvent);
		}

		public void UnsubscribeFromSetState (string stateID, EventHandler setStateEvent)
		{
			if (!StateIDs.ContainsKey (stateID))
			{
				Debug.LogError (string.Format (wrongID, stateID, gameObject.name));
				return;
			}
			
			UnsubscribeFromSetState (StateIDs[stateID], setStateEvent);
		}

		public void UnsubscribeFromSetState (State state, EventHandler setStateEvent)
		{
			if (!StateIDs.ContainsValue (state))
			{
				Debug.LogError (string.Format (wrongState, gameObject.name, state.ID));
				return;
			}

			if (!setStateEvents.ContainsKey (state))
				return;

			Delegate[] invocationList = setStateEvents[state].GetInvocationList ();

			setStateEvents[state] -= setStateEvent;

			if (invocationList.Length < 2)
				setStateEvents.Remove (state);
		}

		public void UnsubscribeFromClearState (string stateID, EventHandler clearStateEvent)
		{
			if (!StateIDs.ContainsKey (stateID))
			{
				Debug.LogError (string.Format (wrongID, stateID, gameObject.name));
				return;
			}
			
			UnsubscribeFromClearState (StateIDs[stateID], clearStateEvent);
		}

		public void UnsubscribeFromClearState (State state, EventHandler clearStateEvent)
		{
			if (!StateIDs.ContainsValue (state))
			{
				Debug.LogError (string.Format (wrongState, gameObject.name, state.ID));
				return;
			}

			if (!clearStateEvents.ContainsKey (state))
				return;

			Delegate[] invocationList = clearStateEvents[state].GetInvocationList ();

			clearStateEvents[state] -= clearStateEvent;

			if (invocationList.Length < 2)
				clearStateEvents.Remove (state);
		}

		public State[] GetStates () => states;

		public bool ContainsState (string stateID) => StateIDs.ContainsKey (stateID);

		public bool ContainsState (State state) => StateIDs.ContainsValue (state);

		public bool IsPreviousState (State state)
		{
			if (state == null || PreviousState == null)
				return false;
			
			if (!StateIDs.ContainsValue (state))
			{
				Debug.LogError (string.Format (wrongState, gameObject.name, state.ID));
				return false;
			}

			return state == PreviousState;
		}

		public bool IsPreviousState (string stateID)
		{
			if (!StateIDs.ContainsKey (stateID))
			{
				Debug.LogError (string.Format (wrongID, stateID, gameObject.name));
				return false;
			}
			
			return IsPreviousState (StateIDs[stateID]);
		}

		public bool IsCurrentState (State state)
		{
			if (state == null || CurrentState == null)
				return false;
			
			if (!StateIDs.ContainsValue (state))
			{
				Debug.LogError (string.Format (wrongState, gameObject.name, state.ID));
				return false;
			}

			return state == CurrentState;
		}

		public bool IsCurrentState (string stateID)
		{
			if (!StateIDs.ContainsKey (stateID))
			{
				Debug.LogError (string.Format (wrongID, stateID, gameObject.name));
				return false;
			}
			
			return IsCurrentState (StateIDs[stateID]);
		}

		public void SetState (State state)
		{
			if (state == null)
				return;
			
			if (!StateIDs.ContainsValue (state))
			{
				Debug.LogError (string.Format (wrongState, gameObject.name, state.ID));
				return;
			}

			bool stateHasBehaviours = behaviours != null && behaviours.Count > 0 && behaviours.ContainsKey (CurrentState);

			if (stateHasBehaviours)
				behaviours[CurrentState].OnClearState ();

			if (clearStateEvents != null && clearStateEvents.ContainsKey (CurrentState))
				clearStateEvents[CurrentState]?.Invoke (this, EventArgs.Empty);

			PreviousState = CurrentState;
			CurrentState = state;

			if (stateHasBehaviours)
				behaviours[CurrentState].OnSetState ();

			if (setStateEvents != null && setStateEvents.ContainsKey (CurrentState))
				setStateEvents[CurrentState]?.Invoke (this, EventArgs.Empty);
		}

		public void SetState (string stateID)
		{
			if (!StateIDs.ContainsKey (stateID))
			{
				Debug.LogError (string.Format (wrongID, stateID, gameObject.name));
				return;
			}
			
			SetState (stateIDs[stateID]);
		}

		public void SetPreviousState () => SetState (PreviousState);

		public void ExecuteBehaviours ()
		{
			if (behaviours == null || behaviours.Count == 0)
			{
				Debug.LogError (string.Format ("There are no behaviours to execute, but you can add them as components of this GameObject. Object name: \"{0}\".", gameObject.name));
				return;
			}

			if (behaviours.ContainsKey (CurrentState))
				behaviours[CurrentState].ExecuteBehaviours ();
		}

		private void CreateDictionary ()
		{
			stateIDs = new ();

			for (int i = 0; i < states.Length; i++)
			{
				if (states[i] == null)
				{
					Debug.LogError (string.Format ("The state at index {0} is null. Object name: \"{1}\".", i.ToString (), gameObject.name));
					continue;
				}

				stateIDs.Add (states[i].ID, states[i]);
			}
		}

		private void InitializeEventHandler ()
		{
			StateEventHandler eventHandler = GetComponent<StateEventHandler> ();

			if (eventHandler == null)
				return;

			eventHandler.Initialize (this);
		}

		private void InitializeBehaviours (StateBehaviourArgs stateBehaviourArgs)
		{
			StateBehaviour[] behaviours = GetComponents<StateBehaviour> ();

			if (behaviours == null || behaviours.Length == 0)
				return;
			
			this.behaviours = new ();

			for (int i = 0; i < behaviours.Length; i++)
			{
				behaviours[i].Initialize (this, stateBehaviourArgs);
				AddBehaviourData (behaviours[i]);
			}
		}

		private void AddBehaviourData (StateBehaviour behaviour)
		{
			if (behaviour == null)
				return;

			State[] states = behaviour.GetLinkedStates ();

			for (int i = 0; i < states.Length; i++)
			{
				State state = states[i];

				if (state == null)
				{
					Debug.LogWarning (string.Format ("The state linked to the \"{0}\" behaviour at index {1} is null. Object name: \"{2}\".", behaviour.name, i.ToString (), gameObject.name));
					continue;
				}

				if (!behaviours.ContainsKey (state))
					behaviours.Add (state, new BehaviourData ());

				behaviours[state].AddBehaviour (behaviour);
			}
		}
	}
}
