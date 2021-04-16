using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public GameObject visualObject;
    public AmmoType ammoType;

    ProjectilePhysics physicsComponent;

    bool beenAHit = false;
    bool isActive = false;

    const float decayTime = 10.0f;
    float decayAge = 0.0f;

    public void MakeActive()
    {
        physicsComponent = GetComponent<ProjectilePhysics>();
        isActive = true;
    }

    const float timestep = 0.0003f;
    void Update()
    {
        if (!beenAHit && isActive)
        {
            float checkAge = physicsComponent.GetAge() - Time.deltaTime;
            while (checkAge < physicsComponent.GetAge() && !beenAHit)
            {
                Vector3 checkPos = physicsComponent.PositionAtAge(checkAge);
                Quaternion checkRot = physicsComponent.RotationAtAge(checkAge);

                Collider[] hits = Physics.OverlapBox(checkPos, visualObject.transform.localScale * 0.5f, checkRot, ~(1 << 8), QueryTriggerInteraction.Collide);

                if (hits.Length > 0)
                {
                    beenAHit = true;

                    for (int c = 0; c < hits.Length; c++)
                    {
                        if (hits[c].transform.parent != null &&  hits[c].transform.parent.TryGetComponent(out Transform parentTransform))
                        {
                            transform.parent = parentTransform;
                        }
                        else
                        {
                            transform.parent = hits[c].transform;
                        }
                        
                    }

                    transform.position = checkPos;
                    Destroy(physicsComponent);
                    visualObject.GetComponent<BoxCollider>().enabled = true;

                    if (ammoType == AmmoType.sleepDart)
                    {
                        decayAge = decayTime + 1;
                    }
                    break;
                }

                checkAge += timestep;
            }
        }
        else if (beenAHit)
        {
            decayAge += Time.deltaTime;

            if (decayAge >= decayTime)
            {
                AmmoStation.Store(ammoType, 1);
                Destroy(gameObject);
            }
        }
    }
}
