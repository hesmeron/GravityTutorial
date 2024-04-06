using UnityEngine;

public class Table2D : MonoBehaviour
{
    [SerializeField] 
    private Vector2[] _serializedAccelerationTable;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    private Vector2[,] _accelerationTable;
}
