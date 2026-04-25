using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Wincon : MonoBehaviour
{

    public UnityEvent Wincondition;

    public Animator animatorScreen;
    public Animator animatorTimer;

    void OnTriggerEnter(Collider other)
    {
        Wincondition.Invoke();
    }

    public void FadeToBlackSad()
    {
        animatorScreen.SetTrigger("WinWinWin");
        animatorTimer.SetTrigger("Reverse");

        StartCoroutine(WaitSad());
    }

    IEnumerator WaitSad()
    {
        yield return new WaitForSeconds(2f);

        ChangeScene("Loss");
    }

    public void FadeToBlack()
    {
        animatorScreen.SetTrigger("WinWinWin");
        animatorTimer.SetTrigger("Reverse");

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);

        ChangeScene("Wedding");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
