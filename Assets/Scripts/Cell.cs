using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool nxtGenAlive;
    public  int neighbors;
    SpriteRenderer spriteRenderer;

    Animator animator;

    public void UpdateStatus()
    {
        animator ??= GetComponent<Animator>();
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = alive;
        animator.SetBool("isAlive", alive );
        animator.SetBool("isNxt", nxtGenAlive);

    }
}
