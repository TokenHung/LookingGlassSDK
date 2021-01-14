using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Warning : MonoBehaviour
{
    public Button Play_Button;
    public TMP_InputField Input_Field_Exp;
    public TMP_InputField Input_Field_Vision;
    public TMP_InputField Input_Field_Age;
    public TMP_InputField Input_Field_Gender;
    public TextMeshProUGUI WarningMessage;
    public GameObject My_Log;
    private LogTracker LogTracker_Script;
    private string[] Log = {"",""};
    private string Age;
    private string Gender;
    private string Exp;
    private string Vision;
    private bool Valid_Input_Exp = false;
    private bool Valid_Input_Gender = false;
    private string[] Valid_Gender_String = {"M", "m", "F", "f"};
    private string[] Valid_Exp_String = {"A", "B", "C", "D"};
    void Start()
    { 
        Age = Input_Field_Age.text;
        Gender = Input_Field_Gender.text;
        Exp = Input_Field_Exp.text;
        Vision = Input_Field_Vision.text;
        LogTracker_Script = My_Log.GetComponent<LogTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Exp != "" && Gender != "" && Age != ""  && Vision != "")
        {   
            bool valid_Vision = int.TryParse(Vision, out int result_Vision);
            bool valid_Age = int.TryParse(Age, out int result_Age);
            if (valid_Age && valid_Vision)
            {
                for (int i = 0 ; i < 4; i++)
                {
                    if (Exp == Valid_Exp_String[i])
                    {
                        Valid_Input_Exp = true;
                    }
                    if (Gender == Valid_Gender_String[i])
                    {
                        Valid_Input_Gender = true;
                        WarningMessage.text = "Gender: " + Gender.ToUpper() + "\n" + "Age: " + Age + "\n" + "Vision: " + Vision + "\n" + "Exp: " + Exp;
                        if (Valid_Input_Exp)
                        {
                            break;
                        }
                    }
                }
            }
        }
        Age = Input_Field_Age.text;
        Gender = Input_Field_Gender.text;
        Exp = Input_Field_Exp.text;
        Vision = Input_Field_Vision.text;

        if (Valid_Input_Exp && Valid_Input_Gender)
        {
            Play_Button.gameObject.SetActive(true);
            LogTracker.Gender = Gender.ToUpper();
            LogTracker.Age = Age;
            LogTracker.Exp = Exp;
            LogTracker.Vision = Vision;
        }

        else
        {
            WarningMessage.text = "Gender: " + Gender.ToUpper() + "\n" + "Age: " + Age;
            Play_Button.gameObject.SetActive(false);
        }
    }


}
