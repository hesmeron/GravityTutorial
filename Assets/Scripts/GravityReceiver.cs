using UnityEngine;

public class GravityReceiver : MonoBehaviour
{
    [SerializeField] 
    private GravityField _gravityField;
    [SerializeField] 
    private Rigidbody _rigidbody;

    private void OnDrawGizmosSelected()
    {
        if (_gravityField)
        {
            Vector3 acceleration = _gravityField.GetAccelerationAtPosition(transform.position);
            Gizmos.DrawLine(transform.position, transform.position + acceleration);
        }
    }

    private void Update()
    {
        _rigidbody.velocity += (Vector3) _gravityField.GetAccelerationAtPosition(transform.position) * Time.deltaTime;
    }
    
}
