using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private const float _tileSizeWidth = 180;
    private const float _tileSizeHeight = 180;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    private bool[,] _gridArrayGridInUse;

    private Vector2 _positionOnTheGrid = new Vector2();
    private Vector2Int _tileGridPosition = new Vector2Int();

    private void Start()
    {
        _gridArrayGridInUse = new bool[_width,_height];
    }

    public Vector2Int GetTileInGridPosition(Vector2 inputPosition)
    {
        _positionOnTheGrid.x = inputPosition.x;
        _positionOnTheGrid.y = inputPosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / _tileSizeWidth );
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / _tileSizeHeight);

        return _tileGridPosition;
    }
    public Vector2Int GetTileInGridPosition(RectTransform rectTransform, Vector2 position, Vector2 offset)
    {
        Vector2 localPointInGrid;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, null, out localPointInGrid);
        Vector2 pivotOffset = offset;
        Vector2 adjustedLocalPoint = localPointInGrid + pivotOffset;
        return GetTileInGridPosition(adjustedLocalPoint);
    }


    //Getters de las variables locales de la clase Grid
    public bool GetTileState(Vector2Int tilePosition)
    {
        return _gridArrayGridInUse[tilePosition.x, tilePosition.y];
    }
    public float GetTileWidthSize()
    {
        return _tileSizeWidth;
    }
    public float GetTileHeightSize() 
    {
        return _tileSizeHeight;
    }
    public int GetGridWidth()
    {
        return _width;
    }
    public int GetGridHeight()
    {
        return _height;
    }
}
