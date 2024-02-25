using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using NVorbis;
using System.IO.Compression;

public class SettleController : MonoBehaviour
{
    public static int level=0;
    public Image back;
    public TextMeshProUGUI settleTitle, panMode, settleWriter,ACC;
    public GameObject settleShow,fuck;
    public GameObject isFull, isNewBest,show,loadline;
    public TextMeshProUGUI tlost, tmega, tgreat, tchaos, tscore, tcombo, newbest;
    ZipArchive zip;
    FileStream zipToOpen;
    private void FixedUpdate()
    {
        if (fuck != null)
        {
            fuck.GetComponent<Image>().color = Color.Lerp(fuck.GetComponent<Image>().color, new Color(0, 0, 0, 0), 0.015f);
            if (fuck.GetComponent<Image>().color.a < 0.001f) Destroy(fuck);
        }
    }
    // Update is called once per frame
    public void Awake()
    {
        zipToOpen = new FileStream(Application.dataPath + "/data/lev/" + PlayerPrefs.GetString("level") + ".mtmlz", FileMode.Open);
        zip = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
        settleTitle.text = PlayerPrefs.GetString("title");
        settleWriter.text = PlayerPrefs.GetString("writer");
        panMode.text = PlayerPrefs.GetString("deterMode");
        ACC.text= (Mathf.Round(PlayerPrefs.GetInt("score") / 200)/100).ToString()+"%"; 
        
        Stream musicStream = zip.GetEntry(PlayerPrefs.GetString("image_name")).Open();
        musicStream.Seek(0, SeekOrigin.Begin);
        byte[] bytes = new byte[musicStream.Length];
        musicStream.Read(bytes, 0, (int)musicStream.Length);
        musicStream.Close();
        musicStream.Dispose();
        Texture2D t = new Texture2D(1, 1);
        t.LoadImage(bytes);
        Sprite sprite= Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f)); ;

        settleShow.GetComponent<Image>().sprite = sprite;
        back.sprite = sprite;

        if (PlayerPrefs.GetInt("isout") == 0)
        {
            if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt(PlayerPrefs.GetString("title") + "-score-Pw:se&216hasd8@17%"))
            {
                isNewBest.SetActive(true);
                newbest.text = (PlayerPrefs.GetInt("score")-PlayerPrefs.GetInt(PlayerPrefs.GetString("title") + "-score-Pw:se&216hasd8@17%")).ToString();
            }
            if(PlayerPrefs.GetInt("combo") == PlayerPrefs.GetInt("maxcombo"))
            {
                isFull.SetActive(true);
            }
        }
        tlost.text = PlayerPrefs.GetInt("lost").ToString();
        tmega.text = PlayerPrefs.GetInt("mega").ToString();
        tgreat.text = PlayerPrefs.GetInt("great").ToString();
        tscore.text = PlayerPrefs.GetInt("score").ToString();
        tchaos.text= PlayerPrefs.GetInt("chaos").ToString();
        tcombo.text = PlayerPrefs.GetInt("combo").ToString();
        level = 1;
        PlayerPrefs.Save();
        zip.Dispose();
        zipToOpen.Dispose();
    }
    IEnumerator load(int scence)
    {
        show.transform.localPosition = new Vector3(0, 0);
        AsyncOperation op = SceneManager.LoadSceneAsync(scence);
        op.allowSceneActivation = false;
        PlayerPrefs.Save();
        while (!op.isDone)
        {
            loadline.GetComponent<Image>().fillAmount = op.progress;
            if (op.progress >= 0.9f)
            {
                loadline.GetComponent<Image>().fillAmount = 1;
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void restart()
    {
        StartCoroutine(load(1));
    }
    public void goout()
    {
        StartCoroutine(load(6));
    }
}
