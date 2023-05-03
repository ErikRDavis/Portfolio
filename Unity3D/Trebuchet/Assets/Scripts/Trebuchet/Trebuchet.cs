using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

public enum TrebuchetState
{
    WAITING,
    REARMING,
    ARMED,
    FIRING
}

public partial class Trebuchet : MonoBehaviour
{
    public delegate void OnReleaseAngleChanged(float angle);
    public event OnReleaseAngleChanged onReleaseAngleChanged;

    public TrebuchetState State { get; private set; } = TrebuchetState.WAITING;

    [SerializeField]
    private Transform arm;
    [SerializeField]
    private Rigidbody armRigidbody;
    [SerializeField]
    private Transform sling;
    [SerializeField]
    private Rigidbody slingRigidbody;
    [SerializeField]
    private Transform slingConnectionPoint;

    [SerializeField]
    private Quaternion slingArmedAngle;
    [SerializeField]
    private Quaternion armArmedAngle;

    [SerializeField]
    private TrebuchetData data;

    private Munition currentMunition;

    private float munitionReleaseAngle;
    private float currentRotationValue;
    private Coroutine updateReleaseAngle;
    private float updateReleaseAngleValue;

    // Start is called before the first frame update
    void Start()
    {
        sling.gameObject.SetActive(false);
    }

    public void SetData(TrebuchetData data)
    {
        this.data = data;
    }

    public void SetMunitionReleaseAngle(float angle)
    {
        munitionReleaseAngle = angle;
    }

    private void ArmTrebuchet()
    {
        sling.gameObject.SetActive(true);
        armRigidbody.isKinematic = true;
        slingRigidbody.isKinematic = true;

        arm.localRotation = armArmedAngle;
        sling.localRotation = slingArmedAngle;

        currentMunition = MunitionManager.GetMunition(data.munitionKey);

        // Parent the munition to the sling to prevent movement when rotating the trebuchet left/right
        currentMunition.transform.SetParent(slingConnectionPoint);
        currentMunition.transform.localPosition = Vector3.zero;

        currentMunition.ConnectToSling(slingRigidbody);

        slingRigidbody.isKinematic = false;

        State = TrebuchetState.ARMED;
    }

    private void ReleaseArm()
    {
        if (State != TrebuchetState.ARMED) return;

        armRigidbody.isKinematic = false;
        currentMunition.transform.SetParent(null);

        State = TrebuchetState.FIRING;

        StartCoroutine(MunitionReleaseWatcher(munitionReleaseAngle));
    }

    private IEnumerator MunitionReleaseWatcher(float releaseAngle)
    {
        slingRigidbody.velocity = slingRigidbody.angularVelocity = Vector3.zero;

        float munitionAngle;
        do
        {
            munitionAngle = Vector3.SignedAngle(currentMunition.Velocity.normalized, transform.forward, transform.right);            

            yield return new WaitForEndOfFrame();
        }
        while (currentMunition.Velocity.sqrMagnitude < 100 || munitionAngle > releaseAngle || munitionAngle < -90);

        currentMunition.ReleaseFromSling();
        MunitionManager.DespawnAfterLifetime(currentMunition);

        // Enforce the munition actually leaves the sling at the release angle the player thinks it does
        //===========================================================
        munitionAngle = Vector3.SignedAngle(currentMunition.Velocity.normalized, transform.forward, transform.right);

        currentMunition.SetVelocity(Quaternion.AngleAxis(-(releaseAngle - munitionAngle), transform.right) * currentMunition.Velocity);
        //===========================================================

        sling.gameObject.SetActive(false);

        State = TrebuchetState.WAITING;

        if (data.autoRearm)
        {
            RearmTrebuchet();
        }
    }

    public void RearmTrebuchet()
    {
        if (State != TrebuchetState.WAITING) return;

        StartCoroutine(Co_RearmTrebuchet());
    }

    private IEnumerator Co_RearmTrebuchet()
    {
        armRigidbody.isKinematic = true;

        State = TrebuchetState.REARMING;

        while (Quaternion.Angle(arm.localRotation, armArmedAngle) > 0.5f)
        {
            yield return null;
            arm.Rotate(Vector3.right, -data.rearmSpeed * Time.deltaTime, Space.Self);
        }

        arm.localRotation = armArmedAngle;

        ArmTrebuchet();
    }

    private void Update()
    {
        UpdateBodyRotation();
    }

    private void UpdateBodyRotation()
    {
        if(currentRotationValue != 0 && State != TrebuchetState.FIRING)
        {
            transform.Rotate(Vector3.up, data.maxBodyRotateSpeed * currentRotationValue * Time.deltaTime);
        }
    }

    private void UpdateReleaseAngle(float value)
    {
        updateReleaseAngleValue = value;

        if (updateReleaseAngleValue == 0 && updateReleaseAngle != null)
        {
            StopCoroutine(updateReleaseAngle);
            updateReleaseAngle = null;
        }
        else if(updateReleaseAngleValue != 0 && updateReleaseAngle == null)
        {
            updateReleaseAngle = StartCoroutine(Co_UpdateReleaseAngle());
        }
    }

    private IEnumerator Co_UpdateReleaseAngle()
    {
        while(true)
        {
            munitionReleaseAngle += updateReleaseAngleValue * Time.deltaTime * data.updateReleaseAngleSpeed;

            munitionReleaseAngle = Mathf.Clamp(munitionReleaseAngle, data.minReleaseAngle, data.maxReleaseAngle);

            onReleaseAngleChanged?.Invoke(munitionReleaseAngle);

            if (munitionReleaseAngle == data.minReleaseAngle || munitionReleaseAngle == data.maxReleaseAngle)
            {
                updateReleaseAngle = null;
                yield break;
            }

            yield return null;
        }
    }
}
