using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid
{
    public int sizeX;
    public int sizeY;

    private GridTile[,] tileArray;

    public bool interactible = false;

    private List<Vector2Int> bombPositions;
    private GameObject gridParent;

    private int flagsLeft = 40;

    public int FlagsLeft { get => flagsLeft; set => flagsLeft = value; }

    // End game scoring
    public int correctFlags;
    public int falsePositives;
    public int flagsUsed;

    public float timer;

    public void Generate(int sizeX, int sizeY, int bombs)
    {
        GameManager.instance.StartCoroutine(InternalGenerate(sizeX, sizeY, bombs));
    }

    public void Update()
    {
        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        UIManager.instance.gameTimerText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
    }

    private IEnumerator InternalGenerate(int sizeX, int sizeY, int bombs)
    {
        interactible = false;
        gridParent = new GameObject("GridGroup");
        GridTile[,] newArray = new GridTile[sizeX, sizeY];

        this.sizeX = sizeX;
        this.sizeY = sizeY;

        bombPositions = new List<Vector2Int>();
        int[,] localTempBombPos = new int[sizeX, sizeY];
        //Generate bomb field first
        for (int i = 0; i < bombs; i++)
        {
            int x = UnityEngine.Random.Range(0, sizeX - 1);
            int y = UnityEngine.Random.Range(0, sizeY - 1);
            bombPositions.Add(new Vector2Int(x, y));
            localTempBombPos[x, y] = 1;
        }

        //Generate map
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                //bool createBomb = (Random.Range(0, bombChance) == 1);
                bool createBomb = localTempBombPos[x, y] == 1;
                GameObject go = GameObject.Instantiate(createBomb ? GameManager.instance.bombTile : GameManager.instance.regTile);
                go.transform.position = new Vector3(x, 0, y);
                go.transform.parent = gridParent.transform;

                GridTile newTile = go.AddComponent<GridTile>();
                newTile.isBomb = createBomb;
                newTile.tileObject = go;

                //Debug.Log("New tile generated at " + go.transform.position);
                newTile.GridPosition = new Vector2Int(x, y);
                newTile.parentGrid = this;
                newArray[x, y] = newTile;
                //newTile.ExposeTile();
            }
        }

        //Count bombs for each tile
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int xCheck = -1; xCheck <= 1; xCheck++)
                {
                    for (int yCheck = -1; yCheck <= 1; yCheck++)
                    {
                        if (xCheck == x && yCheck == y) continue;
                        if (x + xCheck < 0 || x + xCheck >= sizeX) continue;
                        if (y + yCheck < 0 || y + yCheck >= sizeY) continue;
                        if (newArray[x + xCheck, y + yCheck].isBomb)
                            newArray[x, y].bombCount++;
                    }
                }
            }
        }

        gridParent.transform.position = new Vector3(-sizeX * 0.5f + 1, 0, -sizeY * 0.5f + 1);

        tileArray = newArray;
        interactible = true;
        yield return null;

    }

    public IEnumerator BombRevealer()
    {
        List<GridTile> markedTiles = GetMarkedTiles();
        for (int i = 0; i < bombPositions.Count; i++)
        {
            GridTile tile = GetTile(bombPositions[i].x, bombPositions[i].y);
            if (tile.isBomb)
            {
                if (tile.marked)
                    correctFlags++;
                tile.incorrectFlagDisplay.SetActive(true);
                tile.ExposeTile(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
        flagsUsed = markedTiles.Count;
        foreach (GridTile tile in markedTiles)
        {
            if (!tile.isBomb)
            {
                falsePositives++;
                tile.markerDisplay.SetActive(false);
                tile.incorrectFlagDisplay.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
        UIManager.instance.UpdateScoreUI();
    }

    public void KillTiles()
    {
        //For game over or dev
        //Remove all tile objects
        for (int x = 0; x < tileArray.GetLength(0); x++)
        {
            for (int y = 0; y < tileArray.GetLength(1); y++)
            {
                GameObject.Destroy(tileArray[x, y].gameObject);
            }
        }
        GameObject.Destroy(gridParent);
    }

    public List<GridTile> GetMarkedTiles()
    {
        List<GridTile> tiles = new List<GridTile>();
        foreach (GridTile tile in tileArray)
        {
            if (tile.marked)
                tiles.Add(tile);
        }
        return tiles;
    }

    public int GetLeftTilesCount(){
        int val = 0;
        foreach(GridTile tile in tileArray){
            if (!tile.isBomb && !tile.unlocked)
                val++;
        }
        return val;
    }

    public GridTile GetTile(int x, int y)
    {
        return tileArray[x, y];
    }
}
