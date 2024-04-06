using UnityEngine;

public class GravitySource : MonoBehaviour
{
    [SerializeField] 
    private float _mass;

    public Vector2 GetAccelerationAtPosition(Vector3 position)
    {
        Vector2 direction= transform.position - position;
        float distance = direction.magnitude;
        float acceleration = _mass / (distance * distance);
        return direction.normalized * acceleration;
    }
}
