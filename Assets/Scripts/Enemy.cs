using UnityEngine;

public class Enemy : MonoBehaviour
{


    Animator animator;


    void Awake()
    {
        animator.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider collider)
    {
        animator.SetTrigger("Push");
    }
}
