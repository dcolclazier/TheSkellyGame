using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class LobbyColorDropdown : MonoBehaviour {

    [SerializeField]
    void Start() {
        var itemText = gameObject.GetComponentInChildren<Text>();

        if (MultiplayerManager.Instance.ColorsInUse.All(c => c.GetName() != itemText.text)) return;

        GetComponent<Toggle>().interactable = false;
        itemText.color = Color.grey;

        
    }
}
