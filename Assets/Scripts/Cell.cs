using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool nxtGenAlive;
    [SerializeField] public  int neighbors;
    SpriteRenderer spriteRenderer;

    


    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = alive;
    }
}
