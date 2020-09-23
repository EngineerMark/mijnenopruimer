using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    void Start()
    {
        //Generate list of resolutions 
        resolutionDropdown.options = new List<Dropdown.OptionData>();
        foreach (Resolution res in Screen.resolutions){
            resolutionDropdown.options.Add(new Dropdown.OptionData() { text = res.width + "x" + res.height });
        }

        fullscreenToggle.onValueChanged.AddListener(delegate
        {
            ToggleFullscreen(fullscreenToggle);
        });
    }

    public void ToggleFullscreen(Toggle toggle){
        Screen.fullScreen = toggle.isOn;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
