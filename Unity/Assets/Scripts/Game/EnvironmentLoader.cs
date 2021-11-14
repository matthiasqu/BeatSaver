using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class EnvironmentLoader : MonoBehaviour
    {
        [SerializeField] private string environmentSceneName;

        private void Start()
        {
            SceneManager.LoadSceneAsync(environmentSceneName, LoadSceneMode.Additive);
        }
    }
}