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
   
   void Update()
   {
      Debug.Log(inputs.acquireObject);
      
      switch (inputs.acquireObject)
      {
         case true when !readyToThrow:
            anim.SetTrigger("ForcePull");
            readyToThrow = true;
            break;
         case true when readyToThrow:
            anim.SetTrigger("Throw");
            readyToThrow = false;
            break;
      }
   }
   
   
}
