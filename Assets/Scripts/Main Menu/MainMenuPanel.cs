using UnityEngine;

public class MainMenuPanel : MonoBehaviour {

    public MultiplayerManager NetManager;
	public void OnClickSinglePlayer()
    {

    }

    public void OnClickMultiPlayer()
    {
        NetManager.SwitchPanel(NetManager.MultiplayerPanel);
    }
    public void OnClickSettings()
    {
        NetManager.SwitchPanel(NetManager.SettingsPanel);
    }
    public void OnClickExit()
    {

    }

}
