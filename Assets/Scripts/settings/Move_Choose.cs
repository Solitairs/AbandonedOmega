using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Move_Choose : MonoBehaviour
{
    public uint mode;
    public GameObject child;
    public TextMeshProUGUI text;
    private bool isdown = false;
    public void OnMouseEnter()
    {
        isdown = true;
    }
    public void OnMouseExit()
    {
        isdown = false;

    }


    // Update is called once per frame
    void Update()
    {
        // 168 / 140
        if (isdown)
        {
            child.GetComponent<RectTransform>().localScale = Vector3.Lerp(child.GetComponent<RectTransform>().localScale, new Vector3(0.77f, 0.77f,1), 0.08f);
        }
        else
        {
            child.GetComponent<RectTransform>().localScale = Vector3.Lerp(child.GetComponent<RectTransform>().localScale, new Vector3(1, 1, 1), 0.08f);
        }
    }
    private Vector3 screenPos;
    private Vector3 offset;
    void OnMouseDown()
    {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);//获取物体的屏幕坐标     
        offset = screenPos - Input.mousePosition;//获取物体与鼠标在屏幕上的偏移量    
    }
    void OnMouseDrag()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition + offset);//将拖拽后的物体屏幕坐标还原为世界坐标
        transform.localPosition = new Vector3(Mathf.Clamp(transform.parent.InverseTransformPoint(temp).x,-422f,610f), transform.localPosition.y);
        if (mode == 0)
        {
            //volume
            PlayerPrefs.SetFloat("volume", (transform.localPosition.x + 422f) / 1032f);
            PlayerPrefs.Save();
            AudioListener.volume = (transform.localPosition.x + 422f) / 1032f;
            text.text = Mathf.RoundToInt(((transform.localPosition.x + 422f) / 1032f) *100).ToString() + "%";
        }
        else if(mode == 1)
        {
            PlayerPrefs.SetFloat("speed", 0.1f+(transform.localPosition.x + 422f) / 1032f*3.9f);
            PlayerPrefs.Save();
            text.text = (Mathf.Round(10f + (transform.localPosition.x + 422f) / 1032f * 390f) / 100).ToString() + "x";
        }
    }
    private void Start()
    {
        if (mode == 0)
        {
            transform.localPosition = new Vector3(PlayerPrefs.GetFloat("volume") * 1032f - 422f, transform.localPosition.y);
            text.text = Mathf.RoundToInt(((transform.localPosition.x + 422f) / 1032f) * 100).ToString() + "%";
        }
        else if(mode == 1)
        {
            transform.localPosition = new Vector3((PlayerPrefs.GetFloat("speed")-0.1f) /3.9f*1032f - 422f, transform.localPosition.y);
            text.text = (Mathf.Round(10f + (transform.localPosition.x + 422f) / 1032f * 390f) / 100).ToString()+"x";
        }
    }
}
