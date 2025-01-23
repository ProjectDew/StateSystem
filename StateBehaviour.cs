namespace StateSystem
{
	using UnityEngine;

	[RequireComponent (typeof (StateMachine))]
	public abstract class StateBehaviour : ManagedBehaviour
	{
		[SerializeField]
		private State[] linkedStates;

		public State[] GetLinkedStates () => linkedStates;

		public abstract void Initialize (StateMachine stateMachine, StateBehaviourArgs args);

		public virtual void OnSetState () { }

		public abstract void Execute ();

		public virtual void OnClearState () { }
	}
}
