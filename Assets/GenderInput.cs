using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenderInput : MonoBehaviour
{
    public TMP_InputField Input_Field;
    public string Gender_Input;
    public void Store_Input()
    {
        Gender_Input = Input_Field.GetComponent<TextMeshProUGUI>().text;
    }
}
