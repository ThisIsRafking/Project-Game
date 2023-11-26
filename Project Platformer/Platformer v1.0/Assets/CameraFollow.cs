using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float damping;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

   void FixedUpdate() {
    Vector3 Movement = Target.position + offset;
    transform.position = Vector3.SmoothDamp(transform.position, Movement, ref velocity, damping);
   }
}
