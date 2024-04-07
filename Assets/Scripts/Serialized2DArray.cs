using UnityEngine;

[System.Serializable]
public class Serialized2DArray<T> : ISerializationCallbackReceiver
{
    [SerializeField] 
    private T[] _serializedArray;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    private T[,] _array;

    public int Width => _width;

    public int Height => _height;

    public Serialized2DArray(int width, int height)
    {
        _width = width;
        _height = height;
        _array = new T[width, height];
    }

    public void OnBeforeSerialize()
    {
        _serializedArray = new T[_width * _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _serializedArray[(_height * x) + y] = _array[x,y];
            }
        }
    }
        
    public void OnAfterDeserialize()
    {
        _array = new T[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _array[x, y] = _serializedArray[(_height * x) + y];
            }
        }
    }
    public T this[int x, int y]
    {
        get => GetValue(x,y);
        set => SetValue(x, y, value);
    }

    private void SetValue(int x, int y, T value)
    {
        _array[x, y] = value;
    }
    
    private T GetValue(int x, int y)
    {
        return _array[x, y];
    }
}
