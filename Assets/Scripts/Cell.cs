using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool nxtGenAlive;
    public  int neighbors;
    SpriteRenderer spriteRenderer;
    public Sprite deadSprite;
    Animator animator;

    Sprite tempSprite;
    private void Start()
    {
    }

    public void UpdateStatus()
    {
        animator ??= GetComponent<Animator>();
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = alive;
        tempSprite = spriteRenderer.sprite;

        if (alive && !nxtGenAlive)
        {
            //spriteRenderer.color.a = 0.5f;
            spriteRenderer.sprite = deadSprite;
            spriteRenderer.color = Color.white;
        }
        if(!alive && nxtGenAlive)
        {
            spriteRenderer.sprite = tempSprite;
        }
        if(alive && nxtGenAlive)
        {
            spriteRenderer.sprite = tempSprite;
            Debug.Log(spriteRenderer.sprite.name);
        }
        if (!alive && !nxtGenAlive)
            spriteRenderer.sprite = tempSprite;
        animator.SetBool("isAlive", alive );
        animator.SetBool("isNxt", nxtGenAlive);

    }
}
