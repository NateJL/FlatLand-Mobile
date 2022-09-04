using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButtonController : MonoBehaviour
{
    public enum ButtonType
    {
        None = 0,
        DIALOGUE_ENTRY
    }
    public ButtonType buttonType = ButtonType.None;
    public int value;

    /// <summary>
    /// Set the value of the button.
    /// </summary>
    public void SetButtonValue(int value)
    {
        this.value = value;
    }
    
}
