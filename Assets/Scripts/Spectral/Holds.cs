using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Holds : MonoBehaviour
{
    public GameObject top, high;
    public uint id;
    public uint DeId;
    void Start()
    {
        DeId = NoteController.notes.DeId[id];
        NoteController.notes.long_notes[id] *= NoteController.speed;
        top.transform.position = new Vector3(transform.position.x - 0.0209999f, transform.position.y + 0.23299993f + 0.0102f * NoteController.notes.long_notes[id]);
        high.transform.position = new Vector3(transform.position.x - 0.043f, transform.position.y + 0.1108273f + 0.0051f * NoteController.notes.long_notes[id]);
        high.GetComponent<SpriteRenderer>().size = new Vector2(10.5f, 1+ 0.042411f * NoteController.notes.long_notes[id]);

    }

    // Update is called once per frame
    private float away,up=0.15f,up_combo=0.3f;
    private float level;
    private bool have_audio=false;
    private bool arrive=false,Get=false;
    private Object effect;
    private GameObject audio;
    void Update()
    {
        away = NoteController.notes.time[id] - NoteController.game_time;
        if (away <= 0)
        {
            arrive = true;
        }
        if (NoteController.notes.mode[id] == 0)
        {
            transform.position = new Vector3(transform.position.x, NoteController.speed*(-NoteController.notes.line_val[DeId] + NoteController.notes.deviation[id]) * 0.0102f + NoteController.lines[DeId].transform.position.y);
        }
        else if (NoteController.notes.mode[id] == 1)
        {
            transform.position = new Vector3(-1.90905f + 2.952950333333333f * (NoteController.notes.k[id] * NoteController.game_time + NoteController.notes.b[id]), NoteController.speed*(-NoteController.notes.line_val[DeId] + NoteController.notes.deviation[id]) * 0.0102f + NoteController.lines[DeId].transform.position.y);
        }
        else if (NoteController.notes.mode[id] == 2) transform.position = new Vector3(-1.90905f + 2.952950333333333f * (NoteController.notes.a[id] * Mathf.Sin(NoteController.notes.w[id] * NoteController.game_time + NoteController.notes.o[id]) + NoteController.notes.b[id]), NoteController.speed*(NoteController.notes.line_val[DeId] + NoteController.notes.deviation[id]) * 0.0102f + NoteController.lines[DeId].transform.position.y);
        if (Input.GetButtonDown(NoteController.notes.pan_road[id].ToString())&& Mathf.Abs(away) <= 0.110f)
        {
            audio=Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().hold_audio) as GameObject;
            have_audio = true;
            Get = true;
            if (Mathf.Abs(away) <= 0.050f)
            {
                effect=GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().perfect;
                level = 1;
            }
            else if(Mathf.Abs(away) <= 0.100f)
            {
                effect = GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().good;
                level = 0.65f;
            }
            else
            {
                effect = null;
                level = 0;
            }
        }
        else if (NoteController.isAuto && away <= 0&&!Get)
        {
            audio = Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().hold_audio) as GameObject;
            have_audio = true;
            Get = true;
            effect = GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().perfect;
            level = 1;
        }
        if(arrive)
        {
           
            NoteController.notes.long_notes[id] -= NoteController.speed * (NoteController.notes.line_k[DeId] * Time.deltaTime);
            if (NoteController.notes.long_notes[id] <= 0f)
            {
                if (have_audio) Destroy(audio);
                Destroy(gameObject);
            }
            top.transform.position = new Vector3(transform.position.x - 0.0209999f, transform.position.y + 0.23299993f +  0.0102f * NoteController.notes.long_notes[id]);
            high.transform.position = new Vector3(transform.position.x - 0.043f, transform.position.y + 0.1108273f +  0.0051f * NoteController.notes.long_notes[id]);
            high.GetComponent<SpriteRenderer>().size = new Vector2(10.5f, 1 +  0.042411f * NoteController.notes.long_notes[id]);
            transform.position = new Vector3(transform.position.x, NoteController.lines[DeId].transform.position.y);
            if (Get)
            {
                if (!Input.GetButton(NoteController.notes.pan_road[id].ToString())&&!NoteController.isAuto)
                {
                    Get = false;
                    Destroy(audio);
                    have_audio = false;
                }
                up += Time.deltaTime;
                up_combo += Time.deltaTime;
                NoteController.now_score += NoteController.every_score * Time.deltaTime * level;
                if (up >= 0.15f)
                {
                    up = 0;
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().score.fontSize = 72;
                    if(effect!=null) Instantiate(effect, new Vector3(transform.position.x, NoteController.lines[DeId].transform.position.y), Quaternion.identity);
                }
                if (up_combo >= 0.2f)
                {
                    up_combo = 0;
                    NoteController.ahead_num++;
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().ah_num.fontSize = 95;
                }
            }
        }
    }
}
