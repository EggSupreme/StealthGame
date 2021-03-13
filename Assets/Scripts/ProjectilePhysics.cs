using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    Vector3 initialWorldPosition;
    Vector3 initialRotation;
    float anglePerSecond;

    Transform launchTransform;
    Vector3 launchLocalVelocity;

    public GameObject collisionObject;
    public Transform body;

    void Start()
    {
        //Application.targetFrameRate = 30;
    }

    public void Launch(float _launchPower)
    {
        localVelocity.z = _launchPower * 2;

        launchTransform = transform;
        launchLocalVelocity = localVelocity;

        initialWorldPosition = transform.position;
        initialRotation = transform.rotation.eulerAngles;

        Vector4 peak = HighestPointWithinAge(10.0f);
        Vector3 peakPoint = new Vector3(peak.x, peak.y, peak.z);
        float timeToPeak = peak.w;
        Debug.Log("Will reach " + peakPoint + " after " + timeToPeak + " seconds");

        if (timeToPeak <= 0) { timeToPeak = 0.1f; }
        anglePerSecond = (360 - initialRotation.x) / timeToPeak;
        SetActive(true);
    }

    bool active = false;

    // kinematic variables
    float age = 0.0f;
    const float gravity = -9.8f;

    Vector3 worldVelocity = new Vector3(0, 0, 0);
    Vector3 worldAcceleration = new Vector3(0, -19.6f, 0);

    Vector3 localVelocity = new Vector3(0, 0, 50);
    Vector3 localAcceleration = new Vector3(0, 0, -0.005f);

    public float GetAge() { return age; }
    public bool GetActive() { return active; }
    public Transform GetLaunchTransform() { return launchTransform; }
    public Vector3 GetLaunchLocalVelocity() { return launchLocalVelocity; }

    public void SetActive(bool _active) { active = _active; }

    void Update()
    {
        if (active)
        {
            age += Time.deltaTime;

            transform.position = PositionAtAge(age);
            //transform.rotation.eulerAngles.Set(anglePerSecond * age, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            body.Rotate(anglePerSecond * Time.deltaTime, 0, 0, Space.Self);

            //localVelocity += localAcceleration * age * Time.deltaTime;
            //worldVelocity += worldAcceleration * age * Time.deltaTime;

            //if (worldVelocity.y < gravity)
            //{
            //    worldVelocity.y = gravity;
            //}
            //if (localVelocity.z < 0)
            //{
            //    localVelocity.z = 0;
            //}

            //transform.Translate(localVelocity * Time.deltaTime, Space.Self);
            //transform.Translate(worldVelocity * Time.deltaTime, Space.World);

            //if (!peaked && previousHeight > transform.position.y)
            //{
            //    peaked = true;
            //}
            //previousHeight = transform.position.y;
        }
    }

    public Vector3 PositionAtAge(float _age)
    {
        Vector3 newLocation = initialWorldPosition;

        Vector3 localDisplacement = launchLocalVelocity * _age + 0.5f * localAcceleration * (_age * _age);
        Vector3 worldDisplacement = new Vector3(0, 0, 0) * _age + 0.5f * worldAcceleration * (_age * _age);

        localDisplacement = transform.localToWorldMatrix.MultiplyVector(localDisplacement);

        newLocation += localDisplacement + worldDisplacement;
        return newLocation;
    }

    public Vector4 HighestPointWithinAge(float _age)
    {
        Vector4 highestPoint = new Vector4(0.0f, -1.0f, 0.0f, 0.0f);

        Vector3 newPoint;
        for (float localAge = 0.0f; localAge < _age; localAge += 0.03f)
        {
            newPoint = PositionAtAge(localAge);
            if (newPoint.y > highestPoint.y)
            {
                highestPoint = new Vector4(newPoint.x, newPoint.y, newPoint.z, localAge);
            }
        }

        return highestPoint;
    }


    //public Transform GetPositionAtTime(Transform _initialTransform, Vector3 _initialLocalVelocity, float _timePassed)
    //{
    //    GameObject scout = new GameObject();

    //    scout.transform.position = _initialTransform.position;
    //    scout.transform.rotation = _initialTransform.rotation;
    //    scout.transform.localScale = _initialTransform.localScale;

    //    bool _peaked = false;
    //    float _previousHeight = -1;

    //    Vector3 _worldVelocity = new Vector3(0, 0, 0);
    //    Vector3 _worldAcceleration = new Vector3(0, -19.6f, 0);

    //    Vector3 _localVelocity = _initialLocalVelocity;
    //    Vector3 _localAcceleration = new Vector3(0, 0, -0.5f);

    //    float _age = 0.0f;

    //    while (_age < _timePassed)
    //    {
    //        _age += 0.03f;
    //        _localVelocity += _localAcceleration * _age * 0.03f;
    //        _worldVelocity += _worldAcceleration * _age * 0.03f;

    //        if (_worldVelocity.y < gravity)
    //        {
    //            _worldVelocity.y = gravity;
    //        }
    //        if (_localVelocity.z < 0)
    //        {
    //            _localVelocity.z = 0;
    //        }

    //        scout.transform.Translate(_localVelocity * 0.03f, Space.Self);
    //        scout.transform.Translate(_worldVelocity * 0.03f, Space.World);

    //        if (!_peaked && _previousHeight > scout.transform.position.y)
    //        {
    //            _peaked = true;
    //        }
    //        _previousHeight = scout.transform.position.y;
    //    }

    //    return scout.transform;
    //}
}
