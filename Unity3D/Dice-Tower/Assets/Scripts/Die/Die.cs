using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    public bool IsMoving => rb.velocity.sqrMagnitude > 0.0001f && rb.angularVelocity.sqrMagnitude > 0.0001f;

    [SerializeField]
    private Transform[] faces;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ResetDie()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
        EnablePhysics();
    }

    public void DisablePhysics()
    {
        rb.isKinematic = true;
    }

    public void EnablePhysics()
    {
        rb.isKinematic = false;
    }

    public void TumbleDie(Vector3 force, Vector3 torque)
    {
        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(torque);
    }

    public int GetScoringSide()
    {
        int closestFace = 0;
        Transform face;
        float closestDot = Vector3.Dot(Vector3.up, faces[closestFace].up);

        for(int i = 0; i < faces.Length; i++)
        {
            face = faces[i];
            float faceDot = Vector3.Dot(Vector3.up, face.up);
            if (faceDot > closestDot)
            {
                closestFace = i;
                closestDot = faceDot;
            }
        }

        return closestFace + 1;
    }
}
