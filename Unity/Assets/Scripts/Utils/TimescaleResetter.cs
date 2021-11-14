using UnityEngine;

public class TimescaleResetter : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = .02f;
        Time.maximumDeltaTime = 1 / 3f;
    }
}