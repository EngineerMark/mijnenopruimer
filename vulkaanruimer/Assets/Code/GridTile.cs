using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public bool isBomb;
    public bool unlocked = false;
    public bool marked = false;
    public GameObject tileObject = null;
    public int bombCount = 0;
    private Vector2Int gridPosition;

    public Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }

    public void ExposeTile(){
        unlocked = true;
        Transform hiddenChild = tileObject.transform.Find("Hidden");
        hiddenChild.gameObject.SetActive(true);

        if (bombCount == 0)
        {
            // Lets recursive check surrounding tiles
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (GridPosition.x + x < 0 || GridPosition.y + y < 0) continue;

                    if (GridPosition.x + x >= GameManager.instance.GameGrid.sizeX || 
                        GridPosition.y + y >= GameManager.instance.GameGrid.sizeY) continue;

                    GridTile neighbor = GameManager.instance.GameGrid.GetTile(GridPosition.x + x, GridPosition.y + y);
                    if (neighbor.isBomb) continue;
                    if (neighbor.unlocked) continue;
                    neighbor.ExposeTile();
                }
            }
        }
    }

    public void OnMouseDown()
    {
        ExposeTile();
    }

    public void OnMouseEnter()
    {
        
    }

    public void OnMouseExit()
    {
        
    }
}
