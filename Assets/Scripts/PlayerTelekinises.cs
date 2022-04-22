using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerTelekinises : MonoBehaviour
{
   [SerializeField]private StarterAssetsInputs inputs;
   [SerializeField]private Transform telekinesisPoint;
   [SerializeField] private Animator anim;
   [SerializeField] private bool readyToThrow;
   
   
   
   
   [Header("Pull Variables")]
   [SerializeField] private float pullForce;
   [SerializeField] private float pullDistance;
   
   void Update()
   {

      
      
      if (inputs.acquireObject && !readyToThrow)
      {
         anim.SetTrigger("ForcePull");
         
         readyToThrow = true;
         
         
      }
    
      if(inputs.Throw && readyToThrow)
      {
         anim.SetTrigger("Throw");
         readyToThrow = false;
      }


      if (transform != null)
      {
         
      }
   }
}
