using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkLobbyManager {

    public static MultiplayerManager Instance { get; private set; }

    public RectTransform mainMenuPanel;
    public RectTransform multiplayerPanel;
    public RectTransform LobbyPanel;

    public MenuManager MenuManager;

    private int _playerCount = 0;

	// Use this for initialization
	void Start () {
	    Instance = this;
	    //currentActivePanel = mainMenuPanel;

        DontDestroyOnLoad(gameObject);
	}

    public override void OnLobbyClientSceneChanged(NetworkConnection connection) {
        
        //if(SceneManager.Get)

    }

    public override void OnStartHost() {
        base.OnStartHost();

        MenuManager.SwitchPanel(LobbyPanel);
    }

    public void CancelClientConnection()
    {
        StopClient();
        if (MenuManager.CurrentlyMatchmaking) {
            StopMatchMaker();
        }
    }

    public void CancelHostConnection()
    {

    }

    public void CancelServerConnection() {
        StopServer();
        MenuManager.SwitchPanel(mainMenuPanel);
    }

    public void OnPlayerCountChange(int i) {

        //increment server player count
        _playerCount += i;

        //count how many local players were added initially
        var localPlayerCount = 0;
        foreach (var p in ClientScene.localPlayers) {
            localPlayerCount += (p == null || p.playerControllerId != -1) ? 0 : 1;
        }
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId) {

        var prefab = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

        foreach (var netLobbyPlayer in lobbySlots) {
            var player = netLobbyPlayer as LobbyPlayer;
            if (player != null) {
                //update other players with whatever is needed.
            }
        }

        return prefab;
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        MenuManager.InfoPanel.gameObject.SetActive(false);
        
        if (!NetworkServer.active) {
            MenuManager.SwitchPanel(LobbyPanel);
        }
    }



}
