using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AvatarDropDown : MonoBehaviour
{
    List<string> labels = new List<string>() {
    "Please Select",
    "Pre-Questionnaire",
    "Training Scenario",
    "Rover Repair",
    "Post-Questionnaire"
    };

    public TMP_Dropdown dropdown;
    public TextMeshProUGUI selectedLabel;

    public void Dropdown_IndexChanged(int index){
        //Read the index and display the selected label
        selectedLabel.text = labels[index] + " selected!";
        //Send selection to the Menu Controller
        MenuController.MC.OnClickCharacterPick(index);
        Debug.Log(labels[index]);

        // Open Pre-Questionairre
        if (labels[index] == labels[1])
        {
            Application.OpenURL("https://forms.gle/6YMJJJjBC1FkjCtp6");
        }
    }
    
    void Start(){
        PopulateList();
    }

    void PopulateList(){
        dropdown.AddOptions(labels);
    }
}
