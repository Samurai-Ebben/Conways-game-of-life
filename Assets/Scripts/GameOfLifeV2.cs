using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLifeV2 : MonoBehaviour
{
    public static GameOfLifeV2 instance;
    public GameObject cellPrefab;
    Cell[,] cells;

    public float cellSize = 0.15f; //Size of our cells
    [HideInInspector] public int numberOfColums, numberOfRows;
    int spawnChancePercentage = 15;

    [Range(1, 60)]
    public int frameRate = 4;



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;

        //Calculate our grid depending on size and cellSize

        numberOfColums = (int)Mathf.Floor((Camera.main.orthographicSize *
            Camera.main.aspect * 2) / cellSize);
        numberOfRows = (int)Mathf.Floor(Camera.main.orthographicSize * 2 / cellSize);

        //Initiate our matrix array
        cells = new Cell[numberOfColums, numberOfRows];

        //Create all objects

        //For each row
        //for (int y = 0; y < numberOfRows; y++)
        //{
        //    //for each column in each row
        //    for (int x = 0; x < numberOfColums; x++)
        //    {
        //        //Create our game cell objects, multiply by cellSize for correct world placement
        //        Vector2 newPos = new Vector2(x * cellSize - Camera.main.orthographicSize *
        //            Camera.main.aspect,
        //            y * cellSize - Camera.main.orthographicSize);

        //        var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);
        //        newCell.transform.localScale = Vector2.one * cellSize;
        //        cells[x, y] = newCell.GetComponent<Cell>();

        //        //Random check to see if it should be alive
        //        if (Random.Range(0, 100) < spawnChancePercentage)
        //        {
        //            cells[x, y].alive = true;
        //        }

        //        cells[x, y].UpdateStatus();
        //    }
        //}

    }

    void Update()
    {
        Application.targetFrameRate = frameRate;
        //TODO: Calculate next generation

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {

                int aliveNeighborsCount = GetAliveNeighborsCount(x, y);
                cells[x, y].neighbors = aliveNeighborsCount;
                if (cells[x, y].alive)
                {
                    if (aliveNeighborsCount < 2 || aliveNeighborsCount > 3)
                        cells[x, y].nxtGenAlive = false;
                    else
                        cells[x, y].nxtGenAlive = true;
                }
                else
                {
                    if (aliveNeighborsCount == 3)
                        cells[x, y].nxtGenAlive = true;
                    else
                        cells[x, y].nxtGenAlive = false;
                }
            }
        }

        // *Any live cell with fewer than two live aliveNeighborsCount dies.
        //TODO: update buffer

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                cells[x, y].alive = cells[x, y].nxtGenAlive;
                //cells[x, y].GetAliveNeighborsCount(x,y);
                cells[x, y].UpdateStatus();
            }
        }
    }


    /*int GetAliveNeighborsCount(int x, int y)
    {
        int aliveNeighborsCount = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                //Skip me, ie the current cell.
                if (i == x && j == y && cells[x, y].alive)
                    continue;

                if (i >= 0 && i < numberOfColums && j >= 0 && j < numberOfRows && cells[i, j].alive)
                    aliveNeighborsCount++;
            }
        }

        return aliveNeighborsCount;
    }
    */

    int GetAliveNeighborsCount(int x, int y)
    {
        int aliveNeighborsCount = 0;

        // Define relative positions of neighbors (including diagonal neighbors)
        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int newX = (x + dx[i] + numberOfColums) % numberOfColums;
            int newY = (y + dy[i] + numberOfRows) % numberOfRows;

            // Check if the neighbor is alive
            if (cells[newX, newY].alive)
            {
                aliveNeighborsCount++;
            }
        }

        return aliveNeighborsCount;
    }

}
