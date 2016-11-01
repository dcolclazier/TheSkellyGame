using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class LobbyPlayerList : MonoBehaviour
{
    public static LobbyPlayerList Instance = null;
    public RectTransform playerListContent;

    protected VerticalLayoutGroup Layout;
    protected List<LobbyPlayer> Players = new List<LobbyPlayer>();

    public void OnEnable()
    {
        Instance = this;
        Layout = playerListContent.GetComponent<VerticalLayoutGroup>();
    }

    public void AddPlayer(LobbyPlayer playerToAdd)
    {
        if (Players.Contains(playerToAdd)) return;

        Players.Add(playerToAdd);

        playerToAdd.transform.SetParent(playerListContent, false);

    }
}

