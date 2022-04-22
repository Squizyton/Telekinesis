using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    public void UpdateRotation()
    {
        //Rotate towards player
        var direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
    }
    
    
    public float UpdatePositionDistance(Vector3 target)
    { var distance = Vector3.Distance(transform.position, target);

      return distance;

    }

    public void UpdatePosition(Vector3 target)
    { transform.position =
        Vector3.Lerp(transform.position, target, Time.deltaTime * 20f);

        
    }

}
