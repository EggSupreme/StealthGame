using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class Tool
{
    public Tool(int _id, string _name, Vector3 _restingOffset, Vector3 _aimingOffset)
    {
        id = _id;
        name = _name;
        restingHandOffset = _restingOffset;
        aimingHandOffset = _aimingOffset;
    }

    protected int id = -1;
    public string name = "Tool";

    protected GameObject instancedObject = null;
    protected Vector3 restingHandOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public bool aiming;
    protected Vector3 aimingHandOffset = new Vector3(0, 0, 1.0f);

    public virtual void OnStart() { }
    public void SetInstancedObjectParent(GameObject _newParent)
    {
        instancedObject.transform.parent = _newParent.transform;
        instancedObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (aiming)
        {
            instancedObject.transform.localPosition = aimingHandOffset;
        }
        else
        {
            instancedObject.transform.localPosition = restingHandOffset;
        }
    }

    public virtual void Aiming(bool _isAiming) { aiming = _isAiming; }
    public virtual bool OnLeftClickDown() { return false; }
    public virtual bool WhileLeftClickHeld() { return false; }
    public virtual bool OnLeftClickReleased() { return false; }


    public virtual void BringOut(GameObject _hand, GameObject _prefab, GameObject _projectilePrefab = null)
    {
        instancedObject = Object.Instantiate(_prefab, _hand.transform);
    }

    public void PutAway() { Object.Destroy(instancedObject); }
}

class ToolWithProjectile : Tool
{
    public ToolWithProjectile(int _id, string _name, Vector3 _restingOffset, Vector3 _aimingOffset, Vector3 _projectileOffset, int _maxAmmoCount) : base(_id, _name, _restingOffset, _aimingOffset)
    { projectileOffset = _projectileOffset; maxAmmoCount = _maxAmmoCount; }

    protected GameObject projectilePrefab = null;
    protected GameObject currentProjectile = null;

    protected Vector3 projectileOffset;

    protected int maxAmmoCount = 5;
    protected int ammoCount = 0;

    public override void OnStart()
    {
        UpdateAmmo(maxAmmoCount);
    }

    public void UpdateAmmo(int _count) { ammoCount += _count; Tools_Abilities.GetToolVisual(id).GetComponentInChildren<Text>().text = ammoCount.ToString(); }

    public override void BringOut(GameObject _hand, GameObject _prefab, GameObject _projectilePrefab = null)
    {
        instancedObject = Object.Instantiate(_prefab, _hand.transform);
        instancedObject.transform.localPosition = Vector3.zero;
        instancedObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        projectilePrefab = _projectilePrefab;
    }
}

class ToolWithChargedProjectile : ToolWithProjectile
{
    protected bool interrupted = false;
    protected float maxChargeTime;
    protected float chargeTime = 0.0f;

    public ToolWithChargedProjectile(int _id, string _name, Vector3 _restingOffset, Vector3 _aimingOffset, Vector3 _projectileOffset, int _maxAmmoCount, float _maxChargeTime) : base(_id, _name, _restingOffset, _aimingOffset, _projectileOffset, _maxAmmoCount)
    { maxChargeTime = _maxChargeTime; }

    public override void Aiming(bool _isAiming)
    {
        if (aiming && !_isAiming)
        {
            Object.Destroy(currentProjectile);
            currentProjectile = null;
            chargeTime = 0.0f;
            interrupted = true;
        }

        base.Aiming(_isAiming);
    }

    public override bool OnLeftClickDown()
    {
        if (ammoCount <= 0) { Debug.Log("Out of ammo"); return false; }
        interrupted = false;

        currentProjectile = Object.Instantiate(projectilePrefab, instancedObject.transform.position, instancedObject.transform.rotation);
        currentProjectile.transform.parent = instancedObject.transform;
        currentProjectile.transform.Translate(projectileOffset);
        chargeTime = 0.0f;

        return true;
    }
    public override bool WhileLeftClickHeld()
    {
        if (ammoCount <= 0 || interrupted) { return false; }

        if (chargeTime + Time.deltaTime <= maxChargeTime)
        { chargeTime += Time.deltaTime; }

        return true;
    }
    public override bool OnLeftClickReleased()
    {
        if (ammoCount <= 0 || interrupted) { return false; }

        currentProjectile.transform.parent = null;
        UpdateAmmo(-1);

        return true;
    }
}

class Bow : ToolWithChargedProjectile
{
    public Bow() : base(0, "Bow", new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.1f, 0.5f, -0.5f), new Vector3(0.0f, -0.04f, 0.8f), 15, 5.0f) { }

    public override bool OnLeftClickDown()
    {
        if (!base.OnLeftClickDown()) { return false; }

        AudioPlayer.PlaySfxClip(SoundClip.bowPull);
        //instancedObject.GetComponent<BowController>().bowString.transform.Translate(0, 0, -0.05f);
        return true;
    }
    public override bool WhileLeftClickHeld()
    {
        if (!base.WhileLeftClickHeld()) { return false; }

        return true;
    }
    public override bool OnLeftClickReleased()
    {
        if (!base.OnLeftClickReleased()) { return false; }

        currentProjectile.GetComponent<ProjectilePhysics>().Launch(Mathf.Clamp(chargeTime * 10, 1.5f, 50.0f), 1);
        currentProjectile = null;

        AudioPlayer.PlaySfxClip(SoundClip.arrowRelease);
        //instancedObject.GetComponent<BowController>().bowString.transform.Translate(0, 0, 0.05f);
        return true;
    }
}

