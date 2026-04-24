using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Movement : MonoBehaviour
{
    private bool _speedActive = false;
    public float _speedMod = 1.5f;
    public float _maxSpeed = 10f;
    private float _speedValue;

    public float _duration = 0.5f;
    public float _targetSpeed = -5f;

    private Rigidbody rb;

    private Coroutine _fatassCoroutine;

    void Start()
    {
        _speedValue = 0f;
        rb = GetComponent<Rigidbody>();
    }
    [ContextMenu("StartGame")]
    void TriggerStart()
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_speedActive == true)
        {
            _speedValue *= Mathf.Pow(_speedMod, Time.fixedDeltaTime);
        }

        Vector3 movement = Vector3.forward * _speedValue * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
        _speedValue = Mathf.Min(_speedValue, _maxSpeed);
    }
    [ContextMenu("Fatass")]
    public void Fatass()
    {
        if (_fatassCoroutine != null)
        {
            StopCoroutine(_fatassCoroutine);
        }
        _fatassCoroutine = StartCoroutine(FatassRoutine());
    }


    private IEnumerator FatassRoutine()
    {
        _speedActive = false;

        float _timer = 0f;
        float _startSpeed = _speedValue;

        while (_timer < _duration)
        {
            _timer += Time.fixedDeltaTime;

            _speedValue = Mathf.Lerp(_startSpeed, _targetSpeed, _timer / _duration);

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
        
        StartCoroutine(GetMoving());
        
    }
}
