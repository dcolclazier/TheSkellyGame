using UnityEngine;
using UnityEngine.Networking;

public class PlayerLobbyHook : LobbyHook {
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {

        if (lobbyPlayer == null) return;

        var lp = lobbyPlayer.GetComponent<LobbyPlayer>();
        if(lp!= null)
            GameManager.AddPlayer(gamePlayer,lp.slot,lp.PlayerColor,lp.NameInput.text,lp.playerControllerId);

    }
}