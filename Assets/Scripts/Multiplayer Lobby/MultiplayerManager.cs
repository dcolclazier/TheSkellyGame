using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkLobbyManager {

    public static MultiplayerManager Instance { get; private set; }

    public RectTransform MainMenuPanel;
    public RectTransform MultiplayerPanel;
    public RectTransform LobbyPanel;
    public MenuManager MenuManager;
    public LobbyCountdownPanel CountdownPanel;

    private int _playerCount = 0;
    private float _prematchCountdown = 5;

    // Use this for initialization
	void Start () {
	    Instance = this;
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
        if (MenuManager.CurrentlyMatchmaking)
        {
            StopMatchMaker();
        }
    }

    public void CancelHostConnection() {
        StopHost();
        if (MenuManager.CurrentlyMatchmaking)
        {
            StopMatchMaker();
        }
    }

    public void CancelServerConnection()
    {
        StopServer();
        if (MenuManager.CurrentlyMatchmaking)
        {
            StopMatchMaker();
        }
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

    
    public override void OnLobbyServerPlayersReady() {}

    public void StartLobbyGame() {
        var allReady = true;
        foreach (var player in lobbySlots) {
            if (player == null) continue;
            if (player.readyToBegin == false) {
                allReady = false;
                Debug.Log("Found a player who isn't ready " + player.name);
            }
            else Debug.Log("Found ready player.");
        }

        if (allReady) StartCoroutine(ServerCountdownCoroutine());
    }
    public IEnumerator ServerCountdownCoroutine()
    {
        float remainingTime = _prematchCountdown;
        int floorTime = Mathf.FloorToInt(remainingTime);

        while (remainingTime > 0)
        {
            yield return null;

            remainingTime -= Time.deltaTime;
            var newFloorTime = Mathf.FloorToInt(remainingTime);
            if (newFloorTime == floorTime) continue;
//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
            floorTime = newFloorTime;

            foreach (var player in lobbySlots) {
//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                var lobbyPlayer = player as LobbyPlayer;
                if (lobbyPlayer != null)
                    lobbyPlayer.RpcUpdateCountdown(floorTime);
            }
        }

        foreach (var player in lobbySlots) {
            var lobbyPlayer = player as LobbyPlayer;
            if (lobbyPlayer != null) lobbyPlayer.RpcUpdateCountdown(0);
        }

        ServerChangeScene(playScene);
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId) {

        var prefab = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

        foreach (var netLobbyPlayer in lobbySlots) {
            var player = netLobbyPlayer as LobbyPlayer;
            if (player != null) {
                player.PlayerReady = false;
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
