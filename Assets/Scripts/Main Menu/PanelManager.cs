using UnityEngine;

public class PanelManager : MonoBehaviour {

    public RectTransform MainMenuPanel;
    public RectTransform MultiplayerPanel;
    public RectTransform SettingsPanel;
    public RectTransform LobbyPanel;
    public RectTransform TitlePanel;
    public LobbyCountdownPanel CountdownPanel;
    public InfoPanel InfoPanel; 

    private RectTransform _currentPanel;

    public bool CurrentlyInGame { get; set; }
    public bool CurrentlyMatchmaking { get; set; }

    // Use this for initialization
	void Start () {
	    Instance = this;
        DontDestroyOnLoad(gameObject);
	    _currentPanel = MainMenuPanel;
	}

    private static PanelManager _instance;
    private static readonly object _lock = new object();
    public static PanelManager Instance {
        get {
            lock (_lock) return _instance;
        }
        private set { _instance = value; }
    }
    protected PanelManager() {}

    public void SwitchPanel(RectTransform activePanel) {

        if (_currentPanel != null) _currentPanel.gameObject.SetActive(false);
        if (activePanel != null) activePanel.gameObject.SetActive(true);

	    _currentPanel = activePanel;
	}

    public void DisplayInfoPanel(string infoText, string buttonText, UnityEngine.Events.UnityAction cancelAction) {
        
        InfoPanel.Display(infoText, buttonText, cancelAction);

    }

    public void Deactivate() {
        if(_currentPanel != null) _currentPanel.gameObject.SetActive(false);


        MainMenuPanel.gameObject.SetActive(false);
        MultiplayerPanel.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(false);
        LobbyPanel.gameObject.SetActive(false);
        CountdownPanel.gameObject.SetActive(false);
        TitlePanel.gameObject.SetActive(false);
    }
}
