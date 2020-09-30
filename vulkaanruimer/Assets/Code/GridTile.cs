using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    public bool isBomb;
    public bool unlocked = false;
    public bool marked = false;
    public GameObject tileObject = null;
    public int bombCount = 0;
    private Vector2Int gridPosition;

    public GameObject numericDisplay;
    public GameObject markerDisplay;
    public GameObject incorrectFlagDisplay;

    public Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }

    public Grid parentGrid;

    private void Awake()
    {
        numericDisplay = transform.Find("IconDisplay/NumericDisplay").gameObject;
        markerDisplay = transform.Find("IconDisplay/MarkerDisplay").gameObject;
        incorrectFlagDisplay = transform.Find("IconDisplay/IncorrectFlagDisplay").gameObject;
    }

    private void Update()
    {
    }

    public void ExposeTile(bool death = false)
    {
        unlocked = true;
        Transform hiddenChild = tileObject.transform.Find("Hidden");
        hiddenChild.gameObject.SetActive(true);

        //Unmark if marked
        if (marked)
            MarkTile();

        //Check for surrounding bombs
        if (bombCount > 0 && !isBomb)
        {
            numericDisplay.SetActive(true);
            Text t = numericDisplay.GetComponent<Text>();
            t.text = bombCount + "";
            t.color = GameManager.instance.neighborValueColor[bombCount];
        }

        //Check if clicked on bomb
        if (isBomb && !death)
        {
            GameManager.instance.GameOver();
            return;
        }

        //Check if all tiles are uncovered after this one
        if (GameManager.instance.GameGrid.GetLeftTilesCount() == 0 && !death)
        {
            GameManager.instance.GameWin();
            return;
        }

        //Uncover surrounding tiles
        if (bombCount == 0 && !death)
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

    private void MarkTile()
    {
        bool state = marked;

        if (state == true && GameManager.instance.GameGrid.FlagsLeft >= 0)
        {
            marked = false;
            GameManager.instance.GameGrid.FlagsLeft++;
            AudioManager.instance.Play(AudioManager.instance.flagPlacementSFX);
        }
        else if (state == false && GameManager.instance.GameGrid.FlagsLeft >= 1)
        {
            marked = true;
            GameManager.instance.GameGrid.FlagsLeft--;
            AudioManager.instance.Play(AudioManager.instance.flagPlacementSFX);
        }

        markerDisplay.SetActive(marked);
        UIManager.instance.UpdateScoreUI();
    }

    public void OnMouseOver()
    {
        if (unlocked || !parentGrid.interactible)
            return;
        if (Input.GetMouseButtonDown(GameManager.instance.revealMouseButton))
            ExposeTile();
        else if (Input.GetMouseButtonDown(GameManager.instance.markerMouseButton))
            MarkTile();
    }

    public void OnMouseEnter()
    {

    }

    public void OnMouseExit()
    {

    }
}
