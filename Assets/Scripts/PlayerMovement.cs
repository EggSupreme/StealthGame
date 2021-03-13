using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject arr;
    public GameObject rockPrefab;
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
            hand.transform.Rotate(new Vector3(15 * Time.deltaTime, 0, 0));
        }
        else if (Input.GetKey(KeyCode.E)) // rotate hand upwards
        {
            hand.transform.Rotate(new Vector3(-15 * Time.deltaTime, 0, 0));
        }

        if (Input.GetMouseButtonDown(0)) // throw rock
        {
            GameObject rock = Instantiate(rockPrefab);
            rock.transform.parent = transform;
            rock.transform.localPosition = new Vector3(0,1.5f,0);
            rock.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(-45, 0, 0));
            rock.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 1000);
        }
        if (Input.GetMouseButtonDown(1)) // proj
        {
            
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