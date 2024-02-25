using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO.Compression;

public class ChSpecController : MonoBehaviour
{
    public struct tree
    {
        public bool pass;
        public int fromed;
    }
    public TextMeshProUGUI title,tname,pan_model;
    public GameObject ChooseBox,Back,show,loadline;
    public string[] level_filename;
    private Sprite[] back_images;
    private int pages=0;
    private Json_[] read_js;
    private float[] DifPosX;
    private tree[] thetree;
    private int difficulty = 2;
    public Transform[] CDiff; 
    private void Update()
    {
        if (Input.GetButtonDown("esc"))
        {
            StartCoroutine(load(6));
        }
    }
    public void ESC()
    {
        StartCoroutine(load(6));
    }
    public void Select1()
    {
        difficulty = 0;
        CDiff[3].localPosition = new Vector3(DifPosX[0], CDiff[3].localPosition.y);
        CDiff[2].localPosition = new Vector3(DifPosX[1], CDiff[2].localPosition.y);
        CDiff[1].localPosition = new Vector3(DifPosX[2], CDiff[1].localPosition.y);
        CDiff[0].localPosition = new Vector3(DifPosX[3], CDiff[0].localPosition.y);
    }
    public void Select2()
    {
        difficulty = 1;
        CDiff[3].localPosition = new Vector3(DifPosX[1], CDiff[3].localPosition.y);
        CDiff[2].localPosition = new Vector3(DifPosX[0], CDiff[2].localPosition.y);
        CDiff[1].localPosition = new Vector3(DifPosX[2], CDiff[1].localPosition.y);
        CDiff[0].localPosition = new Vector3(DifPosX[3], CDiff[0].localPosition.y);
    }
    public void Select3()
    {
        difficulty = 2;
        CDiff[3].localPosition = new Vector3(DifPosX[2], CDiff[3].localPosition.y);
        CDiff[2].localPosition = new Vector3(DifPosX[1], CDiff[2].localPosition.y);
        CDiff[1].localPosition = new Vector3(DifPosX[0], CDiff[1].localPosition.y);
        CDiff[0].localPosition = new Vector3(DifPosX[3], CDiff[0].localPosition.y);
    }
    public void Select4()
    {
        difficulty = 3;
        CDiff[3].localPosition = new Vector3(DifPosX[3], CDiff[3].localPosition.y);
        CDiff[2].localPosition = new Vector3(DifPosX[1], CDiff[2].localPosition.y);
        CDiff[1].localPosition = new Vector3(DifPosX[2], CDiff[1].localPosition.y);
        CDiff[0].localPosition = new Vector3(DifPosX[0], CDiff[0].localPosition.y);
    }
    // Start is called before the first frame update
    void Awake()
    {
        DifPosX = new float[4];
        DifPosX[0] = -2779;
        DifPosX[1] = -2608;
        DifPosX[2] = -2430;
        DifPosX[3] = -2249;
        thetree = new tree[level_filename.Length];
        thetree[0].fromed = -1 ;
        read_js = new Json_[level_filename.Length];
        back_images=new Sprite[level_filename.Length];
        for (int i=0;i< level_filename.Length; i++)
        {
            
            using(FileStream zipToOpen = new FileStream(Path.Combine(Path.Combine(Path.Combine(Application.dataPath, "data"), "lev"), level_filename[i] + ".mtmlz"), FileMode.Open))
            {
                using (ZipArchive zip = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    StreamReader chartMetadata = new StreamReader(zip.GetEntry("index.mtmlinfo").Open());
                    read_js[i] = JsonUtility.FromJson<Json_>(chartMetadata.ReadToEnd());
                    chartMetadata.Close();


                    Stream musicStream =zip.GetEntry(read_js[i].illustration_file).Open();
                    musicStream.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = new byte[musicStream.Length];
                    musicStream.Read(bytes, 0, (int)musicStream.Length);
                    musicStream.Close();
                    musicStream.Dispose();
                    musicStream = null;
                    Texture2D t = new Texture2D(1, 1);
                    t.LoadImage(bytes);
                    back_images[i]= Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
                    ChooseBox.GetComponent<Image>().sprite = back_images[i];
                    Back.GetComponent<Image>().sprite = back_images[i];
                }
            }
        }
        pages = 0;
        ChooseBox.GetComponent<Image>().sprite = back_images[0];
        Back.GetComponent<Image>().sprite = back_images[0];
        title.text = read_js[pages].title;
        tname.text = read_js[pages].charts[0].writer;
    }
    string ReadData()
    {
        string readData;
        //读取文件
        using (StreamReader sr = File.OpenText("index.mtmlinfo"))
        {
            //数据保存
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return readData;
    }
    IEnumerator load(int scence)
    {
        
        show.transform.localPosition = new Vector3(0, 0);
        AsyncOperation op = SceneManager.LoadSceneAsync(scence);
        op.allowSceneActivation = false;
        PlayerPrefs.Save();
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
    public void START_b()
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.SetString("title", read_js[pages].title);
        PlayerPrefs.SetString("deterMode", pan_model.text);
        PlayerPrefs.SetString("writer", read_js[pages].charts[0].writer);
        PlayerPrefs.SetInt("isout", 0);
        PlayerPrefs.SetString("level", level_filename[pages]);
        PlayerPrefs.Save();
        StartCoroutine(load(1));
    }
    public void leftpress()
    {
        pages--;
        if (pages <0) pages = level_filename.Length - 1;
        if (read_js[pages].charts[0].difficulty == "Demo") pan_model.text = "普通判定";
        title.text = read_js[pages].title;
        tname.text = read_js[pages].charts[0].writer;
        Back.GetComponent<Image>().sprite = back_images[pages];
        ChooseBox.GetComponent<Image>().sprite = back_images[pages];
    }
    public void righttpress()
    {
        pages++;
        if (pages >= level_filename.Length) pages = 0;
        if (read_js[pages].charts[0].difficulty == "Demo") pan_model.text = "普通判定";
        title.text = read_js[pages].title;
        tname.text = read_js[pages].charts[0].writer;
        Back.GetComponent<Image>().sprite = back_images[pages];
        ChooseBox.GetComponent<Image>().sprite = back_images[pages];
    }

    [Serializable]
    public class chart
    {
        public string difficulty;
        public string writer;
    }
    [Serializable]
    public class Json_
    {
        public string music_file;
        public string illustration_file;
        public string title;
        public List<chart> charts;
    }
}
