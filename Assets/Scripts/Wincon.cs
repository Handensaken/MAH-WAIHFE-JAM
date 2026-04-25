using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Wincon : MonoBehaviour
{

    public UnityEvent Wincondition;

    public Animator animator;

    void OnTriggerEnter(Collider other)
    {
        Wincondition.Invoke();
    }

    public void FadeToBlack()
    {
        animator.SetTrigger("WinWinWin");

        StartCoroutine(Wait());
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(2f);

        ChangeScene("Wedding");

        Debug.Log("TA");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
