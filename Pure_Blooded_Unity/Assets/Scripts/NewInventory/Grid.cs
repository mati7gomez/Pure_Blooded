using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    const float _tileSizeWidth = 128;
    const float _tileSizeHeight = 128;

    RectTransform _rectTransform;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    Vector2 _positionOnTheGrid = new Vector2();
    Vector2Int _tileGridPosition = new Vector2Int();

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        _positionOnTheGrid.x = mousePosition.x - _rectTransform.position.x;
        _positionOnTheGrid.y = _rectTransform.position.y - mousePosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / _tileSizeWidth);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / _tileSizeHeight);

        return _tileGridPosition;
    }
}
