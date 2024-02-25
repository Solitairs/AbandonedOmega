using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO.Compression;
using NVorbis;
public class NoteController : MonoBehaviour
{
    public UnityEngine.Object determ, note1, long_note1, perfect, good,hold_audio,tap_audio;
    public static float speed=1;
    private BinaryReader fs;
    private byte[] data = new byte[4];
    public static GameObject[] lines,note_object;
    public static Tap_note notes;
    public static bool isAuto = false;
    private uint line_num,note_num,command_num;
    private void check()
    {
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(data);
        }
    }
    [Serializable]
    public class chart
    {
        public string difficulty;
    }
    [Serializable]
    public class Json_
    {
        public string music_file;
        public string illustration_file;
        public string title;
        public System.Collections.Generic.List<chart> charts;
    }
    public string ReadData(string fileUrl)
    {
        string readData;
        using (StreamReader sr = File.OpenText(fileUrl))
        {
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return readData;
    }
    public Image fufk;
    public static float every_score,now_score;
    public static int ahead_num=0,lost=0,mega=0,great=0,chaos=0;


    public byte[] StreamToBytes(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        // 设置当前流的位置为流的开始
        stream.Seek(0, SeekOrigin.Begin);
        return bytes;
    }
    static VorbisReader vorbis;
    public static AudioClip FromOggData(byte[] data)
    {
        // Load the data into a stream
        MemoryStream oggstream = new MemoryStream(data);
        vorbis = new NVorbis.VorbisReader(oggstream, false);
        int samplecount = (int)(vorbis.SampleRate * vorbis.TotalTime.TotalSeconds);
        AudioClip audioClip = AudioClip.Create("ogg clip", samplecount, vorbis.Channels, vorbis.SampleRate, false, OnAudioRead);
        return audioClip;
    }
    static void OnAudioRead(float[] data)
    {
        var f = new float[data.Length];
        vorbis.ReadSamples(f, 0, data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = f[i];
        }
    }
    ZipArchive zip;
    FileStream zipToOpen;
    private void Awake()
    {
        speed = PlayerPrefs.GetFloat("speed");
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        ahead_num = 0;
        now_score = 0;
        Json_ read_js = new Json_();
        if (PlayerPrefs.GetInt("isout") == 0)
            zipToOpen = new FileStream(Application.dataPath + "/data/lev/" + PlayerPrefs.GetString("level") + ".mtmlz", FileMode.Open);
        else
            zipToOpen = new FileStream(PlayerPrefs.GetString("ImportPath"), FileMode.Open);
        zip = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
        StreamReader chartMetadata = new StreamReader(zip.GetEntry("index.mtmlinfo").Open());
        read_js = JsonUtility.FromJson<Json_>(chartMetadata.ReadToEnd());
        chartMetadata.Close();

        Stream musicStream = zip.GetEntry(read_js.illustration_file).Open();
        musicStream.Seek(0, SeekOrigin.Begin);
        byte[] bytes = new byte[musicStream.Length];
        musicStream.Read(bytes, 0, (int)musicStream.Length);
        musicStream.Close();
        musicStream.Dispose();
        Texture2D t = new Texture2D(1, 1);
        t.LoadImage(bytes);
        GameObject.FindGameObjectWithTag("background").GetComponent<Image>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));

        musicStream = zip.GetEntry(read_js.music_file).Open();
        musicStream.Seek(0, SeekOrigin.Begin);
        bytes = new byte[musicStream.Length];
        musicStream.Read(bytes, 0, (int)musicStream.Length);
        GetComponent<AudioSource>().clip = FromOggData(bytes);
        musicStream.Close();
        musicStream.Dispose();
        musicStream = null;
        fs = new BinaryReader(zip.GetEntry(read_js.charts[0].difficulty + ".mtmlc").Open());
        PlayerPrefs.SetString("image_name", read_js.illustration_file);
        notes = new Tap_note();
        data = fs.ReadBytes(4);
        check();
        if (BitConverter.ToInt32(data, 0) != 1280136269) Debug.LogError("mtmlc格式错误!");
        data = fs.ReadBytes(4);
        check();
        //判定数量
        line_num = BitConverter.ToUInt32(data, 0);
        lines = new GameObject[line_num];
        notes.line_k = new float[line_num];
        notes.line_b = new float[line_num];
        notes.line_val = new float[line_num];
        //音符数量
        data = fs.ReadBytes(4);
        check();
        note_num = BitConverter.ToUInt32(data, 0);
        note_object = new GameObject[note_num];
        notes.Initialize(note_num);
        //指令数量
        data = fs.ReadBytes(4);
        check();
        command_num = BitConverter.ToUInt32(data, 0);
        for (int i = 0; i < line_num; i++)//读取line
        {
            data = fs.ReadBytes(4);
            check();
            lines[i] = (GameObject)Instantiate(determ, new Vector3(0, 5.1f - BitConverter.ToSingle(data, 0) * 0.0102f), Quaternion.LookRotation(Vector3.forward, Vector3.left));
            lines[i].GetComponent<linesid>().id = (uint)i;
            lines[i].GetComponent<linesid>().pos.b = BitConverter.ToSingle(data, 0);
            data = fs.ReadBytes(4);
            check();
            Color line_color = lines[i].GetComponent<SpriteRenderer>().color;
            lines[i].GetComponent<SpriteRenderer>().color = new Color(line_color.r, line_color.g, line_color.b, BitConverter.ToSingle(data, 0));
            lines[i].GetComponent<linesid>().apos.b = BitConverter.ToSingle(data, 0);
            data = fs.ReadBytes(4);
            check();
            notes.line_k[i] = BitConverter.ToSingle(data, 0);
            notes.line_b[i] = 0;
        }
        float seconds = 0;
        for (uint ii = 0; ii < note_num; ii++)//读取note
        {
            uint mode = 0, road = 0, deter_ID = 0;
            float begin_time = 0, end_time = 0, begin_road = 0, long_note = 0, devia = 0;
            for (int i = 1; i < 10; i++)
            {
                data = fs.ReadBytes(4);
                check();
                if (i == 1)
                {
                    begin_time = BitConverter.ToSingle(data, 0);
                }
                if (i == 2)
                {
                    end_time = BitConverter.ToSingle(data, 0);
                }
                if (i == 3)
                {
                    road = BitConverter.ToUInt32(data, 0);
                }
                if (i == 4)
                {
                    begin_road = BitConverter.ToSingle(data, 0);
                }
                if (i == 5)
                {
                    devia = BitConverter.ToSingle(data, 0);
                }
                if (i == 6)
                {
                    long_note = BitConverter.ToSingle(data, 0);
                }
                if (i == 7)
                {
                    deter_ID = BitConverter.ToUInt32(data, 0);
                }
                if (i == 8)
                {
                    //属性
                }
                if (i == 9)
                {
                    if (begin_time == end_time)
                    {
                        seconds += 1f;
                        game_combo++;
                    }
                    else seconds += (end_time - begin_time);
                    notes.push_back(ii, long_note, 0, begin_time, end_time, deter_ID, begin_road, road, mode, 0, 0, 0, 0, 0, devia);
                }
            }
        }
        every_score = 2000000f / seconds;
        game_time = 0;
        goahead = true;
        commands_id = 0;
        for (int i = 0; i < line_num; i++)
        {
            notes.line_b[i] = 0;
        }
    }

    private int game_combo=0;
    public static float game_time=0;
    public TextMeshProUGUI score,ah_num;
    public Image fill;
    public GameObject quit;
    private bool isread=false,goahead;
    private float wait_time = 0;
    uint commands_id=0;
    void note_active(uint id)
    {
        
        if (notes.time[id] == notes.end_time[id])
        {
            note_object[id] = (GameObject)Instantiate(note1, new Vector3(-1.90905f + 2.952950333333333f * notes.road[id], 100), Quaternion.identity,transform);
            note_object[id].GetComponent<Taps>().id = id;
        }
        else
        {
            note_object[id] = (GameObject)Instantiate(long_note1, new Vector3(-1.90905f + 2.952950333333333f * notes.road[id], 100), Quaternion.identity,transform);
            note_object[id].GetComponent<Holds>().id = id;
        }
    }
    public GameObject show,loadline;
    public void Play_again()
    {
        quit.transform.position = new Vector2(1962, 0);
        GetComponent<AudioSource>().pitch = 1;
        Time.timeScale = 1;
        goahead = true;

    }
    public void EXIT()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(6, 0);
        asyncOperation.allowSceneActivation = false;    
        show.transform.localPosition = new Vector3(0, 0);
        PlayerPrefs.SetInt("isout", 0);
        while (asyncOperation.progress < 0.9f)
        {
            loadline.GetComponent<Image>().fillAmount = asyncOperation.progress / 0.9f;
        }
        Time.timeScale = 1;
        asyncOperation.allowSceneActivation = true;
    }
    private int maxcombo = 0;
    private bool isrunning=true;
    private void Update()
    {

        if (Input.GetButtonDown("esc"))
        {
            GetComponent<AudioSource>().pitch = 0;
            Time.timeScale = 0;
            goahead = false;
        }
        if (!goahead) quit.transform.position = Vector2.Lerp(quit.transform.position, new Vector2(0, 0), 0.4f);
        game_time += Time.deltaTime;
        score.text = Mathf.RoundToInt(now_score).ToString();
        ah_num.text = ahead_num.ToString();
        ah_num.fontSize = Mathf.Lerp(ah_num.fontSize, 80, 0.1f);
        score.fontSize = Mathf.Lerp(score.fontSize, 50, 0.13f);
        maxcombo = Mathf.Max(maxcombo, ahead_num);
        fill.fillAmount = gameObject.GetComponent<AudioSource>().time / gameObject.GetComponent<AudioSource>().clip.length;
        if (fill.fillAmount >= 0.999999f && isrunning)
        {
            isrunning = false;
            PlayerPrefs.SetInt("lost", lost);
            PlayerPrefs.SetInt("great", great);
            PlayerPrefs.SetInt("chaos", chaos);
            PlayerPrefs.SetInt("score", Mathf.RoundToInt(now_score));
            PlayerPrefs.SetInt("mega", mega);
            PlayerPrefs.SetInt("combo", maxcombo);
            PlayerPrefs.SetInt("maxcombo", game_combo);
            PlayerPrefs.Save();
            StartCoroutine(load(5));
        }
        if (commands_id < command_num && isrunning)
        {
            if (!isread)
            {
                isread = true;
                data = fs.ReadBytes(4);
                check();
                wait_time = BitConverter.ToSingle(data, 0);
            }
            else
            {
                if (wait_time - game_time <= 0)
                {
                    commands_id++;
                    isread = false;
                    data = fs.ReadBytes(4);
                    check();
                    uint id;
                    if (BitConverter.ToUInt32(data, 0) == 0x01)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        note_active(BitConverter.ToUInt32(data, 0));
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x00)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        gameObject.GetComponent<AudioSource>().Play();
                        gameObject.GetComponent<AudioSource>().time = BitConverter.ToSingle(data, 0);
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x02)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        id = BitConverter.ToUInt32(data, 0);
                        notes.mode[id] = 1;
                        data = fs.ReadBytes(4);
                        check();
                        notes.k[id] = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        notes.b[id] = BitConverter.ToSingle(data, 0);
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x03)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        id = BitConverter.ToUInt32(data, 0);
                        notes.mode[id] = 2;
                        data = fs.ReadBytes(4);
                        check();
                        notes.a[id] = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        notes.w[id] = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        notes.o[id] = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        notes.b[id] = BitConverter.ToSingle(data, 0);
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x11)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        id = BitConverter.ToUInt32(data, 0);
                        lines[id].GetComponent<linesid>().apos.normal = true;
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().apos.k = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().apos.b = BitConverter.ToSingle(data, 0);
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x12)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        id = BitConverter.ToUInt32(data, 0);
                        lines[id].GetComponent<linesid>().apos.normal = false;
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().apos.a = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().apos.w = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().apos.q = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().apos.b = BitConverter.ToSingle(data, 0);
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x13)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        id = BitConverter.ToUInt32(data, 0);
                        lines[id].GetComponent<linesid>().pos.normal = true;
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().pos.k = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().pos.b = BitConverter.ToSingle(data, 0);
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x14)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        id = BitConverter.ToUInt32(data, 0);
                        lines[id].GetComponent<linesid>().pos.normal = false;
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().pos.a = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().pos.w = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().pos.q = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        lines[id].GetComponent<linesid>().pos.b = BitConverter.ToSingle(data, 0);
                    }
                    else if (BitConverter.ToUInt32(data, 0) == 0x15)
                    {
                        data = fs.ReadBytes(4);//参数数量，暂时无用
                        data = fs.ReadBytes(4);
                        check();
                        id = BitConverter.ToUInt32(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        notes.line_k[id] = BitConverter.ToSingle(data, 0);
                        data = fs.ReadBytes(4);
                        check();
                        notes.line_b[id] = BitConverter.ToSingle(data, 0);
                    }
                }
            }
        }
        for (int i = 0; i < line_num; i++)
        {
            notes.line_val[i] = notes.line_k[i] * game_time + notes.line_b[i];
        }
    }
    IEnumerator load(int scence)
    {
        zip.Dispose();
        zipToOpen.Dispose();
        while (fufk.color.a < 0.999f)
        {
            yield return new WaitForSeconds(0.01f);
            fufk.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(fufk.color.a,1,0.01f));
        }
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
}