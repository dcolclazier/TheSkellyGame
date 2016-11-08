using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class LobbyColorDropdown : MonoBehaviour {

    [SerializeField]
	// Use this for initialization
	void Start () {
        var itemText = gameObject.GetComponentInChildren<Text>();

        if (LobbyPlayer.ColorsInUse.Any(c => c.GetName() == itemText.text)) {
            GetComponent<Toggle>().interactable = false;
            itemText.color = Color.green;
        }
        //bool inUse = LobbyPlayer.ColorsInUse.All(c => c.GetName() != itemText.text);
        //GetComponent<Toggle>().interactable = inUse;
        //if (inUse) itemText.color = Color.gray;

    }
    public string StrikeThrough(string s) {
        return s.Aggregate("", (current, c) => current + c + '\u0336');
    }
}
