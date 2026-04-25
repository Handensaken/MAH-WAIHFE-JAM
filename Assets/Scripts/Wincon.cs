using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Win_Scene", LoadSceneMode.Single);
    }

}
