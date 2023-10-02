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
    SpriteRenderer spriteRenderer;
    ParticleSystem paSy;
    public Sprite deadSprite;
    public Sprite livSprite;


    private void Start()
    {
    }

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        paSy ??= GetComponent<ParticleSystem>();
        //spriteRenderer.enabled = alive;


        if (alive && !nxtGenAlive)
        {
            //Color newC = new Color(1,1,1, 0.5f);
            //spriteRenderer.color += newC;
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
