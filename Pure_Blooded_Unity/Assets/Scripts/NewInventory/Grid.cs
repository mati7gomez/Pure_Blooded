using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Librer�a de Visual Scripting de Unity
using UnityEngine;

public class Grid : MonoBehaviour // Define la clase Grid y hereda de MonoBehaviour, lo que permite que sea un componente de un objeto en Unity
{
    // Tama�o de cada celda de la cuadr�cula
    private const float _tileSizeWidth = 180;
    private const float _tileSizeHeight = 180;

    // Ancho y alto de la cuadr�cula
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    // Estado de ocupaci�n de cada celda de la cuadr�cula
    private bool[,] _tileOccupancyState;

    // Posici�n en la cuadr�cula y posici�n de la celda en la cuadr�cula
    private Vector2 _positionOnTheGrid = new Vector2();
    private Vector2Int _tileGridPosition = new Vector2Int();

    private void Start()
    {
        // Inicializa el estado de ocupaci�n de la cuadr�cula
        _tileOccupancyState = new bool[_width, _height];
    }

    // M�todo que devuelve la posici�n de la celda en la cuadr�cula basada en una posici�n de entrada
    public Vector2Int GetTileInGrid(Vector2 inputPosition)
    {
        _positionOnTheGrid.x = inputPosition.x;
        _positionOnTheGrid.y = inputPosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / _tileSizeWidth);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / _tileSizeHeight);

        return _tileGridPosition;
    }

    // M�todo que devuelve la posici�n de la celda en la cuadr�cula basada en una posici�n de entrada y un desplazamiento
    public Vector2Int GetTileInGrid(RectTransform rectTransform, Vector2 position, Vector2 offset)
    {
        Vector2 localPointInGrid;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, null, out localPointInGrid);
        Vector2 pivotOffset = offset;
        Vector2 adjustedLocalPoint = localPointInGrid + pivotOffset;
        return GetTileInGrid(adjustedLocalPoint);
    }

    // M�todos para obtener y establecer el estado de ocupaci�n de una celda espec�fica
    public bool GetTileOccupancyState(Vector2Int tilePosition)
    {
        return _tileOccupancyState[tilePosition.x, tilePosition.y];
    }
    public void SetTileOccupancyState(Vector2Int tilePos, bool occupancyValue)
    {
        _tileOccupancyState[tilePos.x, tilePos.y] = occupancyValue;
    }

    // M�todos para obtener el tama�o de las celdas y las dimensiones de la cuadr�cula
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
