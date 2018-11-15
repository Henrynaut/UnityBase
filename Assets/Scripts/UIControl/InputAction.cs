using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputAction : ScriptableObject 
{
    public string keyWord;
// Need to update this with more fields
    public abstract void RespondToInput (UIController controller, string[] separatedInputWords);
}