using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gridTile;

    public Vector2Int gridSize;

    public GameDifficulty gameDifficulty;

    private Grid gameGrid;

    public GameObject bombTile;
    public GameObject regTile;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        gameGrid = new Grid();
        gameGrid.Generate(gridSize.x, gridSize.y);

        // Create a testgrid

        //for (int x = 0; x < gridSize.x; x++)
        //{
        //    for (int y = 0; y < gridSize.y; y++)
        //    {
        //        Instantiate(gridTile, new Vector3(x, 0, y), Quaternion.identity);
        //    }
        //}
    }
}
