using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    public Text flagCounterText;

    public Text correctFlagText;
    public Text falsePositivesText;
    public Text flagsUsedText;

    private bool currentFullscreen = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Generate list of resolutions 
        resolutionDropdown.options = new List<Dropdown.OptionData>();
        foreach (Resolution res in Screen.resolutions){
            resolutionDropdown.options.Add(new Dropdown.OptionData() { text = res.width + "x" + res.height });
        }

        if(PlayerPrefs.HasKey("window_resX")){
            int resX = PlayerPrefs.GetInt("window_resX");
            int resY = PlayerPrefs.GetInt("window_resY");
            Screen.SetResolution(resX, resY, false);
            int dropdownValue = 0;
            for(int i=0;i<resolutionDropdown.options.Count;i++){
                var resItem = resolutionDropdown.options[i];
                if (resItem.text==resX+"x"+resY){
                    dropdownValue = i;
                    break;
                }
            }
            resolutionDropdown.value = dropdownValue;
        }

        resolutionDropdown.onValueChanged.AddListener(delegate
        {
            SwitchResolution(resolutionDropdown);
        });

        fullscreenToggle.isOn = PlayerPrefs.HasKey("window_Fullscreen")?PlayerPrefs.GetInt("window_Fullscreen")==1:true;
        Screen.fullScreen = fullscreenToggle.isOn;
        currentFullscreen = fullscreenToggle.isOn;
        fullscreenToggle.onValueChanged.AddListener(delegate
        {
            ToggleFullscreen(fullscreenToggle);
        });
    }

    public void UpdateScoreUI(){
        flagCounterText.text = ""+GameManager.instance.GameGrid.FlagsLeft;

        correctFlagText.text = "Correct flags: " + GameManager.instance.GameGrid.correctFlags;
        falsePositivesText.text = "False positives: " + GameManager.instance.GameGrid.falsePositives;
        flagsUsedText.text = "Flags used: " + GameManager.instance.GameGrid.flagsUsed;
    }

    public void SwitchResolution(Dropdown dropdown){
        string[] vals = dropdown.captionText.text.Split('x');
        int resX = Convert.ToInt32(vals[0]);
        int resY = Convert.ToInt32(vals[1]);
        Screen.SetResolution(resX, resY, currentFullscreen);
        PlayerPrefs.SetInt("window_resX", resX);
        PlayerPrefs.SetInt("window_resY", resY);
    }

    public void ToggleFullscreen(Toggle toggle){
        Screen.fullScreen = toggle.isOn;
        currentFullscreen = toggle.isOn;
        PlayerPrefs.SetInt("window_Fullscreen", toggle.isOn ? 1 : 0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
