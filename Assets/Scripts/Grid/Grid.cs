using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grid
{
    private int _width;
    private int _height;
    private float _cellSize;
    private int[,] _gridArray;
    private Vector3 _originPosition;


    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridArray = new int[width, height];
        _originPosition = originPosition;

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, Mathf.Infinity);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, Mathf.Infinity);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, Mathf.Infinity);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, Mathf.Infinity);

    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x - _originPosition.x / _cellSize);
        y = Mathf.FloorToInt(worldPosition.y - _originPosition.y / _cellSize);
    }

    //this probably will be change to something that represents object build on certain cell
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;
        }
    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return _gridArray[x, y];
        }

        return -1;
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
