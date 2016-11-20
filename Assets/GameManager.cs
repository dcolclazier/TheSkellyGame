using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public static GameManager Instance;
    public static List<PlayerManager> Players = new List<PlayerManager>();

    public CameraControl CameraControl;

    [SyncVar] public bool GameIsFinished = false;

    private WaitForSeconds _startWait;
    private WaitForSeconds _endWait;
    private float StartDelay = 3.0f;
    private float EndDelay = 3.0f;

    public void Awake() {
        Instance = this;
    }

    [ServerCallback]
    private void Start() {
        _startWait = new WaitForSeconds(StartDelay);
        _endWait = new WaitForSeconds(EndDelay);
    }

    public static void AddPlayer(GameObject player, int playerNumber, Color color, string playerName, int localId) {

        var newManager = player.GetComponent<PlayerManager>();
        newManager.Player = player;
        newManager.PlayerNumber = playerNumber;
        newManager.PlayerColor = color;
        newManager.PlayerName = playerName;
        newManager.LocalPlayerId = localId;
        newManager.Init();


        //var manager = new PlayerManager {
        //    Player = player,
        //    PlayerNumber = playerNumber,
        //    PlayerColor = color,
        //    PlayerName = playerName,
        //    LocalPlayerId = localId
        //};
        //manager.Init();
        Debug.Log("Adding player.");
        Players.Add(newManager);
    }

    public void RemovePlayer(GameObject player) {
        var toRemove = Players.FirstOrDefault(p => p.Player == player);

        if (toRemove != null) Players.Remove(toRemove);
    }

    public IEnumerator GameLoop() {
        while (Players.Count < 2) yield return null; //shouldn't this be player limit?

        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(GameStarting());

        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(GameEnding());
    }

    private IEnumerator GameEnding() {

        RpcRoundEnding();
        yield return _endWait;


    }
    [ClientRpc]
    private void RpcRoundEnding() {
        

    }

    private IEnumerator GamePlaying() {
        RpcGamePlaying();

        yield return null;
    }

    

    [ClientRpc]
    private void RpcGamePlaying() {
        EnablePlayerControl();
    }

    private void EnablePlayerControl() {
        foreach (var playerManager in Players) {
            playerManager.EnablePlayerControl();
        }

    }

    private IEnumerator GameStarting() {

        RpcGameStarting();
        yield return _startWait;
    }

    [ClientRpc]
    private void RpcGameStarting() {

        ResetAllPlayers();
        DisablePlayerControl();



    }

    private void ResetAllPlayers() {
        

    }

    private void DisablePlayerControl() {
        foreach (var playerManager in Players) {
            playerManager.DisablePlayerControl();
        }
    }

    public PlayerManager GetPlayer(GameObject playerGameObject) {
        return Players.FirstOrDefault(player => player.Player == playerGameObject);
    }
}