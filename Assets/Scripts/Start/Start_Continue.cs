using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Start_Continue : MonoBehaviour
{
    public Image fore;
    public TextMeshProUGUI enter;
    private bool breath = true,wait=false,quit=false,isover=false;
    private float waittime = 0,target=0;
    void FixedUpdate()
    {
        fore.color = new Color(0, 0, 0, Mathf.MoveTowards(fore.color.a,target,0.02f));
        if (quit && fore.color.a == 1 && !isover)
        {
            isover = true;
            SceneManager.LoadSceneAsync(6, 0);
        }
        if (breath)
        {
            enter.color = new Color(1, 1, 1, enter.color.a - 0.015f);
            if (enter.color.a <= 0.036f)
            {
                breath = false;
            }
        }
        else
        {
            enter.color = new Color(1, 1, 1, enter.color.a+0.015f);
            if (enter.color.a >= 0.999f)
            {
                wait = true;
            }
        }
        if (wait)
        {
            waittime += 0.02f;
            if (waittime >= 0.1f)
            {
                waittime = 0;
                breath = true;
                wait = false;
            }
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("enter"))
        {
            quit = true;
            target = 1;
        }
    }
}
