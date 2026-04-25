using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float threshold;
    private float elapsedTime;

    private bool timerOn = true;

    public Slider timerSlider;

    public Wincon winconObject;

    public UnityEvent Loser;

    private void Awake()
    {
        elapsedTime = 0f;

        if (timerSlider != null)
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = threshold;
            timerSlider.value = 0f;
        }
    }

    private void Update()
    {

        if(timerOn){
            elapsedTime += Time.deltaTime;
            if (timerSlider != null)
        {
            timerSlider.value = elapsedTime;
        }

        if (elapsedTime > threshold)
        {
            winconObject.FadeToBlackSad();
            Loser.Invoke();
            timerOn = false;
        }
        }
    }

    public void StopTimer()
    {
        timerOn = false;
    }
}