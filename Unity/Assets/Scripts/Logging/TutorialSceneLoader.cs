using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logging
{
    public class TutorialSceneLoader : MonoBehaviour
    {
        [SerializeField] private ParticipantData participantData;
        [SerializeField] private string experimentalTutorialSceneName;
        [SerializeField] private string controlTutorialSceneName;


        public void LoadScene()
        {
            switch (participantData.Condition)
            {
                case Group.control:
                    Debug.Log(
                        $"[TutorialSceneLoader] Group is {participantData.Condition}, loading {controlTutorialSceneName}");
                    SceneManager.LoadSceneAsync(controlTutorialSceneName);
                    break;
                case Group.experimental:
                    Debug.Log(
                        $"[TutorialSceneLoader] Group is {participantData.Condition}, loading {controlTutorialSceneName}");
                    SceneManager.LoadSceneAsync(experimentalTutorialSceneName);
                    break;
            }
        }
    }
}