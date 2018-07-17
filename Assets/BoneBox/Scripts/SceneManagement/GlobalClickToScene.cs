using UnityEngine;

namespace BoneBox.SceneManagement
{
	public class GlobalClickToScene : MonoBehaviour
	{
		[SerializeField] private string m_TargetScene;

		private void Update()
		{
			if (Input.GetMouseButtonUp(0)) {
				SceneLoader.LoadScene(m_TargetScene);
			}
		}
	}
}