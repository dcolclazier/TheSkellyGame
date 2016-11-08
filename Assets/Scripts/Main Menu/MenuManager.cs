using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public RectTransform MainMenuPanel;
    public RectTransform MultiplayerPanel;
    public RectTransform SettingsPanel;
    public RectTransform LobbyPanel;
    public InfoPanel InfoPanel; 

    private RectTransform _currentPanel;

    public bool CurrentlyInGame { get; set; }
    public bool CurrentlyMatchmaking { get; set; }

    // Use this for initialization
	void Start () {
	    _currentPanel = MainMenuPanel;
	}
	
	public void SwitchPanel(RectTransform activePanel) {

        if (_currentPanel != null) _currentPanel.gameObject.SetActive(false);
        if (activePanel != null) activePanel.gameObject.SetActive(true);

	    _currentPanel = activePanel;
	}

    public void DisplayInfoPanel(string infoText, string buttonText, UnityEngine.Events.UnityAction cancelAction) {
        
        InfoPanel.Display(infoText, buttonText, cancelAction);

    }

    
}
