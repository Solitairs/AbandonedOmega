using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEngine.UI;



public class MainMController : MonoBehaviour
{
    public GameObject loadline, show, foreG,background;
    public Sprite[] images;
    IEnumerator start()
    {
        Image fore=foreG.GetComponent<Image>();
        while (fore.color.a > 0.001f)
        {
            fore.color = new Color(0, 0, 0, fore.color.a - 0.05f);
            yield return new WaitForSeconds(0.018f);
        }
        Destroy(foreG);
        yield return null;
    }
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetButtonDown("esc"))
        {
            Application.Quit();
        }
    }
    void Awake()
    {
        float getRandom = Random.Range(0, images.Length);
        for (int i = 0; i < images.Length; i++)
        {
            if (getRandom <= i + 1)
            {
                background.GetComponent<Image>().sprite = images[i];
                break;
            }
        }
        if (PlayerPrefs.GetString("isinit") != "right")
        {
            //initialize code
            PlayerPrefs.SetFloat("volume", 1);
            PlayerPrefs.SetFloat("speed", 2);
            PlayerPrefs.SetString("isinit", "right");
            PlayerPrefs.Save();
        }
        StartCoroutine(start());
    }

    public void import()
    {
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = "mtmlz\0*.mtmlz\0\0";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;
        //default path
        ofn.initialDir = "C:";  
        ofn.title = "Open Project";

        ofn.defExt = ".mtmlz";//files type
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

        if (WindowDll.GetOpenFileName(ofn))
        {
            PlayerPrefs.SetString("ImportPath", ofn.file);
            PlayerPrefs.SetInt("isout", 1);
            StartCoroutine(load(1));
        }
    }
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
    public void quit()
    {
        Application.Quit();
    }
    public void settings()
    {
        StartCoroutine(load(4));
    }
    public void Spec()
    {
        StartCoroutine(load(2));
    }
}
