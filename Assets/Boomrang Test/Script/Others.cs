using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Others : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //for now changing the only color 
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
