namespace StateSystem
{
    using System;
    using UnityEngine;

    [RequireComponent (typeof (StateMachine))]
    public class StateEventHandler : ManagedBehaviour
    {
        private StateMachine stateMachine;

		public void Initialize (StateMachine stateMachine)
		{
			this.stateMachine = stateMachine;
		}

		public void SubscribeToSetState (State state, EventHandler setStateEvent) => stateMachine.SubscribeToSetState (state, setStateEvent);

		public void SubscribeToSetState (string stateID, EventHandler setStateEvent) => stateMachine.SubscribeToSetState (stateID, setStateEvent);

		public void SubscribeToClearState (State state, EventHandler clearStateEvent) => stateMachine.SubscribeToClearState (state, clearStateEvent);

		public void SubscribeToClearState (string stateID, EventHandler clearStateEvent) => stateMachine.SubscribeToClearState (stateID, clearStateEvent);

		public void UnsubscribeFromSetState (State state, EventHandler setStateEvent) => stateMachine.UnsubscribeFromSetState (state, setStateEvent);

		public void UnsubscribeFromSetState (string stateID, EventHandler setStateEvent) => stateMachine.UnsubscribeFromSetState (stateID, setStateEvent);

		public void UnsubscribeFromClearState (State state, EventHandler clearStateEvent) => stateMachine.UnsubscribeFromClearState (state, clearStateEvent);

		public void UnsubscribeFromClearState (string stateID, EventHandler clearStateEvent) => stateMachine.UnsubscribeFromClearState (stateID, clearStateEvent);
    }
}
