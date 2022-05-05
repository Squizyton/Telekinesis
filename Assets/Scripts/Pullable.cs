using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This script will just allow us to tell the Telekinesis script that its pull-able
/// </summary>
public abstract class Pullable : MonoBehaviour
{
    public bool pulled = false;
    public bool thrown = false;
    private Rigidbody rb { get; set; }
    public Rigidbody Rb => rb;
    
    
    
    [SerializeField]private Collider objCollider;
    
    
    private Vector3 randomRotation;
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

    public void GetPulled()
    {
        objCollider.enabled = false;
        //Turn off the box collider so it doesn't interfere with the player's movement or collide with anything
        rb.useGravity = false;
    }

    public void GotThrown()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        pulled = false;
        thrown = true;
        rb.useGravity = true;
        objCollider.enabled = true;
    }

    public abstract void SpecialInteraction();
}
