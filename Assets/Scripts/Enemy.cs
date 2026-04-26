using UnityEngine;

public class Enemy : MonoBehaviour
{


    public Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Pushing()
    {
        animator.SetTrigger("Push");
    }

    public void Lefter()
    {
        animator.SetTrigger("Lefter");
    }

    public void Righter()
    {
        animator.SetTrigger("Righter");
    }
}
