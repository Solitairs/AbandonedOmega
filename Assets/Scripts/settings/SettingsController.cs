using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class SettingsController : MonoBehaviour
{
    public GameObject the_line,main_part;
    public GameObject[] text;
    private float[] text_to_font;
    private uint page = 0;
    // Start is called before the first frame update
    void Awake()
    {
        text_to_font = new float[5];
        for(int i = 0; i < 5; i++)
        {
            if (i == page)
            {
                text_to_font[i] = 80;
            }
            else
            {
                text_to_font[i] = 50;
            }
        }
    }
    public void ChangePage0()
    {
        page = 0;
    }
    public void ChangePage1()
    {
        page = 1;
    }
    public void ChangePage2()
    {
        page = 2;
    }
    public void ChangePage3()
    {
        page = 3;
    }
    public void ChangePage4()
    {
        page = 4;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("esc"))
        {
            StartCoroutine(load(6));
        }
        for (int i = 0; i < 5; i++)
        {
            if (i == page)
            {
                text_to_font[i] = 80;
            }
            else
            {
                text_to_font[i] = 50;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            text[i].GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(text[i].GetComponent<TextMeshProUGUI>().fontSize, text_to_font[i],0.1f);
        }
        the_line.transform.position = new Vector3(Mathf.Lerp(the_line.transform.position.x,text[page].transform.position.x,0.1f),the_line.transform.position.y);
        main_part.transform.localPosition = new Vector3(Mathf.Lerp(main_part.transform.localPosition.x, 0- (int)page *1920, 0.1f), main_part.transform.localPosition.y);
    }
    public GameObject loadline, show;
    IEnumerator load(int scence)
    {
        show.transform.localPosition = new Vector3(0, 0);
        AsyncOperation op = SceneManager.LoadSceneAsync(scence);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {//如果操作没有结束
            loadline.GetComponent<Image>().fillAmount = op.progress;
            if (op.progress >= 0.9f)
            {
                loadline.GetComponent<Image>().fillAmount = 1;
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
