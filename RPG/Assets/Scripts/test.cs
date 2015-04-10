using UnityEngine;
using System.Collections;
using BestHTTP;
using System;
[ExecuteInEditMode] 
public class test : MonoBehaviour
{
    Vector3 point = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(point);
    }


}
