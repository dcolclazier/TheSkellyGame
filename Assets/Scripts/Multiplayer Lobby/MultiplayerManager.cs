using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MultiplayerManager : NetworkLobbyManager {

    
    
    public bool CurrentlyInGame { get; set; }
    public bool CurrentlyMatchmaking { get; set; }
    private const float PrematchCountdown = 5;

    private RectTransform _currentPanel;

    public readonly List<Color> Colors = new List<Color> { Color.magenta, Color.black, Color.cyan, Color.blue, Color.green, Color.yellow };
    public List<Color> ColorsInUse { get { return _colorsInUse; }}
    private readonly List<Color> _colorsInUse = new List<Color>();

    public static MultiplayerManager Instance;

    public MatchInfo CurrentMatchInfo { get; private set; }

    public RectTransform StartLobbyGameBtn;
    public RectTransform MainMenuPanel;
    public RectTransform MultiplayerPanel;
    public RectTransform SettingsPanel;
    public RectTransform LobbyPanel;
    public RectTransform TitlePanel;
    public LobbyCountdownPanel CountdownPanel;
    public InfoPanel InfoPanel;
    private readonly Dictionary<NetworkConnection, GameObject> _lobbyPlayers = new Dictionary<NetworkConnection, GameObject>();
    private readonly Dictionary<NetworkConnection, GameObject> _gamePlayers = new Dictionary<NetworkConnection, GameObject>();

    // Use this for initialization
    void Start () {
	    Instance = this;
        DontDestroyOnLoad(gameObject);
        SwitchPanel(MainMenuPanel);
	}

    public override void OnLobbyClientSceneChanged(NetworkConnection connection) {
        Deactivate();
    }
    public override void OnLobbyClientDisconnect(NetworkConnection conn) {
        LeaveLobby();
    }
    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        base.OnMatchCreate(success, extendedInfo, matchInfo);

        CurrentMatchInfo = matchInfo;
        ColorsInUse.Clear();
    }
    public override void OnStartHost() {
        base.OnStartHost();

        SwitchPanel(LobbyPanel);
    }
    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();
        StartLobbyGameBtn.gameObject.SetActive(NetworkServer.active);
    }
    public override void OnLobbyServerPlayersReady() { } //intentionally left empty.
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        InfoPanel.gameObject.SetActive(false);

        if (!NetworkServer.active)
        {
            SwitchPanel(LobbyPanel);
        }
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId) {
        var prefab = Instantiate(gamePlayerPrefab.gameObject) as GameObject;
        prefab.GetComponent<SpriteRenderer>().color = _lobbyPlayers[conn].GetComponent<LobbyPlayer>().PlayerColor;
        _gamePlayers.Add(conn, prefab);

        return prefab;
    }
  

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        var prefab = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;
        foreach (var netLobbyPlayer in lobbySlots) {
            var player = netLobbyPlayer as LobbyPlayer;
            if (player != null)
                player.PlayerReady = false;
        }
        _lobbyPlayers.Add(conn, prefab);
        return prefab;
    }

    //Cancel Delegates
    public void CancelClientConnection() {
        StopClient();
    }
    public void SimpleCancel() {
        InfoPanel.gameObject.SetActive(false);
    }
    public void CancelHostConnection() {
        LeaveLobby();
    }


    public void OnPlayerCountChange(int i) {
        
    }


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
        float remainingTime = PrematchCountdown;
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

   
    public Color FirstAvailablePlayerColor()
    {
        foreach (var color in Colors)
        {
            if (ColorsInUse.Contains(color)) continue;
            return color;
        }
        return Color.white;
    }
    public void UpdateAvailableColors(LobbyPlayer player, Color color) {
        if (!ColorsInUse.Remove(player.PlayerColor) && player.PlayerColor != Color.white) {
            Debug.LogError("Tried to make a color available that was already available.");
        }
        if (ColorsInUse.Contains(color)) {
            Debug.LogError("Tried to make an unavailable color unavailable..." + color.GetName());
        } else ColorsInUse.Add(color);

    }

    
    //leave lobby and return to multiplayer screen
    public void LeaveLobby() {

        if (CurrentlyMatchmaking) {
            matchMaker.DestroyMatch(CurrentMatchInfo.networkId, 0, OnDestroyMatch);
            StopHost();
        }
        else if(NetworkServer.active) StopHost();
        else StopClient();

        SwitchPanel(MultiplayerPanel);

    }
    public void SwitchPanel(RectTransform activePanel)
    {
        if (_currentPanel != null) _currentPanel.gameObject.SetActive(false);
        if (activePanel != null) activePanel.gameObject.SetActive(true);
        _currentPanel = activePanel;
    }
    public void DisplayInfoPanel(string infoText, string buttonText, UnityEngine.Events.UnityAction cancelAction)
    {
        InfoPanel.Display(infoText, buttonText, cancelAction);
    }
    public void Deactivate()
    {
        if (_currentPanel != null) _currentPanel.gameObject.SetActive(false);

        MainMenuPanel.gameObject.SetActive(false);
        MultiplayerPanel.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(false);
        LobbyPanel.gameObject.SetActive(false);
        CountdownPanel.gameObject.SetActive(false);
        TitlePanel.gameObject.SetActive(false);
    }
    
}


