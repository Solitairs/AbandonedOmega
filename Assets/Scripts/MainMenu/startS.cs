using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startS : MonoBehaviour
{
    // Start is called before the first frame update
        public Image img;
        Color color;
        public bool ischange = false;
    public void ChangeColor(){
        ischange = true;
    }
    private void Start()
    {
        //initialize
        if (PlayerPrefs.GetString("ini") != "yes")
        {
            Debug.Log("Set initialize");
            PlayerPrefs.SetString("ini", "yes");
            PlayerPrefs.SetString("1", "yes");
            PlayerPrefs.SetString("2", "no");
            PlayerPrefs.SetString("3", "no");
            PlayerPrefs.SetString("4", "no");
            PlayerPrefs.SetString("5", "no");
            PlayerPrefs.SetString("6", "no");
            PlayerPrefs.SetString("7", "no");
            PlayerPrefs.SetString("8", "no");
            PlayerPrefs.SetString("9", "no");
            PlayerPrefs.SetString("10", "no");
        }
    }
    public void Update()
    {
        if (ischange)
        {
            if (img.color.a  < 1) img.color = new Color(255, 255, 255, img.color.a + Time.deltaTime * 0.5f);
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
