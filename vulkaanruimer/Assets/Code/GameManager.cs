using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gridTile;

    public Vector2 gridSize;

    public void Start()
    {
        // Create a testgrid

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Instantiate(gridTile, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
}
