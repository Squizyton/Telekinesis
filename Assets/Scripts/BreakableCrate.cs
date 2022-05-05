using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableCrate : Pullable
{
    [SerializeField] public GameObject replacement;


    private void OnTriggerEnter(Collider other)
    {
        if (!thrown) return;

        if (!other.transform) return;
        
        
        if(other.transform.TryGetComponent(out Pullable pullable))
            pullable.SpecialInteraction();
            
        Instantiate(replacement, transform.position, transform.rotation);
        Destroy(gameObject);
    }


    public override void SpecialInteraction()
    {
        Instantiate(replacement, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
