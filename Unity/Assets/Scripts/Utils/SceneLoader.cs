using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    [CreateAssetMenu(menuName = "_custom/Create SceneLoader", fileName = "SceneLoader", order = 0)]
    public class SceneLoader : ScriptableObject
    {
        public void LoadScene(string scenename)
        {
            SceneManager.LoadScene(scenename);
        }

        public void LoadTutorialScene()
        {
        }
    }
}