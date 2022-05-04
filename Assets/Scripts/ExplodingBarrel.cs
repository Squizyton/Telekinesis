using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplodingBarrel : Pullable
{
   [SerializeField]private Vector2 explosionForce;
   [SerializeField]private float explosionRadius = 5f;
   public bool currentlyExploding = false;
   public LayerMask LayerMask;
   public void OnTriggerEnter(Collider other)
   {
       if(thrown)
         Explode();
   }


   private void Explode()
   {
       currentlyExploding = true;
       
       Debug.Log(gameObject.name + " is exploding");
       
       var maxColliders = 50;
       var hitColliders = new Collider[maxColliders];
       //No garbage allocation and it's faster
       var numColliders = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders, LayerMask);
       Debug.Log(numColliders);
       

       for (int i = 0; i < numColliders; i++)
       {
           var explosionAmount = UnityEngine.Random.Range(explosionForce.x, explosionForce.y);
           var obj = hitColliders[i].gameObject;
           Debug.Log(obj.name);
           
           if (obj.TryGetComponent(out ExplodingBarrel otherBarrel) && !otherBarrel.currentlyExploding)
           {
               otherBarrel.Explode();
           }

           var rb = obj.GetComponent<Rigidbody>();
           if (rb != null)
           {
               Debug.Log("adding force");
               rb.AddForce(obj.transform.position - transform.position * explosionAmount * Time.deltaTime,
                   ForceMode.Impulse);
           }
       }
       Destroy(gameObject);
   }


   private void OnDrawGizmos()
   {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, explosionRadius);
   }
}
