using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgeInput : MonoBehaviour
{
    public string Age_Input;
    public GameObject Input_Field;
    public void Store_Input()
    {
        Age_Input = Input_Field.GetComponent<TextMeshProUGUI>().text;
    }
}
