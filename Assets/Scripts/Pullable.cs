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
    private bool thrown = false;
    private Rigidbody rb { get; set; }

    private Vector3 randomRotation;
    
    private BoxCollider boxCollider;
    public Rigidbody Rb => rb;
    private void Start()
    {
        randomRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        pulled = false;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (pulled)
        {
            //Add 
            rb.AddTorque(randomRotation * 35f);
        }
    }

    public void GetPulled()
    {
        //Turn off the box collider so it doesn't interfere with the player's movement or collide with anything
        rb.useGravity = false;
        boxCollider.enabled = false;
    }

    public void GotThrown()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        pulled = false;
        thrown = true;
        rb.useGravity = true;
        boxCollider.enabled = true;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (thrown && !collision.transform)
        {
            thrown = false;
        }
    }
}
