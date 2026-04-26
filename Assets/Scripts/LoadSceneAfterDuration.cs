using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterDuration : MonoBehaviour
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private string sceneName = "";
  

    private void Start()
    {
        Invoke(nameof(LoadSceneByNameIdk), _duration);
    }

    private void LoadSceneByNameIdk()
    {
        SceneManager.LoadScene(sceneName);
    }
}
