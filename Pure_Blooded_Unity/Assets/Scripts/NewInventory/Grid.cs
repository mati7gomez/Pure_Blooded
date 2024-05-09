using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Librería de Visual Scripting de Unity
using UnityEngine;

public class Grid : MonoBehaviour // Define la clase Grid y hereda de MonoBehaviour, lo que permite que sea un componente de un objeto en Unity
{
    // Tamaño de cada celda de la cuadrícula
    private const float _tileSizeWidth = 180;
    private const float _tileSizeHeight = 180;

    // Ancho y alto de la cuadrícula
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    // Estado de ocupación de cada celda de la cuadrícula
    private bool[,] _tileOccupancyState;

    // Posición en la cuadrícula y posición de la celda en la cuadrícula
    private Vector2 _positionOnTheGrid = new Vector2();
    private Vector2Int _tileGridPosition = new Vector2Int();

    private void Start()
    {
        // Inicializa el estado de ocupación de la cuadrícula
        _tileOccupancyState = new bool[_width, _height];
    }

    // Método que devuelve la posición de la celda en la cuadrícula basada en una posición de entrada
    public Vector2Int GetTileInGrid(Vector2 inputPosition)
    {
        _positionOnTheGrid.x = inputPosition.x;
        _positionOnTheGrid.y = inputPosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / _tileSizeWidth);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / _tileSizeHeight);

        return _tileGridPosition;
    }

    // Método que devuelve la posición de la celda en la cuadrícula basada en una posición de entrada y un desplazamiento
    public Vector2Int GetTileInGrid(RectTransform rectTransform, Vector2 position, Vector2 offset)
    {
        Vector2 localPointInGrid;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, null, out localPointInGrid);
        Vector2 pivotOffset = offset;
        Vector2 adjustedLocalPoint = localPointInGrid + pivotOffset;
        return GetTileInGrid(adjustedLocalPoint);
    }

    // Métodos para obtener y establecer el estado de ocupación de una celda específica
    public bool GetTileOccupancyState(Vector2Int tilePosition)
    {
        return _tileOccupancyState[tilePosition.x, tilePosition.y];
    }
    public void SetTileOccupancyState(Vector2Int tilePos, bool occupancyValue)
    {
        _tileOccupancyState[tilePos.x, tilePos.y] = occupancyValue;
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
}
