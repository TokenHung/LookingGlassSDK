using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class LogTracker : MonoBehaviour
{
    private string filename;
    public static bool log_flag = false;
    public static bool unfinished_flag = false;
    public string file_name_to_parse;
    private bool IsPlay = false;
    public int Quality_Matrix_num;
    string f = "f";
    static public string Age;
    static public string Gender;
    public int matrix_index = 0;
    private int quality_index = 0;
    public string Quality_Matrix_csv_string;
    static public string[ , ] Quality_Matrix = new string[5 , 2];
    string timeStamp;
    // Start is called before the first frame update
    void Start()
    {
        Quality_Matrix_num = (Quality_Matrix_num == 0)? 5 : Quality_Matrix_num;
        //DontDestroyOnLoad(this.gameObject);
    }

    void WriteQualityMatrix(string message)
    {
        StreamWriter LogFile = new StreamWriter(@"C:\vscode\" + filename + ".csv", true);
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
                WriteQualityMatrix(Quality_Matrix_csv_string);
                Quality_Matrix_csv_string = "";
                unfinished_flag = false;
            }
            if (log_flag)
            {
                // int size = file_name_to_parse.Length;
                string matrix_substring = file_name_to_parse.Substring(18, file_name_to_parse.Length - 18);
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
                matrix_index += (matrix_index == 4)? -4 : 1;
                log_flag = false;
            }
    }
    public void IsPlaying()
    {
        IsPlay = true;
        timeStamp = DateTime.Now.ToString("MMMddHHmm");
        filename = Gender + Age + "__" + timeStamp;
        Init_Quality_Matrix();
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
    private void Init_Quality_Matrix()
    {
        for (int i = 0; i < 5; i++)
            for (int j = 0 ; j < 2; j++)
            {
                Quality_Matrix[i,j] = "-1";
            }
        StreamWriter LogFile = new StreamWriter(@"C:\vscode\" + filename + ".csv", true);
        LogFile.WriteLine ("Q00,Q01,Q10,Q11,Q20,Q21,Q30,Q31,Q40,Q41");
        LogFile.Flush();
        LogFile.Close();
    }
}
