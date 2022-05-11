using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class PlayerTelekinises : MonoBehaviour
{
    [Header("Camera")] public Camera mainCamera;

    [Header("UI")] public TargetUIThing teleTargetUI;

    [Header("Player Variables")] [SerializeField]
    private StarterAssetsInputs inputs;
    [SerializeField] private Transform telekinesisPoint;
    [SerializeField] private Animator anim;
    [SerializeField] private bool readyToThrow;
    [SerializeField] private GameObject teleObject;

    [Header("Telekinisis Variables")] 
    private const float coolDownTimer = 1f;
    [SerializeField]private bool canUse = true;

    [Header("Pull Variables")] [SerializeField]
    private float pullForce;

    [SerializeField] private float pullDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Pullable pullingObject;

    [Header("Throw Variables")] [SerializeField]
    private float throwForce;

    private void Start()
    {
        //If the teleUI is active, turn it off
        if (teleTargetUI.gameObject.activeSelf) teleTargetUI.gameObject.SetActive(false);
    }

    void Update()
    {

        if (pullingObject)
        {
            Debug.Log("Pulling");
            PullObject(pullingObject);
            return;
        }

        #region Raycasting
        var screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        //Make a ray that goes from the camera to the middle of the screen
        var ray = mainCamera.ScreenPointToRay(screenPoint);
        //Shoot out a raycast
        if (Physics.Raycast(ray, out var hit, pullDistance, layerMask) && !teleObject && canUse)
        {
            //If the raycast hits something
            if (hit.transform.GetComponent<Pullable>().thrown) return;
            
                teleTargetUI.gameObject.SetActive(true);
                //Set the UI to the position of the object
                teleTargetUI.GivePos(hit.transform.position);
        }
        else
        {
            //If we aren't hitting anything, turn off the UI
            teleTargetUI.gameObject.SetActive(false);
        }

        #endregion

        //If we Right click, and the ability is off cool down and we are hitting something
        if (inputs.acquireObject &&hit.transform && canUse)
        {
            //Set the object we are pulling towards us to the object we are hitting
            pullingObject = hit.transform.GetComponent<Pullable>();
            //Set animation states
            anim.SetBool("Throw", false);
            anim.SetBool("Pull", true);
            //Turn off UI
            teleTargetUI.gameObject.SetActive(false);
            canUse = false;
        }

        if (inputs.Throw && readyToThrow)
        {
            anim.SetBool("Throw", true);
            StartCoroutine(CoolDown());
            ThrowObject();
            readyToThrow = false;
        }

    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownTimer);
        canUse = true;
    }


    private void PullObject(Pullable obj)
    {
        
        obj.GetPulled();
        //Make a vector from the telekinesis point to the object   
       obj.transform.DOMove(telekinesisPoint.position, 0.1f);
        
        var distance = Vector3.Distance(transform.position, telekinesisPoint.position);

        if (!(distance > .2f)) return;
        
        teleObject = obj.transform.gameObject;
        pullingObject = null;
        readyToThrow = true;
    }

    private void ThrowObject()
    {
        //rotate the player
        var obj = teleObject.GetComponent<Pullable>();
        obj.GotThrown();
        //Remove Parent
        obj.transform.SetParent(null);
        //Aim direction to center of screen
        var centerOfScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        var ray = mainCamera.ViewportPointToRay(centerOfScreen);

        var direction = (Vector3.zero - telekinesisPoint.transform.position).normalized;
        
        obj.Rb.AddForce(( hit.point - telekinesisPoint.position) * throwForce, ForceMode.Impulse);
                
        //Set teleObject to null
        teleObject = null;
    }
}
