using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    Vector3 arrowSpawnPoint = new Vector3(0, 0.5f, 0);
    public GameObject arrowPrefab = null;
    public GameObject markerPrefab = null;

    public GameObject bowString = null;

    GameObject currentArrow = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // user readies an arrow
        {
            

            currentArrow = Instantiate(arrowPrefab);
            //currentArrow.GetComponent<ArrowPhysics>().QuadStart(-0.05f, 0, 1.5f);
            currentArrow.GetComponent<ArrowPhysics>().QuadStart(-0.005f, 5, 1.5f);
            //currentArrow.GetComponent<ArrowPhysics>().Initialise(transform, arrowSpawnPoint, 99);//transform.position + arrowSpawnPoint, GetComponentInParent<Transform>().rotation, 99);

            bowString.transform.Translate(0, 0, -0.05f);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //GameObject marker = Instantiate(markerPrefab);
            //marker.GetComponent<ArrowPhysics>().Initialise(transform, arrowSpawnPoint, 99);
            //marker.GetComponent<ArrowPhysics>().Activate();
        }
        if (Input.GetKeyUp(KeyCode.Space)) // user releases the arrow
        {
            currentArrow.GetComponent<ArrowPhysics>().Activate();
            bowString.transform.Translate(0, 0, 0.05f);
        }
    }
}
