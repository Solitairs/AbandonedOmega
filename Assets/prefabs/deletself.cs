using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletself : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("deletes", 1.2f);
    }
    void deletes()
    {
        Destroy(gameObject);
    }
}
