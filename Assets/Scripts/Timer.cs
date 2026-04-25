using UnityEngine;

public class Timer : MonoBehaviour
{
    public float threshold;
    public float elapsedTime;

    private void Start()
    {
        elapsedTime = 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > threshold)
        {
            Debug.Log("Time out!");
            // Do what you want
        }
    }
}