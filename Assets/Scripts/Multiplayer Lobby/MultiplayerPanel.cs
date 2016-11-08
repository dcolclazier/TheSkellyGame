using UnityEngine;
using UnityEngine.UI;

public class MultiplayerPanel : MonoBehaviour {

    private PanelManager _panelManager = PanelManager.Instance;
    private MultiplayerManager _multiplayerManager = MultiplayerManager.Instance;
    

    public PublicGameList PublicGameList;

    public InputField MultiplayerGameName;
    public InputField MultiplayerHostAddress;
    public Dropdown SlotCountDropdown;

    public void OnEnable() {
        
        //_multiplayerManager.StartMatchMaker();
        if (_multiplayerManager == null) _multiplayerManager = FindObjectOfType<MultiplayerManager>();
        if (_panelManager == null) _panelManager = FindObjectOfType<PanelManager>();
    }
	
	public void OnClickBack() {
        
        // unload all multiplayer stuff...
        _panelManager.SwitchPanel(_panelManager.MainMenuPanel);
    }

    public void OnClickHost() {
        _multiplayerManager.StartHost();
    }

    public void OnClickConnect() {
        
        _panelManager.SwitchPanel(_panelManager.LobbyPanel);

        _multiplayerManager.networkAddress = MultiplayerHostAddress.text;
        _multiplayerManager.StartClient();

        _panelManager.DisplayInfoPanel("Connecting...", "Cancel", _multiplayerManager.CancelClientConnection);

    }
    public void OnClickCreateMpGame() {

        _multiplayerManager.SetMatchHost(_multiplayerManager.matchHost,_multiplayerManager.matchPort,true);

        _multiplayerManager.maxPlayers = SlotCountDropdown.value + 2;
        _multiplayerManager.matchMaker.CreateMatch(MultiplayerGameName.text, (uint) _multiplayerManager.maxPlayers, true, "", "", "", 0, 0,
            _multiplayerManager.OnMatchCreate);

        _panelManager.CurrentlyMatchmaking = true;
        _panelManager.DisplayInfoPanel("Creating match...", "Cancel", _multiplayerManager.CancelHostConnection);
     
    }

    public void OnClickStartServer() {
        
        _panelManager.SwitchPanel(null);
        _multiplayerManager.StartServer();
        _panelManager.DisplayInfoPanel("Server running...", "Cancel", _multiplayerManager.StopServer);
    }

    public void OnClickRefreshPublicGames() {
        
        PublicGameList.ClearGames();
        PublicGameList.RequestPage(PublicGameList.CurrentPage);
    }
}
