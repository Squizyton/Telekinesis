using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This script will just allow us to tell the Telekinesis script that its pull-able
/// </summary>
public class Pullable : MonoBehaviour
{
    public bool pulled = false;
    private Rigidbody rb { get; set; }

    private Vector3 randomRotation;
    
    public Rigidbody Rb => rb;
    private void Start()
    {
        randomRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        pulled = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (pulled)
        {
            //Add 
            rb.AddTorque(randomRotation * 35f);
        }
    }
}
