using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgain : MonoBehaviour
{
    private float toz = 0;
    private Vector2 toscale = new Vector2(1, 1);
    private void OnMouseEnter()
    {
        toscale = new Vector2(2, 2);
        toz = 180;
    }
    private void OnMouseExit()
    {
        toscale = new Vector2(1, 1);
        toz = 0;
    }
    private void Update()
    {
        if (SettleController.level > 0)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, toscale, 0.05f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, toz), 0.05f);
        }
    }
}
