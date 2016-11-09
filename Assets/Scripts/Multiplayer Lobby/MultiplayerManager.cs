using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MultiplayerManager : NetworkLobbyManager {

    private static MultiplayerManager _instance;
    private static readonly object _lock = new object();
    public static MultiplayerManager Instance {
        get {
            lock (_lock) {
                return _instance;
            }
        }
        private set { _instance = value; }
    }
    protected MultiplayerManager () { }
    public Button StartLobbyGameBtn;

    private int _playerCount = 0;
    private float _prematchCountdown = 5;

    private PanelManager _panelManager = PanelManager.Instance;

    // Use this for initialization
	void Start () {
	    Instance = this;
        DontDestroyOnLoad(gameObject);
	}

    public override void OnLobbyClientSceneChanged(NetworkConnection connection) {

        PanelManager.Instance.Deactivate();

    }

    public override void OnLobbyClientDisconnect(NetworkConnection conn) {

        if (NetworkServer.active) {
            Debug.Log("Destroying Host!");
            matchMaker.DestroyMatch(CurrentMatchInfo.networkId, 0, OnDestroyMatch);
            base.OnLobbyClientDisconnect(conn);
        }
        StopHost();
        _panelManager.SwitchPanel(_panelManager.MultiplayerPanel);
        //NetworkMatch.DropConnection(CurrentMatchInfo.networkId,)


    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        base.OnMatchCreate(success, extendedInfo, matchInfo);

        CurrentMatchInfo = matchInfo;
    }

    public MatchInfo CurrentMatchInfo { get; private set; }

    public override void OnStartHost() {
        base.OnStartHost();

        PanelManager.Instance.SwitchPanel(PanelManager.Instance.LobbyPanel);
    }

    public void CancelClientConnection()
    {
        if (NetworkServer.active) {
            //StopServer();
        }
        //if (PanelManager.Instance.CurrentlyMatchmaking)
        //    StopMatchMaker();
    }

    public void CancelHostConnection() {
        StopHost();
        if (PanelManager.Instance.CurrentlyMatchmaking)
            StopMatchMaker();
    }

    public void CancelServerConnection()
    {
        StopServer();
        if (PanelManager.Instance.CurrentlyMatchmaking)
            StopMatchMaker();
    }

    public void OnPlayerCountChange(int i) {
        ////why is this here?
        ////increment server player count
        //_playerCount += i;

        ////count how many local players were added initially
        //var localPlayerCount = 0;
        //foreach (var p in ClientScene.localPlayers) {
        //    localPlayerCount += (p == null || p.playerControllerId != -1) ? 0 : 1;
        //}
    }

    public override void OnLobbyClientEnter() {
        base.OnLobbyClientEnter();

        StartLobbyGameBtn.enabled = NetworkServer.active;
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
            //to avoid flooding the network with messages, we only send a notice to client when the number of plain seconds change.
            floorTime = newFloorTime;

            foreach (var player in lobbySlots) {
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

        PanelManager.Instance.InfoPanel.gameObject.SetActive(false);
        
        if (!NetworkServer.active) {
            PanelManager.Instance.SwitchPanel(PanelManager.Instance.LobbyPanel);
        }
    }


}
