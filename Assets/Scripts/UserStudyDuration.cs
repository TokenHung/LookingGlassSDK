using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStudyDuration : MonoBehaviour
{
    private bool Start_count_flag = false;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Start to count the user study after clicking "Play" button.
        if (Start_count_flag)
        {
            timer += Time.deltaTime;
        }
    }    
    public void Start_timer_Count() // call by play button click
    {
        Start_count_flag = true;
    }

    public void End_timer_Count() // end timer
    {
        Start_count_flag = false;
    }
}
