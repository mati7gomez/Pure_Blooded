using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private RectTransform _rectTransform; //Componente rect transform
    [SerializeField] Grid _selectedGrid; //Referencia de la grilla seleccionada

    private Vector2 _localPointInGrid; //Posicion del mouse dentro de la grilla

    private Vector3 _rectTransformAlignment; //Vector que sirve para calcular la posicion de los items droppeados
                                             //dentro de la grilla del inventario

    //------------Metodos propios de MonoBehaviour-------------//
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransformAlignment = new Vector2(_rectTransform.rect.width / 2f, -_rectTransform.rect.height / 2f);
    }
    private void Update()
    {
        if (_selectedGrid == null) return;
    }

    //----------------Implementacion interfaces----------------//
    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectedGrid = eventData.pointerEnter.transform.GetComponent<Grid>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selectedGrid = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<NewInventoryItem>() != null)
        {
            if (_selectedGrid != null)
            {
                Vector2Int tilePos = GetTileInGridPosition(eventData.position);
                if (tilePos == new Vector2Int(0, 0))
                {
                    eventData.pointerDrag.gameObject.GetComponent<NewInventoryItem>().SetNewParent(transform, _rectTransformAlignment + GetTileCoordsToPositionImage(tilePos));
                }
                else if (tilePos == new Vector2Int(0, 1))
                {
                    eventData.pointerDrag.gameObject.GetComponent<NewInventoryItem>().SetNewParent(transform, new Vector3(0,0,0));
                }
                Vector2 extraPosition = GetTileCoordsToPositionImage(tilePos);
            }

        }
    }

    //-----------Metodos de la clase---------------------//

    private Vector2Int GetTileInGridPosition(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, position, null, out _localPointInGrid);
        return _selectedGrid.GetTileInGridPosition(_localPointInGrid);
    }

    private Vector3 GetTileCoordsToPositionImage(Vector2Int tilePos)
    {
        int tileX = tilePos.x;
        int tileY = tilePos.y;

        float x = 0f;
        float y = 0f;

        int gridWidth = _selectedGrid.GetGridWidth();
        int gridHeight = _selectedGrid.GetGridHeight();

        if (gridWidth > 0)
        {
            if (tileX < 1)
            if (gridWidth % 2 != 0)
            {
                x += gridWidth / 2;
            }
        }
        return new Vector2(x, y);
    }
}
