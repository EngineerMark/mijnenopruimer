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

    public void ExposeTile(){
        unlocked = true;
        Transform hiddenChild = tileObject.transform.Find("Hidden");
        hiddenChild.gameObject.SetActive(true);
    }
}
