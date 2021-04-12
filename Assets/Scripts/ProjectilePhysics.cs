using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    public GameObject body;

    Vector3 initialWorldPosition;
    Quaternion initialRotation;

    Vector3 initialLocalVelocity;

    bool active = false;
    float age = 0.0f;

    Vector3 worldAcceleration = new Vector3(0, -19.6f, 0);
    Vector3 localAcceleration = new Vector3(0, 0, -0.005f);


    public float GetAge() { return age; }

    public void SetActive(bool _active)
    {
        active = _active;

        ProjectileCollision collisionComponent = GetComponent<ProjectileCollision>();
        if (collisionComponent) { collisionComponent.MakeActive(); }
    }



    public void Launch(float _launchPower, float _heaviness)
    {
        initialLocalVelocity = new Vector3(0.0f, 0.0f, _launchPower * 2);

        initialWorldPosition = transform.position;
        initialRotation = body.transform.rotation;

        SetActive(true);
    }

    void Update()
    {
        if (active)
        {
            age += Time.deltaTime;

            transform.position = PositionAtAge(age);
            body.transform.rotation = RotationAtAge(age);
        }
    }

    public Vector3 PositionAtAge(float _age)
    {
        if (age <= 0) { return initialWorldPosition; }

        Vector3 newLocation = initialWorldPosition;

        Vector3 localDisplacement = initialLocalVelocity * _age + 0.5f * localAcceleration * (_age * _age);
        Vector3 worldDisplacement = new Vector3(0, 0, 0) * _age + 0.5f * worldAcceleration * (_age * _age);

        localDisplacement = transform.localToWorldMatrix.MultiplyVector(localDisplacement);

        newLocation += localDisplacement + worldDisplacement;
        return newLocation;
    }

    const float rotationCheckRate = 0.03f;
    public Quaternion RotationAtAge(float _age)
    {
        if (age - rotationCheckRate <= 0) { return initialRotation; }

        Vector3 currentPos = PositionAtAge(_age);
        Vector3 previousPos = PositionAtAge(_age - rotationCheckRate);

        Vector3 dir = (currentPos - previousPos).normalized;

        return Quaternion.LookRotation(dir) * Quaternion.Euler(90, 0, 0);
    }
}
