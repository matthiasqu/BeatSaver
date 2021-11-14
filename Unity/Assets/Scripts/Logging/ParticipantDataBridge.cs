using Logging;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class ParticipantDataBridge : MonoBehaviour
{
    [SerializeField] private ParticipantData participantData;
    [SerializeField] private UnityEventInt onIdChanged = new UnityEventInt();
    [SerializeField] private UnityEvent<Group> onConditionChanged = new UnityEvent<Group>();

    private void Start()
    {
        participantData.onIdChanged.AddListener(onIdChanged.Invoke);
        participantData.onGroupChanged.AddListener(onConditionChanged.Invoke);
    }
}