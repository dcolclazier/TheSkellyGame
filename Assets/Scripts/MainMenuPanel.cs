using UnityEngine;
using System.Collections;

public class MainMenuPanel : MonoBehaviour {

    public MenuManager MenuManager;

    public RectTransform MultiplayerPanel;
    public RectTransform SettingsPanel;
    
	public void OnClickSinglePlayer()
    {

    }

    public void OnClickMultiPlayer()
    {
        MenuManager.SwitchPanel(MultiplayerPanel);
    }
    public void OnClickSettings()
    {
        MenuManager.SwitchPanel(SettingsPanel);
    }
    public void OnClickExit()
    {

    }

}
