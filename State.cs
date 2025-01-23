namespace StateSystem
{
	using UnityEngine;

	[CreateAssetMenu (fileName = "New state", menuName = "State")]
	public class State : ScriptableObject
	{
		[SerializeField]
		private string id;

		public string ID => id;
	}
}
