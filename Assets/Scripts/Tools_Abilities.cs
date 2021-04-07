using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Tool
{
    public Tool(int _id, string _name)
    {
        id = _id;
        name = _name;
    }

    protected int id = -1;
    public string name = "Tool";

    protected GameObject instancedObject = null;
    protected Vector3 restingHandOffset = new Vector3(0, 0, 0);

    public bool aiming;
    protected Vector3 targetingHandOffset = new Vector3(0, 0, 1);

    public void SetInstancedObjectParent(GameObject _newParent)
    {
        instancedObject.transform.parent = _newParent.transform;
        instancedObject.transform.rotation = _newParent.transform.rotation;

        if (aiming)
        {
            instancedObject.transform.localPosition = targetingHandOffset;
        }
        else
        {
            instancedObject.transform.localPosition = restingHandOffset;
        }
    }

    public virtual void OnLeftClickDown() { }
    public virtual void WhileLeftClickHeld() { }
    public virtual void OnLeftClickReleased() { }


    public virtual void BringOut(GameObject _hand, GameObject _prefab, GameObject _projectilePrefab = null)
    {
        instancedObject = Object.Instantiate(_prefab, _hand.transform);
    }

    public void PutAway() { Object.Destroy(instancedObject); }
}

class ToolWithProjectile : Tool
{
    public ToolWithProjectile(int _id, string _name) : base(_id, _name) { }

    protected GameObject projectilePrefab = null;
    protected GameObject currentProjectile = null;

    public override void BringOut(GameObject _hand, GameObject _prefab, GameObject _projectilePrefab = null)
    {
        instancedObject = Object.Instantiate(_prefab, _hand.transform);
        projectilePrefab = _projectilePrefab;
    }
}

class Bow : ToolWithProjectile
{
    public Bow() : base(0, "Bow") { }

    protected float pullTime = 0.0f;

    public override void OnLeftClickDown()
    {
        currentProjectile = Object.Instantiate(projectilePrefab, instancedObject.transform.position + new Vector3(0, 0.5f, 0), instancedObject.transform.rotation);
        currentProjectile.transform.parent = instancedObject.transform;
        pullTime = 0.0f;

        instancedObject.GetComponent<BowController>().bowString.transform.Translate(0, 0, -0.05f);
    }
    public override void WhileLeftClickHeld()
    {
        pullTime += Time.deltaTime;
    }
    public override void OnLeftClickReleased()
    {
        currentProjectile.transform.parent = null;
        currentProjectile.GetComponent<ProjectilePhysics>().Launch(Mathf.Clamp(pullTime * 5, 1.5f, 50.0f), 1);
        currentProjectile = null;

        instancedObject.GetComponent<BowController>().bowString.transform.Translate(0, 0, 0.05f);
    }
}

class BlowPipe : ToolWithProjectile
{
    public BlowPipe() : base(2, "Blowpipe") { }

    public override void OnLeftClickDown()
    {
        currentProjectile = Object.Instantiate(projectilePrefab, instancedObject.transform.position, instancedObject.transform.rotation);
        currentProjectile.transform.parent = instancedObject.transform;
    }
    public override void WhileLeftClickHeld()
    {
        
    }
    public override void OnLeftClickReleased()
    {
        currentProjectile.transform.parent = null;
        currentProjectile.GetComponent<ProjectilePhysics>().Launch(40.0f, 0.5f);
        currentProjectile = null;
    }
}

public class Tools_Abilities : MonoBehaviour
{
    const int toolCount = 5;

    public GameObject restingHand = null;
    public GameObject targetingHand = null;
    public GameObject[] toolPrefabs = new GameObject[toolCount];
    public GameObject[] projectilePrefabs = new GameObject[toolCount];

    int currentToolID = -1;
    Tool[] tools = new Tool[toolCount] { new Bow(), null, new BlowPipe(), null, null };

    // Start is called before the first frame update
    void Start()
    {
        SwapToTool(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            tools[currentToolID].aiming = true;
            tools[currentToolID].SetInstancedObjectParent(targetingHand);
            //hand.transform.rotation = Camera.main.transform.rotation;

            if (Input.GetMouseButtonDown(0)) { tools[currentToolID].OnLeftClickDown(); }

            if (Input.GetMouseButton(0)) { tools[currentToolID].WhileLeftClickHeld(); }

            if (Input.GetMouseButtonUp(0)) { tools[currentToolID].OnLeftClickReleased(); }
        }
        else
        {
            tools[currentToolID].aiming = false;
            tools[currentToolID].SetInstancedObjectParent(restingHand);
            //hand.transform.rotation = Quaternion.Euler(45, hand.transform.rotation.eulerAngles.y, hand.transform.rotation.eulerAngles.z);
        }

        for (int i = 0; i < toolCount; i++)
        {
            if (Input.GetKeyDown((KeyCode)49 + i))
            {
                SwapToTool(i);
                break;
            }
        }
    }

    void SwapToTool(int _newToolID)
    {
        if (currentToolID != -1)
        {
            tools[currentToolID].PutAway();
        }

        currentToolID = _newToolID;
        tools[currentToolID].BringOut(restingHand, toolPrefabs[currentToolID], projectilePrefabs[currentToolID]);
    }
}
