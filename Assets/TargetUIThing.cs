using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUIThing : MonoBehaviour
{
    public Vector3 gotoPoint;
    public GameObject player;
    public Transform cam;

    public void Update()
    {
        if(gameObject.activeSelf)
            UpdateRotation();
        
        
        if (gotoPoint.Equals(Vector3.zero)) return;
        
        if(UpdatePosition(gotoPoint) <= .0f)
        {
            gotoPoint = Vector3.zero;
        }
    }


    private void UpdateRotation()
    {
        //Rotate towards player
        var direction = player.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
    }

    public void GivePos(Vector3 pos)
    {
        gotoPoint = pos;
    }
    
    private float UpdatePosition(Vector3 target)
    { 
        transform.position =
        Vector3.Lerp(transform.position, gotoPoint, Time.deltaTime * 50f);

        var distance = Vector3.Distance(transform.position, target);

        return distance;
    }

}
