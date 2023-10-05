using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class GameOfLife : MonoBehaviour
{
    public static GameOfLife instance;
    public GameObject cellPrefab;
    Cell[,] cells;

    [HideInInspector]public int numberOfColums, numberOfRows;
    [SerializeField]float cellSize = 0.2f; //Size of our cells
    [SerializeField]int spawnChancePercentage = 15;

    [Range(1,560)]
    public int frameRate = 4;

    int genNum=0;

    int prevGeneration = -1;
    int currGeneration = 0;
    int stableGenCount = 0;
    int maxStableGen = 5;



    Vector3 mousePos; 
    Vector3 worldMousePos;

    //int stillLifeCount = 0, oscillators2Pcount =0;
    bool stable = false;

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
        for (int y = 0; y < numberOfRows; y++)
        {
            //for each column in each row
            for (int x = 0; x < numberOfColums; x++)
            {
                //Create our game cell objects, multiply by cellSize for correct world placement
                Vector2 newPos = new Vector2(x * cellSize - Camera.main.orthographicSize *
                    Camera.main.aspect,
                    y * cellSize - Camera.main.orthographicSize);

                var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);
                newCell.transform.localScale = Vector2.one * cellSize;
                cells[x, y] = newCell.GetComponent<Cell>();
                cells[x, y].alive = false;

                //Random check to see if it should be alive
                //Change this and put it in update.
                //if (Random.Range(0, 100) < spawnChancePercentage)
                //{
                //    cells[x, y].alive = true;
                //}

                cells[x, y].UpdateStatus();
            }
        }
       
        Application.targetFrameRate = 1;
        Time.timeScale = 0;

    }


    void Update()
    {
        ZoomInNOut();

        //TODO: Get mouse pos when cliked, alive cell on pos.

        if (Input.GetMouseButtonDown(0) && Time.deltaTime<1)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int gridX = Mathf.Clamp((int)((mouseWorldPos.x + Camera.main.orthographicSize * Camera.main.aspect)
                / cellSize), 0, numberOfColums - 1);
            int gridY = Mathf.Clamp((int)((mouseWorldPos.y + Camera.main.orthographicSize) / cellSize), 0, numberOfRows - 1);

            // Toggle the state of the clicked cell
            cells[gridX, gridY].alive = !cells[gridX, gridY].alive;
            cells[gridX, gridY].StartStatus();
        }

            
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 1;
        }
        if (Time.timeScale < 1) return;

        Application.targetFrameRate = frameRate;
        CalculateNextGen();
        CheckStability();
        UpdateCells();


    }

    private void CalculateNextGen()
    {
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
    }

    void CheckStability()
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                if (cells[x, y].alive)
                    prevGeneration++;
                if (cells[x, y].nxtGenAlive)
                    currGeneration++;
            }
        }
        if (prevGeneration == currGeneration)
            stableGenCount++;
        else
            stableGenCount = 0;

        prevGeneration = currGeneration;
        if(stableGenCount >= maxStableGen && !stable)
        {
            int stableNum = genNum - maxStableGen;
            Debug.Log("The game is stable at generation number: " + stableNum);
            stable = true;

        }
    }

    private void UpdateCells()
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                cells[x, y].UpdateStatus();

                cells[x, y].alive = cells[x, y].nxtGenAlive;
            }
        }
        //if (!isStable)
        genNum++;
    }

    private void ZoomInNOut()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            ZoomOut();
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            ZoomIn();
            mousePos.z = -10;
            

            worldMousePos.x = Mathf.Clamp(worldMousePos.x, -numberOfColums, numberOfColums);
            worldMousePos.y = Mathf.Clamp(worldMousePos.y, -numberOfRows, numberOfRows);

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, worldMousePos, Time.deltaTime * 2);

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
        int[] neighborsX = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] neighborsY = { -1, 0, 1, -1, 1, -1, 0, 1 };

        //2- repeating patterns(ships and blinkers).
        for (int i = 0; i < 8; i++)
        {
            int newX = (x + neighborsX[i] + numberOfColums) % numberOfColums;
            int newY = (y + neighborsY[i] + numberOfRows) % numberOfRows;

            //cells[x, y].paSy.Play();
            // Check if the neighbor is alive and wrap
            if (cells[newX, newY].alive)
            {
                //cells[x, y].paSy.Stop();

                aliveNeighborsCount++;
            }

        }

        return aliveNeighborsCount;
    }

    void ZoomIn()
    {
        Camera.main.orthographicSize --;
    }
    void ZoomOut()
    {
        Camera.main.orthographicSize ++;

    }

}