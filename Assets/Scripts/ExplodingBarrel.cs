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

   public void OnTriggerEnter(Collider other)
   {
       if(thrown)
         Explode();
   }


   public void Explode()
   {
       currentlyExploding = true;
       var objects = Physics.OverlapSphere(transform.position,explosionRadius).ToList();
       
       foreach(var obj in objects)
       {
           var explosionAmount = UnityEngine.Random.Range(explosionForce.x, explosionForce.y);

           if (obj.TryGetComponent(out ExplodingBarrel otherBarrel) && !otherBarrel.currentlyExploding)
           {
               otherBarrel.Explode();
           }
           
           var rigidbody = obj.GetComponent<Rigidbody>();
           if(rigidbody != null)
           {
               rigidbody.AddForce(obj.transform.position - transform.position* explosionAmount  * Time.deltaTime, ForceMode.Impulse);
           }
           
           
       }
       
       Destroy(gameObject);
   }
}
