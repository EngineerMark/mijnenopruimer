﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Grid Data")]
    public Color[] neighborValueColor;
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

    [Header("Misc")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject backgroundPanel;

    public Grid GameGrid { get => gameGrid; set => gameGrid = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        GameGrid = new Grid();
        //gameGrid.Generate(gridSize.x, gridSize.y);
    }

    public void Update()
    {
        if (GameGrid.interactible)
        {
            GameGrid.Update();
        }
    }

    public void ResetGame()
    {
        GameGrid.KillTiles();
        GameGrid = new Grid();
    }

    public void GameOver()
    {
        GameGrid.interactible = false;
        StartCoroutine(InternalGameOver());
    }

    public void GameWin()
    {
        GameGrid.interactible = false;
        StartCoroutine(InternalGameWin());
    }

    private IEnumerator InternalGameOver()
    {
        yield return GameGrid.BombRevealer();
        yield return new WaitForSeconds(2f);
        gameOverPanel.SetActive(true);
        backgroundPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        yield return null;
    }

    private IEnumerator InternalGameWin()
    {
        yield return GameGrid.BombRevealer();
        yield return new WaitForSeconds(2f);
        gameWinPanel.SetActive(true);
        backgroundPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        yield return null;
    }

    //Three seperate methods for easier usage in button interaction.
    public void SetX(int width)
    {
        gridSize.x = width;
    }

    public void SetY(int height)
    {
        gridSize.y = height;
    }

    public void SetBombCount(int bombCount)
    {
        this.bombCount = bombCount;
    }

    public void Play()
    {
        GameGrid.FlagsLeft = bombCount + 10;
        GameGrid.Generate(gridSize.x, gridSize.y, bombCount);
        backgroundPanel.SetActive(false);
    }
}
