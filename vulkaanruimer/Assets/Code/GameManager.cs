using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gridTile;

    public Vector2Int gridSize;
    public int bombChance;

    public GameDifficulty gameDifficulty;

    private Grid gameGrid;

    public GameObject bombTile;
    public GameObject regTile;

    public Grid GameGrid { get => gameGrid; set => gameGrid = value; }

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        GameGrid = new Grid();
        //gameGrid.Generate(gridSize.x, gridSize.y);
    }

    //Three seperate methods for easier usage in button interaction.
    public void SetX(int width){
        gridSize.x = width;
    }

    public void SetY(int height){
        gridSize.y = height;
    }

    public void SetBombChance(int bombChance){
        this.bombChance = bombChance;
    }

    public void Play(){
        GameGrid.Generate(gridSize.x,gridSize.y,bombChance);
    }
}
