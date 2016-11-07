using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkLobbyPlayer {


    private static readonly Color[] Colors = {Color.white, Color.magenta, Color.black, Color.cyan, Color.blue, Color.green, Color.yellow};
    private static readonly List<Color> ColorsInUse = new List<Color>();
        
    [SyncVar(hook = "OnMyName")] public string PlayerName = "";

    [SyncVar(hook = "OnMyColor")] public Color PlayerColor = Color.white;

    [SyncVar(hook = "OnMyReady")] public bool PlayerReady = false;

    public InputField NameInput;
    public Toggle ReadyCheck;
    public Dropdown ColorChoicesDropdown;
    public Image DropDownImage;
    

    public override void OnClientEnterLobby() {
        base.OnClientEnterLobby();

        if(MultiplayerManager.Instance!=null)
            MultiplayerManager.Instance.OnPlayerCountChange(1); // gross

        LobbyPlayerList.Instance.AddPlayer(this);

        SetupOtherPlayer();

        OnMyName(PlayerName);
        OnMyColor(PlayerColor);
        OnMyReady(PlayerReady);
    }

    private void SetupOtherPlayer() {
        if (isLocalPlayer) return;

        NameInput.interactable = false;
        ReadyCheck.interactable = false;
        ColorChoicesDropdown.interactable = false;

    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        SetupLocalPlayer();
    }
    private void SetupLocalPlayer() {
        NameInput.interactable = true;
        ReadyCheck.interactable = true;
        ColorChoicesDropdown.interactable = true;

        //must choose color?
        ReadyCheck.isOn = false;

        UpdateAvailableColors();
        if (PlayerColor == Color.white)
            CmdColorChanged(PlayerColor);

        if (PlayerName == "")
            CmdNameChanged("Player " + (LobbyPlayerList.Instance.PlayerListContentTransform.childCount-1));

        NameInput.onEndEdit.RemoveAllListeners();
        NameInput.onEndEdit.AddListener(OnNameChanged);

        ColorChoicesDropdown.onValueChanged.RemoveAllListeners();
        ColorChoicesDropdown.onValueChanged.AddListener(OnColorChanged);

        ReadyCheck.onValueChanged.RemoveAllListeners();
        ReadyCheck.onValueChanged.AddListener(OnReadyClicked);
    }

    private void OnReadyClicked(bool ready) {
        CmdReadyChanged(ready);
    }

    private void OnColorChanged(int colorIndex) {
        CmdColorChanged(Colors[colorIndex]);
    }

    private void OnNameChanged(string newName) {
        CmdNameChanged(newName);
    }

    public void OnMyName(string newName) {

        PlayerName = newName;
        NameInput.text = PlayerName;
    }

    public void OnMyReady(bool ready) {
        ReadyCheck.isOn = ready;
    }

    public void OnMyColor(Color newColor) {

        PlayerColor = newColor;
        DropDownImage.color = PlayerColor;
    }

    public void UpdateAvailableColors() {
        var availableColors = Colors.Where(color => ColorsInUse.All(c2 => c2 != color));
        ColorChoicesDropdown.options.Clear();
        foreach (var color in availableColors) {
            ColorChoicesDropdown.options.Add(new Dropdown.OptionData() {
                text = color.ToString()
            });
        }
    }

    public void OnPlayerListChanged(int playerIndex) {


    }

    [Command]
    public void CmdNameChanged(string playerName) {
        PlayerName = playerName;
    }

    [Command]
    public void CmdColorChanged(Color color) {

        var availableColors = Colors.Where(c => ColorsInUse.All(c2 => c2 != c)).ToList();

        ColorsInUse.Remove(PlayerColor);
        PlayerColor = availableColors.Contains(color) ? color : availableColors.First();
        ColorsInUse.Add(PlayerColor);

        UpdateAvailableColors();
    }

    [Command]
    public void CmdReadyChanged(bool ready) {
        ReadyCheck.isOn = ready;
    }
}
