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
   [SerializeField]private float explosionDelay = 0.5f;
   [SerializeField] private GameObject fireParticles;
   [SerializeField] private GameObject explosionParticles;
   
   
   public void OnTriggerEnter(Collider other)
   {
       if(thrown)
         Explode();
   }


   public override void SpecialInteraction()
   {
       StartCoroutine(StartTimer());
   }

   IEnumerator StartTimer()
   {
       fireParticles.SetActive(true);
       yield return new WaitForSeconds(explosionDelay);
       Explode();
   }   
   

   private void Explode()
   {
       currentlyExploding = true;
       
       Instantiate(explosionParticles, transform.position, Quaternion.identity);
       
       const int maxColliders = 50;
       var hitColliders = new Collider[maxColliders];
       //No garbage allocation and it's faster
       var numColliders = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders, LayerMask);
       
       for (var i = 0; i < numColliders; i++)
       {
           var explosionAmount = UnityEngine.Random.Range(explosionForce.x, explosionForce.y);
           var obj = hitColliders[i].gameObject;

           
           if(obj.TryGetComponent(out Pullable pullable))
               pullable.SpecialInteraction();
           
           
           if (obj.TryGetComponent(out ExplodingBarrel otherBarrel) && !otherBarrel.currentlyExploding)
           {
               otherBarrel.SpecialInteraction();
               return;
           }

           var rb = obj.GetComponent<Rigidbody>();
           
           if (!rb) return;
           
           Debug.Log("adding force");
           rb.AddForce(obj.transform.position - transform.position * (explosionAmount * Time.deltaTime),
               ForceMode.Impulse);
       }
       Destroy(gameObject);
   }


   private void OnDrawGizmos()
   {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, explosionRadius);
   }
}
