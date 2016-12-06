using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Settings : MonoBehaviour
{
    public MultiplayerManager NetManager;
    public Dropdown settings;
    public Slider audioS;
    public Button applyChanges;
    private float audioLevel = 100;
    private bool ignoreOnce = true; //For initilization purposes

    void Start()
    {
        if(QualitySettings.GetQualityLevel() == 0)
        {
            settings.value = 0;
        }
        else if(QualitySettings.GetQualityLevel() == 3)
        {
            settings.value = 1;
        }
        else if(QualitySettings.GetQualityLevel() == 5)
        {
            settings.value = 2;
        }
    }

    public void ApplyChanges()
    {
        if(settings.value == 0)
        {
           QualitySettings.SetQualityLevel(0, false);
        }
        else if(settings.value == 1)
        {
            QualitySettings.SetQualityLevel(3, false);
        }
        else
        {
            QualitySettings.SetQualityLevel(5, true);
        }
        audioLevel = audioS.value;
        applyChanges.gameObject.SetActive(false);
        NetManager.SwitchPanel(NetManager.MainMenuPanel);
    }

    public float getAudioValue()
    {
        return (audioLevel);
    }

    public void showApply()
    {
        if (!ignoreOnce)
        {
            print("Settings Changed");
            applyChanges.gameObject.SetActive(true);
        }
        else
            ignoreOnce = false;
    }

    public void ExitSettings()
    {
        applyChanges.gameObject.SetActive(false);
        NetManager.SwitchPanel(NetManager.MainMenuPanel);
    }
}
