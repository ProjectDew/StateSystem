# StateSystem

A finite state machine that I designed for personal use.

*(Note: all the classes in this repository that would inherit from MonoBehaviour inherit from [ManagedBehaviour](https://gist.github.com/ProjectDew/affdadd490060f680e135be9e5fa32bc) instead, a class that I use to manage what should be executing and when).*

- ***State***: a ScriptableObject that, along with the StateMachine component, is the core of the system.

  - ***ID***: returns a string that serves to identify the state.

- ***StateMachine***: the core class of the system. It's a component which contains an array of states that can be added through the inspector, as well as events that are raised whenever a state is set or cleared.

  - ***PreviousState***: returns the previous state of the object.
  - ***CurrentState***: returns the current state of the object.
  - ***Initialize (2 overloads)***: if the object has an event handler or any of the states has some behaviours, this method initializes them.
  - ***SubscribeToSetState (2 overloads)***: subscribes to an event that is fired when the specified state is set.
  - ***SubscribeToClearState (2 overloads)***: subscribes to an event that is fired when the specified state is cleared.
  - ***UnsubscribeFromSetState (2 overloads)***: unsubscribes from an event that is fired when the specified state is set.
  - ***UnsubscribeFromClearState (2 overloads)***: unsubscribes from an event that is fired when the specified state is cleared.
  - ***GetStates***: returns an array with all the possible states of the object.
  - ***ContainsState (2 overloads)***: returns true if the specified state is among the ones in the object.
  - ***IsPreviousState (2 overloads)***: returns true if the specified state is the previous one.
  - ***IsCurrentState (2 overloads)***: returns true if the specified state is the current one.
  - ***SetState (2 overloads)***: sets the specified state as current.
  - ***SetPreviousState***: sets the previous state as current.
  - ***ExecuteBehaviours***: runs all the behaviours linked to the current state.

- ***StateBehaviour***: an abstract class that requires a StateMachine component. Any class inheriting from this one should be linked to one or more states through the inspector.

  - ***Initialize***: prepares the class to work.
  - ***GetLinkedStates***: returns an array with all the states linked to this behaviour.
  - ***OnSetState***: virtual method (empty by default) that is called automatically every time one of the linked states is set.
  - ***Execute***: abstract method that contains the actual behaviour.
  - ***OnClearState***: virtual method (empty by default) that is called automatically every time one of the linked states is cleared.

- ***StateBehaviourArgs***: an optional abstract class that can be used to pass data to a state behaviour when initializing it.

  - ***Empty***: returns an empty StateBehaviourArgs.

***StateEventHandler***: an optional class that requires a StateMachine component. It's used to subscribe to, or unsubscribe from, the events of that machine without exposing its other methods and properties to the listener.

  - ***Initialize***: prepares the class to work.
  - ***SubscribeToSetState (2 overloads)***: subscribes to an event that is fired when the specified state is set.
  - ***SubscribeToClearState (2 overloads)***: subscribes to an event that is fired when the specified state is cleared.
  - ***UnsubscribeFromSetState (2 overloads)***: unsubscribes from an event that is fired when the specified state is set.
  - ***UnsubscribeFromClearState (2 overloads)***: unsubscribes from an event that is fired when the specified state is cleared.
