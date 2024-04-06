using UnityEngine;

[System.Serializable]
public class Serialized2DArray : ISerializationCallbackReceiver
{
    [SerializeField] 
    private Vector2[] _serializedAccelerationTable;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    private Vector2[,] _accelerationTable;

    public int Width => _width;

    public int Height => _height;

    public Vector2[,] AccelerationTable => _accelerationTable;

    public Serialized2DArray(int width, int height)
    {
        _width = width;
        _height = height;
        _accelerationTable = new Vector2[width, height];
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

    public void SetValue(int x, int y, Vector2 value)
    {
        _accelerationTable[x, y] = value;
    }
    
    public Vector2 GetValue(int x, int y)
    {
        return _accelerationTable[x, y];
    }
}
