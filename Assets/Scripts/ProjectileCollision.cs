using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public GameObject visualObject;

    ProjectilePhysics physicsComponent;

    bool beenAHit;
    bool active = false;

    public void MakeActive()
    {
        physicsComponent = GetComponent<ProjectilePhysics>();
        active = true;
    }

    const float timestep = 0.0003f;
    void Update()
    {
        if (!beenAHit && active)
        {
            float checkAge = physicsComponent.GetAge() - Time.deltaTime;
            while (checkAge < physicsComponent.GetAge())
            {
                Vector3 checkPos = physicsComponent.PositionAtAge(checkAge);
                Quaternion checkRot = physicsComponent.RotationAtAge(checkAge);

                beenAHit = Physics.CheckBox(checkPos, visualObject.transform.localScale * 0.5f, checkRot);

                if (beenAHit)
                {
                    transform.position = checkPos;

                    Destroy(physicsComponent);
                    Destroy(this);
                    break;
                }

                checkAge += timestep;
            }
        }
    }
}
