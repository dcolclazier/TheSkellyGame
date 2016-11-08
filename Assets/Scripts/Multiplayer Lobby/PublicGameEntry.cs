using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class PublicGameEntry : MonoBehaviour {

    public Text AvailableSlotsText;
    public Text GameNameText;
    public Button JoinButton;

    private MatchInfoSnapshot _gameInfo; 

    private readonly MultiplayerManager _netManager = MultiplayerManager.Instance;
    private readonly PanelManager _panelManager = PanelManager.Instance;
  
    public void OnJoinClicked() {
        if (_gameInfo.currentSize < _gameInfo.maxSize) 
            JoinMatch(_gameInfo.networkId);
        else
            UpdateJoinButton(false);
        
    }

    private void JoinMatch(NetworkID networkId) {
        _netManager.matchMaker.JoinMatch(networkId, "", "", "", 0, 0, _netManager.OnMatchJoined);
        _panelManager.DisplayInfoPanel("Connecting...", "Cancel", _netManager.CancelClientConnection);
    }

    public void Populate(MatchInfoSnapshot gameInfo) {
        _gameInfo = gameInfo;
        GameNameText.text = gameInfo.name;
        AvailableSlotsText.text = "(" + gameInfo.currentSize +
            "\\" + gameInfo.maxSize + ")";
        UpdateJoinButton(_gameInfo.maxSize > _gameInfo.currentSize);
    }

    private void UpdateJoinButton(bool available) {
        JoinButton.interactable = available;
        JoinButton.GetComponentInChildren<Text>().color = available ? Color.black : Color.grey;
    }
}
