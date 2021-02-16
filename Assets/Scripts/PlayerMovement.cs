using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject hand = null;

    const float diagonalPenalty = 0.7f;
    const float movementSpeed = 500;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) // rotate hand downwards
        {
            hand.transform.Rotate(new Vector3(5 * Time.deltaTime, 0, 0));
        }
        else if (Input.GetKey(KeyCode.E)) // rotate hand upwards
        {
            hand.transform.Rotate(new Vector3(-5 * Time.deltaTime, 0, 0));
        }

        float h_axis = Input.GetAxis("Horizontal");
        float v_axis = Input.GetAxis("Vertical");

        float tempMovementSpeed = movementSpeed;
        if (h_axis != 0 && v_axis != 0)
        {
            tempMovementSpeed *= diagonalPenalty;
        }

        h_axis *= tempMovementSpeed * Time.deltaTime;
        v_axis *= tempMovementSpeed * Time.deltaTime;
        rb.AddForce(new Vector3(h_axis, 0, v_axis));
    }
}