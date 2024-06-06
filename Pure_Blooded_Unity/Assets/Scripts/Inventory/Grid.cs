using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Librería de Visual Scripting de Unity
using UnityEngine;

public class Grid : MonoBehaviour // Define la clase Grid y hereda de MonoBehaviour, lo que permite que sea un componente de un objeto en Unity
{
    //
    // Tamaño de cada celda de la cuadrícula
    private const float _tileSizeWidth = 180;
    private const float _tileSizeHeight = 180;

    // Ancho y alto de la cuadrícula
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    // Estado de ocupación de cada celda de la cuadrícula
    [SerializeField] private bool _isConteiner;
    private bool[,] _tileOccupancyState;

    // Posición en la cuadrícula y posición de la celda en la cuadrícula
    private Vector2 _positionOnTheGrid = new Vector2();
    private Vector2Int _tileGridPosition = new Vector2Int();

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
    }

    private void Start()
    {
        // Inicializa el estado de ocupación de la cuadrícula
        if (_isConteiner)
            _tileOccupancyState = new bool[_width, _height];

        //Debug.Log(_tileOccupancyState.Length);
    }

    // Método que devuelve la posición de la celda en la cuadrícula basada en una posición de entrada
    private Vector2Int GetTileInGrid(Vector2 inputPosition)
    {
        _positionOnTheGrid.x = inputPosition.x;
        _positionOnTheGrid.y = inputPosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / _tileSizeWidth);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / _tileSizeHeight);

        return _tileGridPosition;
    }

    // Método que devuelve la posición de la celda en la cuadrícula basada en una posición de entrada y un desplazamiento
    public Vector2Int GetTileInGrid(RectTransform rectTransform, Vector2 inputPosition, Vector2 offset)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, inputPosition, null, out Vector2 localPointInGrid);
        Vector2 pivotOffset = offset;
        Vector2 adjustedLocalPoint = localPointInGrid + pivotOffset;
        return GetTileInGrid(adjustedLocalPoint);
    }

    // Métodos para obtener y establecer el estado de ocupación de una celda específica
    public bool GetTileOccupancyState(Vector2Int tilePosition)
    {
        return _tileOccupancyState[tilePosition.x, tilePosition.y];
    }
    private void SetTileOccupancyState(Vector2Int tilePos, bool occupancyValue)
    {
        _tileOccupancyState[tilePos.x, tilePos.y] = occupancyValue;
    }
    public void SetTilesOccupancyState(int initX, int initY, int maxX, int maxY, bool occupancyValue)
    {
        //Debug.Log($"initx: {initX} inity: {initY} maxX: {maxX} maxY: {maxY}");
        //Debug.Log($"{_tileOccupancyState.GetLength(0)},{_tileOccupancyState.GetLength(1)}");
        for (int i = initX; i <= maxX; i++)
        {
            for (int j = initY; j <= maxY; j++)
            {
                //Debug.Log($"Tile ({i},{j})");
                SetTileOccupancyState(new Vector2Int(i, j), occupancyValue);
                //Debug.Log($"Tile ({i},{j})");
            }
        }
    }

    // Métodos para obtener el tamaño de las celdas y las dimensiones de la cuadrícula
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

    public void SetGridWidth(int width) { _width = width; }
    public void SetGridHeight(int height) {  _height = height; }
}
