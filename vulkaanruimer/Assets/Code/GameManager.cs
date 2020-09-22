using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Grid Data")]
    public GameObject gridTile;

    public Vector2Int gridSize;
    public int bombCount;

    public GameDifficulty gameDifficulty;

    private Grid gameGrid;


    [Header("Grid Input")]
    public int revealMouseButton;
    public int markerMouseButton;

    [Header("Prefabs")]
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

    public void GameOver(){
        GameGrid.interactible = false;

        //Reveal bombs
        StartCoroutine(GameGrid.BombRevealer());

        //Show deathscreen
    }

    //Three seperate methods for easier usage in button interaction.
    public void SetX(int width){
        gridSize.x = width;
    }

    public void SetY(int height){
        gridSize.y = height;
    }

    public void SetBombCount(int bombCount){
        this.bombCount = bombCount;
    }

    public void Play(){
        GameGrid.Generate(gridSize.x,gridSize.y, bombCount);
    }
}
