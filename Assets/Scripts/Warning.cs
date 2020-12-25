using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Warning : MonoBehaviour
{
    public Button Play_Button;
    public TMP_InputField Input_Field_Age;
    public TMP_InputField Input_Field_Gender;
    public TextMeshProUGUI WarningMessage;
    public GameObject My_Log;
    private LogTracker LogTracker_Script;
    private string[] Log = {"",""};
    private string Age;
    private string Gender;
    private bool Valid_Input = false;
    private string[] Valid_Gender_String = {"M", "m", "F", "f"};
    void Start()
    { 
        Age = Input_Field_Age.text;
        Gender = Input_Field_Gender.text;
        LogTracker_Script = My_Log.GetComponent<LogTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gender != "" && Age != "")
        {   
            bool valid_Age = int.TryParse(Age, out int result);
            if (valid_Age)
            {
                for (int i = 0 ; i < 4; i++)
                {
                    if (Gender == Valid_Gender_String[i])
                    {
                        Valid_Input = true;
                        WarningMessage.text = "Gender: " + Gender.ToUpper() + "\n" + "Age: " + Age;
                        break;
                    }
                }
            }
            else
            {
                Valid_Input = false;
            }
        }
        Age = Input_Field_Age.text;
        Gender = Input_Field_Gender.text;

        if (Valid_Input)
        {
            Play_Button.gameObject.SetActive(true);
            LogTracker.Gender = Gender.ToUpper();
            LogTracker.Age = Age;
        }

        else
        {
            WarningMessage.text = "Gender: " + Gender.ToUpper() + "\n" + "Age: " + Age;
            Play_Button.gameObject.SetActive(false);
        }
    }


}
