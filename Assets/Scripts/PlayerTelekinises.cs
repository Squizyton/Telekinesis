using System.Collections;
using System.Collections.Generic;

using StarterAssets;
using UnityEngine;

public class PlayerTelekinises : MonoBehaviour
{
   [Header("Camera")] public Camera mainCamera;
   
   
   [Header("Player Variables")]
   [SerializeField]private StarterAssetsInputs inputs;
   [SerializeField]private Transform telekinesisPoint;
   [SerializeField] private Animator anim;
   [SerializeField] private bool readyToThrow;
   
   [SerializeField]private Rigidbody objectBeingTelekinesisedRB;
   
   
   
   [Header("Pull Variables")]
   [SerializeField] private float pullForce;
   [SerializeField] private float pullDistance;
   [SerializeField]private LayerMask layerMask;
   [SerializeField] private Transform pullingObject;
   void Update()
   {
      if (inputs.acquireObject && !readyToThrow)
      {
         //Get the position in the middle of the screen
         var screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
         //Make a ray that goes from the camera to the middle of the screen
         var ray = mainCamera.ScreenPointToRay(screenPoint);
         //Shoot out a raycast
         if (Physics.Raycast(ray,out var hit,pullDistance,layerMask)) ;
         {
            //If the raycast hits something
            if (hit.transform.GetComponent<Pullable>())
            {
               pullingObject = hit.transform;
               //Set the anim.Trigget to pull
               anim.SetTrigger("ForcePull");
            }
         }
      }

      if (pullingObject != null)
      {
         PullObject(pullingObject);
      }

      if(inputs.Throw && readyToThrow)
      {
         anim.SetTrigger("Throw");
         readyToThrow = false;
      }
   }

   private void PullObject(Transform transform)
   {
      var obj = transform.GetComponent<Pullable>();
      //Set the rigidbody to kinematic
      obj.Rb.useGravity = false;
      //obj.Rb.isKinematic = true;
      
     //Move the transform to the telekinesis point
     transform.position = Vector3.Lerp(transform.position,telekinesisPoint.position,Time.deltaTime * pullForce);
      
     var distance = Vector3.Distance(transform.position,telekinesisPoint.position);
     
      //if the position of the object is close to the telekinesis point
      if (distance > .2f) return;
      
      //Set the transform's parent to the telekinesis point so it will follow us
      transform.SetParent(telekinesisPoint);
      
      //Add random rotation to the object
     obj.pulled = true;
      
      pullingObject = null;
      //WE are ready to throw
      readyToThrow = true;
   }
   
   private void ThrowObject()
   {
      
   }
}
