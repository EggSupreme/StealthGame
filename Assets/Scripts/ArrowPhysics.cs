using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPhysics : MonoBehaviour
{
    // consts
    public GameObject physicalComponents = null;
    float maxVelocity = 1f;
    const float gravity = -10f;
    const float airResistance = -1f;
    const float tipDropVelocity = 5f;
    float tipDropRate = 5f;

    Quaternion initialRotation;

    // variables
    bool active = false;
    public float velocity = 1f;
    float age = 0f;

    float force = 1f;

    bool topOfArc = false;

    public void Activate()
    {
        active = true;
        transform.parent = null;
    }

    float a, b, c;
    public void QuadStart(float _power, float _angle, float _height)
    {
        a = _power;
        b = _angle * (1 - (a * -5));
        c = _height;
    }

    public void Initialise(Transform _parent, Vector3 _positionOffset, float _force)
    {
        transform.SetParent(_parent);
        transform.position = _parent.position + _positionOffset;
        transform.rotation = _parent.rotation;
        velocity = 50;
        maxVelocity = velocity;
        tipDropRate = 30; // _force
        initialRotation = _parent.rotation;
        force = 100 - _force;
    }
    public void Initialise(Vector3 _position, Quaternion _rotation, float _force)
    {
        //transform.SetParent();
        transform.position = _position;
        transform.localRotation = _rotation;
        velocity = 50;
        maxVelocity = velocity;
        tipDropRate = 30; // _force
        initialRotation = _rotation;
        force = 100 - _force;
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        // age
        age += Time.deltaTime;
        if (age > 50)
        {
            Destroy(gameObject);
        }

        float x = age * 15;
        transform.position = new Vector3(transform.position.x, ((a * x * x)) + (b * x) + c, x);

        //// rotation
        //float angle = (transform.rotation * Quaternion.Inverse(initialRotation)).x * 100;
        //Debug.Log(angle);

        //if (angle > 0)
        //{
        //    Debug.Log("Arc peak reached");
        //    topOfArc = true;
        //}

        //if (angle < 80)
        //{
        //    float xRotation = tipDropRate * 0.6f * Time.deltaTime; //tipDropRate * (maxVelocity - velocity) * Time.deltaTime;
        //    transform.Rotate(new Vector3(xRotation, 0, 0), Space.Self);
        //}
        //else
        //{
        //    Debug.Log("STOP ROTATING");

        //    //Destroy(this);
        //}
        
        //// movement
        //Vector3 movement = new Vector3(0, 0, velocity);
        //transform.Translate(movement * Time.deltaTime, Space.Self);

        //int dir = -1;
        //if (velocity > 0) 
        //{
        //    if (angle > 0)
        //    {
        //        //dir = 1;
        //    }
        //    velocity += maxVelocity * 0.0001f * force * dir;
        //}
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with trigger " + other.gameObject.name);
    }
}
