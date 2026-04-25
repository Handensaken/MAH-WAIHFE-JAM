using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Movement : MonoBehaviour
{
    private bool _speedActive = false;
    [Tooltip("How quickly you speed up")]
    public float _speedMod = 1.5f;
    [Tooltip("Speedclamp adjusting")]
    public float _maxSpeed = 10f;
    private float _speedValue;

    [Tooltip("How long you're stunned when fatassed")]
    public float _duration = 0.5f;
    [Tooltip("How fast you go back when you're a fatass")]
    public float _targetSpeed = -5f;

    [Tooltip("How far the swiper can reach")]
    public float swipeDistance = 10f;
    [Tooltip("How wide the swiper is")]
    public float swipeWidth = 1f;
    [Tooltip("How tall the swiper is")]
    public float swipeHeight = 1f;
    [Tooltip("How strongly innocents are thrown")]
    public float swipeForce = 5f;
    [Tooltip("Upward force applied")]
    public float swipeUpForce = 1f;
    [Tooltip("Rotate it man")]
    public float rotateForce = 5f;
    //Add random rotation and torque

    private Rigidbody rb;
    private AudioManager AudioManager;

    private Coroutine _fatassCoroutine;
    public LayerMask collisionMask = ~0;

    private bool _inFatass = false;

    void Start()
    {
        _speedValue = 0f;
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        AudioManager = FindAnyObjectByType<AudioManager>();
    }

    [ContextMenu("StartGame")]
    public void TriggerStart()
    {
        StartCoroutine(GetMoving());
    }

    private IEnumerator GetMoving()
    {
        float _timer = 0f;
        float _startSpeed = 0f;
        float _endSpeed = 1f;
        float _accelDuration = 1f;

        while (_timer < _accelDuration)
        {
            _timer += Time.fixedDeltaTime;

            _speedValue = Mathf.Lerp(_startSpeed, _endSpeed, _timer / _accelDuration);

            yield return new WaitForFixedUpdate();
        }

        _speedValue = _endSpeed;

        _speedActive = true;
    }

    void FixedUpdate()
    {
        if (_speedActive == true)
        {
            _speedValue *= Mathf.Pow(_speedMod, Time.fixedDeltaTime);
        }

        Vector3 movement = Vector3.forward * _speedValue * Time.fixedDeltaTime;
        float moveDistance = movement.magnitude;

        if (moveDistance > 0f && rb != null && !_inFatass)
        {
            Vector3 dir = movement.normalized;

            RaycastHit sweepHit;
            bool willHit = rb.SweepTest(dir, out sweepHit, moveDistance + 0.01f, QueryTriggerInteraction.Ignore);

            if (willHit)
            {
                if ((collisionMask.value & (1 << sweepHit.collider.gameObject.layer)) != 0)
                {
                    Vector3 safePos = rb.position + dir * Mathf.Max(0f, sweepHit.distance - 0.01f);
                    rb.MovePosition(safePos);

                    Fatass();
                    return;
                }
            }
            else
            {
                RaycastHit rayHit;
                if (Physics.Raycast(rb.position, dir, out rayHit, moveDistance + 0.01f, collisionMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3 safePos = rb.position + dir * Mathf.Max(0f, rayHit.distance - 0.01f);
                    rb.MovePosition(safePos);

                    Fatass();
                    return;
                }
            }
        }
        rb.MovePosition(rb.position + movement);
        _speedValue = Mathf.Min(_speedValue, _maxSpeed);
    }

    [ContextMenu("Fatass")]
    public void Fatass()
    {
        if (_fatassCoroutine != null)
        {
            StopCoroutine(_fatassCoroutine);
            _fatassCoroutine = null;
        }

        AudioManager.Play("Fatass");
        _fatassCoroutine = StartCoroutine(FatassRoutine());
    }

    private IEnumerator FatassRoutine()
    {
        _inFatass = true;
        _speedActive = false;

        float _timer = 0f;
        float _startSpeed = _speedValue;
        float initialReverseImpulse = 0.5f;

        float startMag = Mathf.Abs(_startSpeed);
        float targetMag = Mathf.Abs(_targetSpeed);
        float targetSign = Mathf.Sign(_targetSpeed);

        if (Mathf.Sign(_startSpeed) != Mathf.Sign(_targetSpeed) && startMag < 0.01f)
        {
            startMag = Mathf.Max(startMag, initialReverseImpulse);
        }

        while (_timer < _duration)
        {
            _timer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(_timer / _duration);

            float mag = Mathf.Lerp(startMag, targetMag, t);
            _speedValue = targetSign * mag;

            yield return new WaitForFixedUpdate();
        }

        _timer = 0f;
        _startSpeed = _speedValue;

        while (_timer < _duration)
        {
            _timer += Time.fixedDeltaTime;

            _speedValue = Mathf.Lerp(_startSpeed, 1f, _timer / _duration);

            yield return new WaitForFixedUpdate();
        }

        _speedValue = 0f;

        _inFatass = false;
        _fatassCoroutine = null;

        StartCoroutine(GetMoving());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_inFatass)
            Fatass();
    }

    public void SwipeLeft()
    {
        SwipeDirectional(-transform.right);
        AudioManager.Play("Grunt");
    }

    public void SwipeRight()
    {
        SwipeDirectional(transform.right);
        AudioManager.Play("Grunt");
    }

    private void SwipeDirectional(Vector3 localXDirection)
    {
        Vector3 center = transform.position + transform.forward * (swipeDistance * 0.5f);
        Vector3 halfExtents = new Vector3(swipeWidth * 0.5f, swipeHeight * 0.5f, swipeDistance * 0.5f);

        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation, collisionMask, QueryTriggerInteraction.Ignore);

        bool Matches;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider c = hits[i];
            if (c.attachedRigidbody == rb) continue;

            Rigidbody targetRb = c.attachedRigidbody;
            if (targetRb == null) continue;
            if (targetRb.isKinematic) continue;

            bool isRightSwipe = localXDirection.x > 0f;

            bool isRightie = c.CompareTag("Rightie");
            bool isLeftie = c.CompareTag("Leftie");

            float forceMultiplier = 1f;

            if ((isRightSwipe && isRightie) || (!isRightSwipe && isLeftie))
            {
                Matches = true;
                forceMultiplier = 1f;
            }
            else
            {
                forceMultiplier = 0.1f;
                Matches = false;
            }

            Vector3 horizontal = localXDirection.normalized;
            Vector3 impulse = (horizontal * swipeForce + Vector3.up * swipeUpForce) * forceMultiplier;

            targetRb.AddForce(impulse, ForceMode.Impulse);
            
            if(Matches)
            {
                AudioManager.Play("Scream");

                if(isRightie)
                {
                    targetRb.AddTorque(Vector3.back * rotateForce, ForceMode.Impulse);
                }
                else
                {
                    targetRb.AddTorque(Vector3.forward * rotateForce, ForceMode.Impulse);
                }

                Collider[] cols = c.gameObject.GetComponentsInChildren<Collider>(true);
                for (int j = 0; j < cols.Length; j++)
                {
                    if (cols[j] != null)
                        cols[j].enabled = false;
                }
            }
        }
}


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + transform.forward * (swipeDistance * 0.5f);
        Vector3 size = new Vector3(swipeWidth, swipeHeight, swipeDistance);
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
