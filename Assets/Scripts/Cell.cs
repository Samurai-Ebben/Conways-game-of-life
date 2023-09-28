using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool nxtGenAlive;
    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = alive;
    }

    public void GetNeighborsCount(int x, int y)
    {
        int col = GameOfLife.instance.numberOfColums;
        int row = GameOfLife.instance.numberOfRows;
        int aliveNeighborsCount = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                //Skip me, ie the current cell.
                if (i == x && j == y)
                    continue;

                if (i >= 0 && i < col && j >= 0 && j < row) //Issue
                    aliveNeighborsCount++;
            }
        }

        //alive = nxtGenAlive;
        nxtGenAlive = alive;

         if (alive)
         {
            Debug.Log(aliveNeighborsCount);
             if (aliveNeighborsCount < 2 || aliveNeighborsCount > 3)
                 nxtGenAlive = false;
             else
                 nxtGenAlive = true;
         }
         else
         {
             if (aliveNeighborsCount == 3)
                 nxtGenAlive = true;
         }
         UpdateStatus();
    }
}
