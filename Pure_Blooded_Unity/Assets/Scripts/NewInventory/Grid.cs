using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    const float _tileSizeWidth = 180;
    const float _tileSizeHeight = 180;

    private void Start()
    {

    }

    Vector2 _positionOnTheGrid = new Vector2();
    Vector2Int _tileGridPosition = new Vector2Int();

    public Vector2Int GetTileGridPosition(Vector2 mousePositionInRectagle)
    {
        _positionOnTheGrid.x = mousePositionInRectagle.x;
        _positionOnTheGrid.y = -mousePositionInRectagle.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / _tileSizeWidth );
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / _tileSizeHeight);

        return _tileGridPosition;
    }
}
