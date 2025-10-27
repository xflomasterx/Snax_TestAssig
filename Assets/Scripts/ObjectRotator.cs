using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.zero;
   
    void FixedUpdate()
    {
        transform.Rotate(rotationAxis);
    }
}
