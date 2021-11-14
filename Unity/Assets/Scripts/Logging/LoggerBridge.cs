using Logging;
using UnityEngine;
using UnityEngine.Events;

public class LoggerBridge : MonoBehaviour
{
    [SerializeField] private DataLogger logger;
    [SerializeField] private UnityEvent onInitialize = new UnityEvent();

    private async void Start()
    {
    }

    public async void Send()
    {
        logger.SendData();
    }
}