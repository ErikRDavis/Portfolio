using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float explosionRadius = 3;
    public float explosionForce = 20;
    public float upwardsModifier = 2;

    public virtual void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        for(int i = 0; i < hits.Length; i++)
        {
            hits[i].attachedRigidbody?.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
        }
    }
}
