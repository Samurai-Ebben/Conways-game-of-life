using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool nxtGenAlive;
    public int neighbors;
    public Sprite deadSprite;
    public Sprite livSprite;

    SpriteRenderer spriteRenderer;
    ParticleSystem paSy;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        paSy ??= GetComponent<ParticleSystem>();
        //spriteRenderer.enabled = alive;


        if (alive && !nxtGenAlive)
        {

            paSy.Play();
            spriteRenderer.sprite = null;
            //transform.localScale *= .5f;
        }
        if (!alive && !nxtGenAlive)
        {
            spriteRenderer.sprite = null;
        }
        if (nxtGenAlive)
        {
            spriteRenderer.sprite = livSprite;
        }
        //if (!alive && !nxtGenAlive)
        //    spriteRenderer.sprite = deadSprite;

    }
}
