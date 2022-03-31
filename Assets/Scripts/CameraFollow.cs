using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    void FixedUpdate() {
        Vector3 newOffset = -target.forward * offset.z;
        newOffset.y=offset.y ;
        transform.position = target.position+newOffset;
        transform.LookAt(target);
    }
}
