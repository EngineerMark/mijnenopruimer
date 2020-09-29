using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public Slider loadingBar;

    public void Start()
    {
        StartCoroutine(SceneLoader());
    }

    private IEnumerator SceneLoader(){
        AsyncOperation op = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while(!op.isDone){
            loadingBar.value = op.progress;
            yield return null;
        }
    }
}
