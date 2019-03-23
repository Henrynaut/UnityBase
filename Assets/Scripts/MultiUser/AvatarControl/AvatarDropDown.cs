using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AvatarDropDown : MonoBehaviour
{
    List<string> labels = new List<string>() {
    "Please Select",
    "Green Suit",
    "Red Suit",
    "Blue Suit",
    "White Suit"
    };

    public TMP_Dropdown dropdown;
    public TextMeshProUGUI selectedLabel;

    public void Dropdown_IndexChanged(int index){
        //Read the index and display the selected label
        selectedLabel.text = labels[index] + " selected!";
        //Send selection to the Menu Controller
        MenuController.MC.OnClickCharacterPick(index);
    }
    
    void Start(){
        PopulateList();
    }

    void PopulateList(){
        dropdown.AddOptions(labels);
    }
}
