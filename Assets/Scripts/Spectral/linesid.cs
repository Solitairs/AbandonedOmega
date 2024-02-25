using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linesid : MonoBehaviour
{
    public uint id = 0;
    public Pos pos;
    public APos apos;
    public SpriteRenderer[] sprite;
    public struct Pos
    {
        public bool normal;
        public float k, a, w, q, b;
    }
    public struct APos
    {
        public bool normal;
        public float k, a, w, q, b;
    }
    private void Awake()
    {
        pos.normal = apos.normal = true;
        pos.k = apos.k = 0;
        pos.b=apos.b = 0;
        
    }
    private void Update()
    {
        if (pos.normal)
        {
            transform.position = new Vector3(0, 5.1f- (pos.k*NoteController.game_time+pos.b) * 0.0102f);
        }
        else
        {
            transform.position = new Vector3(0, 5.1f - (pos.a * Mathf.Sin(pos.w*NoteController.game_time+pos.q) + pos.b) * 0.0102f);
        }
        if (apos.normal)
        {
            sprite[0].color = new Color(sprite[0].color.r, sprite[0].color.g, sprite[0].color.b, apos.k * NoteController.game_time + apos.b);
            sprite[1].color = new Color(sprite[1].color.r, sprite[1].color.g, sprite[1].color.b, apos.k * NoteController.game_time + apos.b);
            sprite[2].color = new Color(sprite[2].color.r, sprite[2].color.g, sprite[2].color.b, apos.k * NoteController.game_time + apos.b);
            sprite[3].color = new Color(sprite[3].color.r, sprite[3].color.g, sprite[3].color.b, apos.k * NoteController.game_time + apos.b);
        }
        else
        {
            sprite[0].color = new Color(sprite[0].color.r, sprite[0].color.g, sprite[0].color.b, apos.a * Mathf.Sin(apos.w * NoteController.game_time + apos.q) + apos.b);
            sprite[1].color = new Color(sprite[1].color.r, sprite[1].color.g, sprite[1].color.b, apos.a * Mathf.Sin(apos.w * NoteController.game_time + apos.q) + apos.b);
            sprite[2].color = new Color(sprite[2].color.r, sprite[2].color.g, sprite[2].color.b, apos.a * Mathf.Sin(apos.w * NoteController.game_time + apos.q) + apos.b);
            sprite[3].color = new Color(sprite[3].color.r, sprite[3].color.g, sprite[3].color.b, apos.a * Mathf.Sin(apos.w * NoteController.game_time + apos.q) + apos.b);
        }
    }
}
