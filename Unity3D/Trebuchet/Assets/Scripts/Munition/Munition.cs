using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MunitionState
{
    NORMAL,
    BROKEN
}

public class Munition : MonoBehaviour, IPoolItem
{
    public Vector3 Velocity => rb.velocity;
    public MunitionState State { get; protected set; } = MunitionState.NORMAL;

    [SerializeField]
    private ParticleSystem impactParticles;
    [SerializeField]
    private TrailRenderer trailRenderer;
    [SerializeField]
    private GameObject mainVisual;
    [SerializeField]
    private float speedForDestruction = 5;
    [SerializeField]
    private float despawnTimeAfterImpact = 5;
    [SerializeField]
    private Collider munitionCollider;
    [SerializeField]
    private Rigidbody rb;

    public UnityEvent onBreak;

    private FixedJoint joint;

    public string PoolKey { get; set; }
    public GameObject GameObject => gameObject;

    private void OnEnable()
    {
        ResetMunition();
    }

    private void ResetMunition()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
        mainVisual.SetActive(true);
        munitionCollider.enabled = true;
        trailRenderer.enabled = false;
        State = MunitionState.NORMAL;

        if (joint)
        {
            Destroy(joint);
        }
    }

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel;
    }

    public void ConnectToSling(Rigidbody sling)
    {
        ResetMunition();

        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = sling;
    }

    public void ReleaseFromSling()
    {
        trailRenderer.enabled = true;
        transform.SetParent(null);
        Destroy(joint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnImpact(collision);
    }

    protected virtual void OnImpact(Collision collision)
    {
        if (joint) return;

        if(collision.relativeVelocity.sqrMagnitude >= speedForDestruction)
        {
            BreakMunition(collision);
        }
    }

    protected virtual void BreakMunition(Collision collision)
    {
        if (State == MunitionState.BROKEN) return;

        State = MunitionState.BROKEN;
        mainVisual.SetActive(false);
        impactParticles.Play();
        munitionCollider.enabled = false;
        rb.isKinematic = true;
        transform.forward = -collision.relativeVelocity;
        impactParticles.transform.forward = Vector3.Reflect(transform.forward, Vector3.up);

        MunitionManager.WaitThenDespawnMunition(despawnTimeAfterImpact, this);

        onBreak?.Invoke();
    }

    public void Despawn()
    {
        // Required by IPoolItem, but despawning is actaully controlled by the MunitionManager
    }
}
