using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(CountdownEvent());
    }
    
    private IEnumerator CountdownEvent()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
