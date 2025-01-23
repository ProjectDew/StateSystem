namespace StateSystem
{
	public abstract class StateBehaviourArgs
	{
		private class EmptyArgs : StateBehaviourArgs { }

		private static EmptyArgs emptyArgs;

		public static StateBehaviourArgs Empty
		{
			get
			{
				if (emptyArgs == null)
					emptyArgs = new ();

				return emptyArgs;
			}
		}
	}
}
