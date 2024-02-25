using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class del_self : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("del", 1.5f);   
    }
    void del()
    {
        Destroy(gameObject);
    }
}