class Dagger : Tool
{
    public Dagger() : base(1, "Dagger", Vector3.zero, new Vector3(0.4f, 0.2f, 0.3f)) { }

    public override bool OnLeftClickDown()
    {
        Vector3 checkPos = instancedObject.transform.position + (instancedObject.transform.right * -0.5f) + (instancedObject.transform.forward * 0.2f);
        //Collider[] collidersHit = Physics.OverlapBox(checkPos, new Vector3(0.5f, 0.5f, 1.0f), instancedObject.transform.rotation, ~(1 << 8), QueryTriggerInteraction.Collide);
        Collider[] collidersHit = Physics.OverlapSphere(checkPos, 0.8f, ~(1 << 8), QueryTriggerInteraction.Collide);
        for (int c = 0; c < collidersHit.Length; c++)
        {
            if (collidersHit[c].CompareTag("Enemy"))
            {
                AudioPlayer.PlaySfxClip(SoundClip.daggerHit);
                //collidersHit[c].GetComponentInParent<EnemyController>().Damage(-1);
            }
        }
        return true;
    }
}

class BlowPipe : ToolWithProjectile
{
    public BlowPipe() : base(2, "Blowpipe", new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.4f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), 5) { }

    public override bool OnLeftClickDown()
    {
        if (ammoCount <= 0) { Debug.Log("Out of darts"); return false; }

        currentProjectile = Object.Instantiate(projectilePrefab, instancedObject.transform.position, instancedObject.transform.rotation);
        currentProjectile.transform.parent = instancedObject.transform;
        currentProjectile.transform.Translate(projectileOffset);

        currentProjectile.transform.parent = null;
        currentProjectile.GetComponent<ProjectilePhysics>().Launch(40.0f, 0.5f);
        currentProjectile = null;
        UpdateAmmo(-1);

        return true;
    }
}

class RockBag : ToolWithProjectile
{
    public RockBag() : base(3, "Rock Bag", Vector3.zero, Vector3.zero, Vector3.zero, 10) { }
}

public class Tools_Abilities : MonoBehaviour
{
    const int toolCount = 5;
    
    public GameObject[] toolVisuals = new GameObject[toolCount];
    static GameObject[] staticToolVisuals = new GameObject[toolCount];

    public GameObject restingHand = null;
    public GameObject targetingHand = null;
    public GameObject[] toolPrefabs = new GameObject[toolCount];
    public GameObject[] projectilePrefabs = new GameObject[toolCount];

    int currentToolID = -1;
    public int CurrentToolID { get { return currentToolID; } }
    Tool[] tools = new Tool[toolCount] { new Bow(), new Dagger(), new BlowPipe(), null, null };

    public static GameObject GetToolVisual(int _id) { return staticToolVisuals[_id]; }

    // Start is called before the first frame update
    void Start()
    {
        SwapToTool(0);
        staticToolVisuals = toolVisuals;
        for (int i = 0; i < toolCount; i++) { if (tools[i] != null) { tools[i].OnStart(); } }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            tools[currentToolID].Aiming(true);
            tools[currentToolID].SetInstancedObjectParent(targetingHand);

            if (Input.GetMouseButtonDown(0)) { tools[currentToolID].OnLeftClickDown(); }

            if (Input.GetMouseButton(0)) { tools[currentToolID].WhileLeftClickHeld(); }

            if (Input.GetMouseButtonUp(0)) { tools[currentToolID].OnLeftClickReleased(); }
        }
        else
        {
            tools[currentToolID].Aiming(false);
            tools[currentToolID].SetInstancedObjectParent(restingHand);
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
        if (currentToolID != -1) { tools[currentToolID].PutAway(); } // if there's a previous tool, put it away
        if (tools[_newToolID] == null) { _newToolID = 0; Debug.Log("No tools there, getting bow instead"); } // if an invalid tool is requested, request bow instead

        currentToolID = _newToolID;
        tools[currentToolID].BringOut(restingHand, toolPrefabs[currentToolID], projectilePrefabs[currentToolID]);
    }

    public void CollectAmmo(AmmoType _type, int _amount)
    {
        int toolID = -1;
        switch (_type)
        {
            case AmmoType.arrow:
                toolID = 0;
                break;
            case AmmoType.sleepDart:
                toolID = 2;
                break;
        }
        ((ToolWithProjectile)tools[toolID]).UpdateAmmo(_amount);
    }
    public void CollectAmmo(int[] _ammo)
    {
        for(int i = 0; i < _ammo.Length; i++)
        {
            if (_ammo[i] > 0)
            {
                CollectAmmo((AmmoType)i, _ammo[i]);
            }
        }
    }
}