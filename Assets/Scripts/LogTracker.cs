using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class LogTracker : MonoBehaviour
{
    static public int User_Study_Mode = 0;
    private string filename_P;
    private string filename_Q;
    static public bool log_flag = false;
    static public int Quality_flag = 0;
    static public bool unfinished_flag = false;
    public string file_name_to_parse;
    public string string_to_parse;
    private bool IsPlay = false;
    public int Quality_Matrix_num;
    string f = "f";
    static public string Age;
    static public string Gender;
    static public string Exp;
    static public string Vision;
    public int matrix_index = 0;
    private int quality_index = 0;
    public string Quality_Matrix_csv_string;
    static public string[ , ] Quality_Matrix = new string[5 , 2];
    public int [] Choose_Right_Ref = new int [18];
    string timeStamp;
    // Start is called before the first frame update
    void Start()
    {
        // Quality_Matrix_num = (Quality_Matrix_num == 0)? 5 : Quality_Matrix_num;
        //DontDestroyOnLoad(this.gameObject);
    }

    /*void WriteQualityMatrix(string message)
    {
        StreamWriter LogFile = new StreamWriter(@"C:\vscode\" + filename + ".csv", true);
        LogFile.WriteLine (message);
        LogFile.Flush();
        LogFile.Close();
    }*/
    void WriteAnswer_P_AndTime(string message)
    {
        StreamWriter LogFile = new StreamWriter(@"C:\vscode\" + filename_P + ".csv", true);
        LogFile.WriteLine (message);
        LogFile.Flush();
        LogFile.Close();
    }

    void WriteAnswer_Q_AndTime(string message)
    {
        StreamWriter LogFile = new StreamWriter(@"C:\vscode\" + filename_Q + ".csv", true);
        LogFile.WriteLine (message);
        LogFile.Flush();
        LogFile.Close();
    }

    void Update()
    {

        	/*if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				//WriteQualityMatrix(f);
                string test1 = "Tile_generate__21__10__9";
                string test2 = "Tile_generate__17__20__12";
                string test = "Tile_generate__0__90__45";
                // print(test.Substring(18, test.Length - 18));
                
                
                string[] arrays = new string[3];
                print (arrays[0] + "1");
                int.TryParse(arrays[0]+"1", out int result);
                print(result + 2);
                if ( arrays[1] == "")
                {
                    print("yyyyyyyyy");
                }
                print(test2.Substring(test2.Length - 6, 6));
                //print(test1.Substring(test1.Length - 6, 6));
                //print(test.Substring(test.Length - 6, 6));
                string x = Get_Quality(test2.Substring(test2.Length - 6, 6));
                

			}*/
            if (unfinished_flag)
            {
                for (int i = matrix_index; i < 5; i++)
                {
                    Quality_Matrix_csv_string += "0,0,";
                }

                matrix_index = 0;
                Quality_Matrix_csv_string = Quality_Matrix_csv_string.Remove(Quality_Matrix_csv_string.Length - 1);
                Debug.Log(Quality_Matrix_csv_string);
                //WriteQualityMatrix(Quality_Matrix_csv_string);
                Quality_Matrix_csv_string = "";
                unfinished_flag = false;
            }
            if (log_flag)
            {
                if (Quality_flag == 1)
                {
                    WriteAnswer_Q_AndTime(string_to_parse);
                }
                else if (Quality_flag == 2)
                {
                    WriteAnswer_P_AndTime(string_to_parse);
                }
                // int size = file_name_to_parse.Length;
                // old
                /*string matrix_substring = file_name_to_parse.Substring(18, file_name_to_parse.Length - 18);
                Quality_Matrix [matrix_index, quality_index] = Get_Quality(matrix_substring);
                Quality_Matrix [matrix_index, quality_index + 1] = Get_Parallax(matrix_substring);

                for (int j = 0 ; j < 2; j++)
                {
                    Quality_Matrix_csv_string = Quality_Matrix_csv_string + Quality_Matrix [matrix_index, j] + ",";
                }
                if (matrix_index == Quality_Matrix_num - 1) 
                {
                    Quality_Matrix_csv_string = Quality_Matrix_csv_string.Remove(Quality_Matrix_csv_string.Length - 1);
                    print("Result:  " + Quality_Matrix_csv_string);
                    WriteQualityMatrix(Quality_Matrix_csv_string);
                    Quality_Matrix_csv_string = "";
                }   
                matrix_index += (matrix_index == 4)? -4 : 1;*/
                log_flag = false;
            }
    }
    public void IsPlaying()
    {
        float coin = UnityEngine.Random.Range(0.0f, 1.0f); // Conflict with System.Random
        User_Study_Mode = (coin > 0.5f)? 1 : 2;
        IsPlay = true;
        timeStamp = DateTime.Now.ToString("MMMddHHmm");
        filename_P = Gender + "__" + Age + "__" + Vision + "__" + Exp + "__" + "P" + "__" + timeStamp;
        filename_Q = Gender + "__" + Age + "__" + Vision + "__" + Exp + "__" + "Q" + "__" + timeStamp;
        Init_Quality_Matrix(User_Study_Mode);
    }
    private string Get_Parallax(string matrix_substring)
    {
        string[] temp_array = new string[4];     
        int temp = 0;
        //print(matrix_substring.Length);
        for (int i = 0; i < matrix_substring.Length ; i++)
        {
            string temp_read = matrix_substring.Substring(i, 1);
            bool valid_int = int.TryParse(temp_read, out int result);
            //print("now read" + temp_read);
            if (valid_int)
            {
                temp += 1;
                if (temp > 2)
                {
                    temp_array[temp-1] = temp_read;
                }
                continue;
            }
        }
        matrix_substring = temp_array[2] + temp_array[3];
        //print(matrix_substring);
        //print("xxxx");
        return matrix_substring;
    }
    private string Get_Quality(string matrix_substring)
    {
        string[] temp_array = new string[3];     
        int temp = 0;
        //print(matrix_substring.Length);
        for (int i = 0; i < matrix_substring.Length ; i++)
        {
            string temp_read = matrix_substring.Substring(i, 1);
            bool valid_int = int.TryParse(temp_read, out int result);
            //print("now read" + temp_read);
            if (valid_int)
            {
                temp_array[temp] = temp_read;
                temp += 1;
                if(temp == 2)
                {
                    break;
                }
            }
        }
        matrix_substring = temp_array[0] + temp_array[1];
        //print(matrix_substring);
        //print("xxxx");
        return matrix_substring;
    }
    private void Init_Quality_Matrix(int User_Study_Mode)
    {
        for (int i = 0; i < 18; i++)
        {
            Choose_Right_Ref[i] = -1;
        }

        StreamWriter LogFile;
        LogFile = new StreamWriter(@"C:\vscode\" + filename_P + ".csv", true);
        LogFile.WriteLine ("a00,t00,a01,t01,a02,t02"); // a04,t04,a05,t05,a06,t06,a07,t07,a08,t08,a09,t09,a10,t10,a11,t11,a12,t12,a13,t13,a14,t14
        LogFile.Flush();
        LogFile.Close();

        StreamWriter LogFile2;
        LogFile2 = new StreamWriter(@"C:\vscode\" + filename_Q + ".csv", true);
        LogFile2.WriteLine ("a00,t00,a01,t01,a02,t02"); // a04,t04,a05,t05,a06,t06,a07,t07,a08,t08,a09,t09,a10,t10,a11,t11,a12,t12,a13,t13,a14,t14
        LogFile2.Flush();
        LogFile2.Close();        
        /*if (User_Study_Mode == 1) // Play->Blank->Play
        {
            LogFile = new StreamWriter(@"C:\vscode\" + filename + ".csv", true);
            LogFile.WriteLine ("a00,t00,a01,t01,a02,t02"); // a04,t04,a05,t05,a06,t06,a07,t07,a08,t08,a09,t09,a10,t10,a11,t11,a12,t12,a13,t13,a14,t14
            LogFile.Flush();
            LogFile.Close();
        }
        else
        {

        }*/
        //StreamWriter LogFile = new StreamWriter(@"C:\vscode\" + filename + ".csv", true);
    }
}
