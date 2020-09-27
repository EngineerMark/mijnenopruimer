using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid
{
    public int sizeX;
    public int sizeY;

    private GridTile[,] tileArray;

    public bool interactible = true;

    private List<Vector2Int> bombPositions;
    private GameObject gridParent;


    public void Generate(int sizeX, int sizeY, int bombs)
    {
        GameManager.instance.StartCoroutine(InternalGenerate(sizeX, sizeY, bombs));
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
            int x = Random.Range(0, sizeX - 1);
            int y = Random.Range(0, sizeY - 1);
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
                        if (x + xCheck < 0 || x + xCheck >= sizeX - 1) continue;
                        if (y + yCheck < 0 || y + yCheck >= sizeY - 1) continue;
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
        for (int i = 0; i < bombPositions.Count; i++)
        {
            if (GetTile(bombPositions[i].x, bombPositions[i].y).isBomb)
            {
                GetTile(bombPositions[i].x, bombPositions[i].y).ExposeTile(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
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

    public GridTile GetTile(int x, int y)
    {
        return tileArray[x, y];
    }
}
