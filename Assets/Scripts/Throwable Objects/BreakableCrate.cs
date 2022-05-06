using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableCrate : Pullable
{
    [Header("Class Variables")]
    [SerializeField] public GameObject replacement;


    private void OnTriggerEnter(Collider other)
    {
        if (!thrown) return;

        if (!other.transform) return;
        
        
        if(other.transform.TryGetComponent(out Pullable pullable))
            pullable.SpecialInteraction();
            
      SpecialInteraction();
    }


    public override void SpecialInteraction()
    {
        CinemachineShake.instance.ShakeCamera(2f,.2f);
        Instantiate(replacement, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
