using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taps : MonoBehaviour
{
    // Start is called before the first frame update
    public uint id=0;
    public float away_time=10;
    // Update is called once per frame
    void Update()
    {
        away_time = Mathf.Abs(NoteController.notes.time[id] - NoteController.game_time);
        uint line_id = NoteController.notes.DeId[id];
        if (NoteController.notes.mode[id] == 0) transform.position = new Vector3(transform.position.x, NoteController.speed*(-NoteController.notes.line_val[line_id] + NoteController.notes.deviation[id]) * 0.0102f + NoteController.lines[line_id].transform.position.y);
        else if (NoteController.notes.mode[id] == 1) transform.position = new Vector3(-1.90905f + 2.952950333333333f * (NoteController.notes.k[id]* NoteController.game_time+ NoteController.notes.b[id]), NoteController.speed * (-NoteController.notes.line_val[line_id] + NoteController.notes.deviation[id]) * 0.0102f + NoteController.lines[line_id].transform.position.y);
        else if (NoteController.notes.mode[id]==2) transform.position = new Vector3(-1.90905f + 2.952950333333333f * (NoteController.notes.a[id] * Mathf.Sin(NoteController.notes.w[id] * NoteController.game_time + NoteController.notes.o[id]) + NoteController.notes.b[id]), NoteController.speed * (-NoteController.notes.line_val[line_id] + NoteController.notes.deviation[id]) * 0.0102f + NoteController.lines[line_id].transform.position.y);
        if (transform.position.y < -6)
        {
            NoteController.lost++;
            Destroy(gameObject);
        }
        if (Input.GetButtonDown(NoteController.notes.pan_road[id].ToString())&& away_time < 0.110f)
        {
            Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().tap_audio);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().ah_num.fontSize = 100;
            GameObject[] temp = GameObject.FindGameObjectsWithTag("tap");
            NoteController.ahead_num++;
            foreach(GameObject gameobject in temp)
            {
                if (NoteController.notes.pan_road[id] == NoteController.notes.pan_road[gameobject.GetComponent<Taps>().id]) if (gameobject.GetComponent<Taps>().away_time < away_time) return;
            }
            if (away_time <= 0.050f)
            {
                NoteController.mega++;
                Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().perfect, new Vector3(transform.position.x, NoteController.lines[line_id].transform.position.y), Quaternion.identity);
                NoteController.now_score += NoteController.every_score;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().score.fontSize = 100;
            }
            else if(away_time <= 0.1f)
            {
                NoteController.great++;
                Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().good, new Vector3(transform.position.x, NoteController.lines[line_id].transform.position.y), Quaternion.identity);
                NoteController.now_score += NoteController.every_score * 0.65f;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().score.fontSize = 100;
            }
            else
            {
                NoteController.chaos++;
                NoteController.ahead_num = 0; ;
            }
            Destroy(gameObject);
        }
        else if(NoteController.isAuto&& NoteController.notes.time[id] - NoteController.game_time <= 0)
        {
            Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().tap_audio);
            NoteController.mega++;
            NoteController.ahead_num++;
            Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().perfect, new Vector3(transform.position.x, NoteController.lines[line_id].transform.position.y), Quaternion.identity);
            NoteController.now_score += NoteController.every_score;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().ah_num.fontSize = 95;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteController>().score.fontSize = 72;
            Destroy(gameObject);
        }
    }
}
