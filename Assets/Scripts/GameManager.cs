using System.Collections;

using UnityEngine;

using BoneBox;

namespace Flocking
{
	public class GameManager : Singleton<GameManager>
	{
		[SerializeField]
		private bool _drawGroupCenters = false;

		[SerializeField]
		private bool _dictatorship = false;

		[SerializeField]
		private bool _assignRandomLeader = false;

		private int _updates = 0;

		public bool DrawGroupCenters { get { return _drawGroupCenters; } }
		public bool Dictatorship { get { return _dictatorship; } }
		public bool AssignRandomLeader { get { return _assignRandomLeader; } set { _assignRandomLeader = value; } }
		public int Updates { get { return _updates; } }

		protected override void Awake()
		{
			base.Awake();

			_updates = 0;

			Random.InitState((int) System.DateTime.Now.Ticks);
		}

		void Update()
		{
			_updates++;
		}
	}
}
