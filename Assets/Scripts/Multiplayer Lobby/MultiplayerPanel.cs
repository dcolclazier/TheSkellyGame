﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerPanel : MonoBehaviour {

    public MultiplayerManager NetManager;

    public PublicGameList PublicGameList;

    public InputField MultiplayerGameName;
    public InputField MultiplayerHostAddress;
    public InfoPanel InfoPanel;
    public Dropdown SlotCountDropdown;

    //void Start() {
    //    DontDestroyOnLoad(gameObject);
    //}

    public void OnEnable() {
        
        OnClickRefreshPublicGames();
    }
	
	public void OnClickBack() {

        // unload all multiplayer stuff...
        NetManager.SwitchPanel(NetManager.MainMenuPanel);
    }

    public void OnClickHost() {
        NetManager.StartHost();
    }

    public void OnClickConnect() {

        NetManager.SwitchPanel(NetManager.LobbyPanel);

        NetManager.networkAddress = MultiplayerHostAddress.text;
        NetManager.StartClient();

        NetManager.DisplayInfoPanel("Connecting...", "Cancel", NetManager.CancelClientConnection);

    }
    public void OnClickCreateMpGame() {

        //no blank names fix
        if (MultiplayerGameName.text == "") {
            NetManager.DisplayInfoPanel("Game name cannot be blank.", "Ok.", NetManager.SimpleCancel);
            return;
        }

        if (PublicGameList.LatestPublicGames.Any(game => MultiplayerGameName.text == game.name)) {
            NetManager.DisplayInfoPanel("Game name exists... Please try another.", "Ok.", NetManager.SimpleCancel);
            return;
        }

        NetManager.SetMatchHost(NetManager.matchHost,NetManager.matchPort,true);

        NetManager.maxPlayers = SlotCountDropdown.value + 2;
        NetManager.matchMaker.CreateMatch(MultiplayerGameName.text, (uint) NetManager.maxPlayers, true, "", "", "", 0, 0,
            NetManager.OnMatchCreate);

        NetManager.CurrentlyMatchmaking = true;
        NetManager.DisplayInfoPanel("Creating match...", "Cancel", NetManager.CancelHostConnection);
     
    }

    public void OnClickStartServer() {

        NetManager.SwitchPanel(null);
        NetManager.StartServer();
        NetManager.DisplayInfoPanel("Server running...", "Cancel", NetManager.StopServer);
    }

    public void OnClickRefreshPublicGames() {
        
        PublicGameList.ClearGames();
        PublicGameList.RequestPage(PublicGameList.CurrentPage);
    }
}
