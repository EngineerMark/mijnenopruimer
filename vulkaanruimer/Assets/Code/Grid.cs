using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid
{
    public int sizeX;
    public int sizeY;

    private GridTile[,] tileArray;

    public GridTile[,] Generate(int sizeX, int sizeY)
    {
        GameObject parentGo = new GameObject("GridGroup");
        GridTile[,] newArray = new GridTile[sizeX, sizeY];


        //Generate map
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                bool createBomb = (Random.Range(0, 15) == 1);

                GameObject go = GameObject.Instantiate(createBomb ? GameManager.instance.bombTile : GameManager.instance.regTile);
                go.transform.position = new Vector3(x, 0, y);
                go.transform.parent = parentGo.transform;

                GridTile newTile = go.AddComponent<GridTile>();
                newTile.isBomb = createBomb;
                newTile.tileObject = go;

                Debug.Log("New tile generated at " + go.transform.position);

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

        parentGo.transform.position = new Vector3(-sizeX * 0.5f + 1, -sizeY * 0.5f + 1);

        return newArray;
    }
}
