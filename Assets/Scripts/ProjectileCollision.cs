using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public ProjectilePhysics mainObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PerformChecks(float _currentAge, float _deltaTime, int _checksToPerform)
    {
        transform.position = mainObject.PositionAtAge(_currentAge);
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision with " + other.gameObject.name);
        if (!other.gameObject.CompareTag("ProjectileIgnore"))
        {
            //mainObject.transform.position = other..GetContact(0).point;
            Destroy(mainObject);
            Debug.Log("collision");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision with " + collision.gameObject.name);
        if (!collision.gameObject.CompareTag("ProjectileIgnore"))
        {
            mainObject.transform.position = collision.GetContact(0).point;
            Destroy(mainObject);
            Debug.Log("collision");
        }
        
    }
}
