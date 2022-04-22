using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerTelekinises : MonoBehaviour
{
    //TODO Refactor all of this

    [Header("Camera")] public Camera mainCamera;

    [Header("UI")] public RotateTowardsPlayer teleTargetUI;

    [Header("Player Variables")] [SerializeField]
    private StarterAssetsInputs inputs;

    [SerializeField] private Transform telekinesisPoint;
    [SerializeField] private Animator anim;
    [SerializeField] private bool readyToThrow;
    [SerializeField] private GameObject teleObject;


    [Header("Pull Variables")] [SerializeField]
    private float pullForce;

    [SerializeField] private float pullDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform pullingObject;

    [Header("Throw Variables")] [SerializeField]
    private float throwForce;


    private GameObject cachedHit;

    private void Start()
    {
        //If the teleUI is active, turn it off
        if (teleTargetUI.gameObject.activeSelf) teleTargetUI.gameObject.SetActive(false);
    }

    void Update()
    {
        #region Raycasting

        var screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        //Make a ray that goes from the camera to the middle of the screen
        var ray = mainCamera.ScreenPointToRay(screenPoint);
        //Shoot out a raycast
        if (Physics.Raycast(ray, out var hit, pullDistance, layerMask))
        {
            //If the raycast hits something
            if (hit.transform != null && hit.transform.gameObject != cachedHit)
            {
                cachedHit = hit.transform.gameObject;

                if (!teleTargetUI.gameObject.activeSelf)
                {
                    teleTargetUI.gameObject.SetActive(true);
                }

                teleTargetUI.UpdateRotation();
            }
        }
        else
        {
            cachedHit = null;
            teleTargetUI.gameObject.SetActive(false);
        }

        #endregion


        if (teleTargetUI.gameObject.activeSelf)
        {
            if (teleTargetUI.UpdatePositionDistance(hit.point) > 0.1f)
            {
                teleTargetUI.UpdatePosition(hit.point);
            }
        }


        if (inputs.acquireObject && !readyToThrow && hit.transform != null)
        {
            anim.SetBool("Throw", false);
            //Get the position in the middle of the screen
            teleTargetUI.gameObject.SetActive(false);
            pullingObject = hit.transform;
            anim.SetBool("Pull", true);
        }

        if (inputs.Throw && readyToThrow)
        {
            anim.SetBool("Throw", true);
            ThrowObject();

            readyToThrow = false;
        }


        if (pullingObject != null)
        {
            PullObject(pullingObject);
        }
    }

    private void PullObject(Transform transform)
    {
        var obj = transform.GetComponent<Pullable>();
        //Set the rigidbody to kinematic
        obj.Rb.useGravity = false;
        //obj.Rb.isKinematic = true;

        //Move the transform to the telekinesis point
        transform.position = Vector3.Lerp(transform.position, telekinesisPoint.position, Time.deltaTime * pullForce);

        //Check the distance between the object and the telekinesis point
        var distance = Vector3.Distance(transform.position, telekinesisPoint.position);

        //if the position of the object is not close to the telekinesis point return
        if (distance > .2f) return;

        //Set the anim.Trigget to pull
        anim.SetBool("Pull", false);

        //Set the transform's parent to the telekinesis point so it will follow us
        transform.SetParent(telekinesisPoint);

        //Add this object to teleObject
        teleObject = transform.gameObject;
        //Add random rotation to the object
        obj.pulled = true;
        //Tell Game we arent pulling anymore
        pullingObject = null;
        //We are ready to throw
        readyToThrow = true;
    }

    private void ThrowObject()
    {
        //Get the rigidbody of the object
        var obj = teleObject.GetComponent<Rigidbody>();
        //Set the rigidbody to use  gravity

        obj.GetComponent<Pullable>().pulled = false;

        obj.useGravity = true;

        //Remove Parent
        obj.transform.SetParent(null);

        //Aim direction to center of screen


        var centerOfScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        var ray = mainCamera.ViewportPointToRay(centerOfScreen);

        var direction = (Vector3.zero - telekinesisPoint.transform.position).normalized;

        if (Physics.Raycast(ray, out var hit, pullDistance))
        {
            direction = hit.point - telekinesisPoint.transform.position;
        }


        //Add Force to the object
        obj.AddRelativeForce(this.transform.forward * throwForce, ForceMode.Impulse);

        //Reset the object
        teleObject = null;
    }
}