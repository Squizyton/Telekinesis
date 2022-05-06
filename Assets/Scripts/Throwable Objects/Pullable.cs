using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This script will just allow us to tell the Telekinesis script that its pull-able
/// </summary>
public abstract class Pullable : MonoBehaviour
{
    [Header("Inherited Values")]
    public bool pulled = false;
    public bool thrown = false;
    private Rigidbody rb { get; set; }
    public Rigidbody Rb => rb;
    
    
    [SerializeField]private Collider nonTriggerCollider;
    
    
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
       //Turn off the non-trigger collider so it doesn't interfere with the player's movement or collide with anything
        nonTriggerCollider.enabled = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    public void GotThrown()
    {
      
        transform.rotation = Quaternion.Euler(Vector3.zero);
        pulled = false;
        thrown = true;
        rb.useGravity = true;
        nonTriggerCollider.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
    }

    public abstract void SpecialInteraction();
}
