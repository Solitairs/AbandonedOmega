using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap_note : MonoBehaviour
{
    public float[] line_k, line_b, line_val;
    public float[] deviation;
    public float[] y, time, road, long_notes, end_time;
    public uint[] mode,DeId,pan_road;
    public float[] road_y, k, b, a, w, o;
    public void Initialize(uint n)
    {
        road_y = new float[4];
        road_y[0] = -2.016875f - 1.391482f;
        road_y[1] = 0.244375f - 1.391482f;
        road_y[2] = 2.505625f - 1.391482f ;
        road_y[3] = 4.766875f - 1.391482f;
        deviation = new float[n];
        long_notes = new float[n];
        end_time = new float[n];
        y = new float[n];
        k = new float[n];
        b = new float[n];
        a = new float[n];
        w = new float[n];
        o = new float[n];
        road = new float[n];
        DeId = new uint[n];
        pan_road=new uint[n];
        mode = new uint[n];
        time = new float[n];
    }
    public void change_mode1(float ck, float cb, uint id)
    {
        k[id] = ck;
        b[id] = cb;
        mode[id] = 1;
    }
    public void change_mode2(float ca,float cw,float co, float cb, uint id)
    {
        a[id] = ca;
        w[id] = cw;
        o[id] = co;
        b[id] = cb;
        mode[id - 1] = 2;
    }
    public void push_back(uint id, float note_long,float cy, float ct, float cet, uint deterid, float cr, uint cpr, uint cm, float Ck, float Cb, float Ca, float Cw, float Co,float cdev)
    {
        y[id] = cy;
        time[id]=ct;
        long_notes[id] = note_long;
        end_time[id] = cet;
        k[id] = Ck;
        b[id] = Cb;
        a[id] = Ca;
        w[id] = Cw;
        o[id] = Co;
        road[id] = cr;
        pan_road[id] = cpr;
        mode[id] = cm;
        DeId[id] = deterid;
        deviation[id] = cdev;
    }
}