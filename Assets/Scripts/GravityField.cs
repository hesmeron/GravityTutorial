using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GravityField : MonoBehaviour
{
    [SerializeField] 
    private float _gravityCoefficient = 1f;
    [SerializeField]
    private List<GravitySource> _gravitySources; 
    [SerializeField]
    private Vector2 _extent;
    [SerializeField]
    private float _cellSize = 0.25f;
    [SerializeField] 
    private Vector2[] _serializedAccelerationTable;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    private Vector2[,] _accelerationTable;

    public Vector2 Extent
    {
        get => _extent;
        set => _extent = value;
    }

    private void OnDrawGizmosSelected()
    {
        if (_accelerationTable == null)
        {
            CalculateAcceleration();
        }


        float drawingStep = _cellSize/3f;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 position = PositionFromCoordinates(x,y);
                Vector2 acceleration = GetAccelerationAtPosition(position + transform.position) / 50f;
                acceleration = acceleration.normalized * MathF.Min(acceleration.magnitude, _cellSize/2f);
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(position, drawingStep/10f);
                DrawArrow(position, acceleration);
                for (float xi = 0; xi < _cellSize; xi += drawingStep)
                {
                    for (float yi = 0; yi < _cellSize; yi += drawingStep)
                    {
                        Vector3 positionPrime = (Vector2) position + new Vector2(xi,yi);
                        Vector2 accelerationPrime = GetAccelerationAtPosition(positionPrime + transform.position) / 50f;
                        accelerationPrime = accelerationPrime.normalized * MathF.Min(acceleration.magnitude, drawingStep/2f);
                        DrawArrow(positionPrime, accelerationPrime);
                    }
                }
            }
        }
    }

    public void OnBeforeSerialize()
    {
        _serializedAccelerationTable = new Vector2[_width * _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _serializedAccelerationTable[(_height * x) + y] = _accelerationTable[x,y];
            }
        }
    }
        
    public void OnAfterDeserialize()
    {
        _accelerationTable = new Vector2[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _accelerationTable[x, y] = _serializedAccelerationTable[(_height * x) + y];
            }
        }
    }
    
    private static void DrawArrow(Vector3 position, Vector3 direction)
    {
        Color color = new Color(Mathf.Sign(direction.x), Mathf.Sign(direction.y),Mathf.Sign(direction.z), 1);
        DrawArrow(position, direction, color);
        
    }

    private static void DrawArrow(Vector3 position, Vector3 direction, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(position, position + direction);
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized * (direction.magnitude/2);
        Vector3 sidesOrign = position + (direction / 2f);
        Gizmos.DrawLine(sidesOrign + perpendicular, position + direction);
        Gizmos.DrawLine(sidesOrign - perpendicular, position + direction);
    }


    private void Awake()
    {
        CalculateAcceleration();
    }

    public void CalculateAcceleration()
    {
        _width = Mathf.CeilToInt(_extent.x / _cellSize) +1;
        _height = Mathf.CeilToInt(_extent.y / _cellSize) +1;
        
        _accelerationTable = new Vector2[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 position = PositionFromCoordinates(x, y);
                _accelerationTable[x, y] = CalculateAccelerationAtPosition(position);
            }
        }
    }
    
    public Vector2 GetAccelerationAtPosition(Vector3 position)
    {
        Vector2 localPosition = (Vector2) position - (Vector2) transform.position + (_extent/2f);
        float xFull = localPosition.x / _cellSize;
        float yFull = localPosition.y / _cellSize;
        int baseXIndex = Mathf.Clamp(Mathf.FloorToInt(xFull),0, _width-1);
        int baseYIndex = Mathf.Clamp(Mathf.FloorToInt(yFull), 0, _height-1);
        float xRest = xFull - baseXIndex;
        float yRest = yFull - baseYIndex;
        int otherX = Mathf.Clamp(baseXIndex + (int) Mathf.Sign(xRest), 0, _width-1);
        int otherY = Mathf.Clamp(baseYIndex + (int)Mathf.Sign(yRest), 0, _height - 1);
            
        Vector2 forceD = _accelerationTable[baseXIndex, baseYIndex];
        Vector2 forceC = _accelerationTable[otherX, baseYIndex];
        Vector2 forceB = _accelerationTable[otherX, otherY];
        Vector2 forceA = _accelerationTable[baseXIndex, otherY];
        Vector3 force = Blerp(forceA, forceB, forceD, forceC, xRest, yRest);

        return  force;
    }
    
    private Vector2 CalculateAccelerationAtPosition(Vector3 position)
    {
        Vector2 acceleration = Vector2.zero;
        foreach (GravitySource gravitySource in _gravitySources)
        {
            acceleration += gravitySource.GetAccelerationAtPosition(position) * _gravityCoefficient;
        }
        return acceleration;
    }  

    private Vector3 PositionFromCoordinates(int x, int y)
    {
        Vector3 localPosition = new Vector2(x * _cellSize, y * _cellSize) - (_extent / 2f);
        return localPosition + transform.position;
    }

    private Vector2 Blerp(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float x, float y)
    {
        Vector2 abx = Vector2.Lerp(a, b, x);
        Vector2 cdx = Vector2.Lerp(c, d, x);
        return Vector2.Lerp(cdx, abx, y);
    }
}
