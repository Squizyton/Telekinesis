using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AddForce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-10f,10f),UnityEngine.Random.Range(-10,10),UnityEngine.Random.Range(-10,10)),ForceMode.Impulse);        
    }

}
