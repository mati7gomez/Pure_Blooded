using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewInventoryItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Variable static para controlar el abrir y cerrar del menu
    static bool _anyItemIsBeingDragged;

    [SerializeField] private ItemSO _itemSO;

    //Variables para el uso del Grid en el objeto
    private Grid _itemGrid;
    private RectTransform _rectTransform;
    private Vector2Int _selectedTile;
    private Vector2Int _tileBeforeDrag;
    private Vector2 _desiredPivot;
    private Vector2 _pivotBeforeDrag;


    private bool _isBeingDragged;
    private Transform _lastParent;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    private void Start()
    {
        _itemGrid = GetComponent<Grid>();
        _rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

        }
    }


    //----------------Implementacion interfaces----------------//


    #region Interfaces
    public void OnPointerDown(PointerEventData eventData)
    {
        _tileBeforeDrag = _selectedTile;
        _selectedTile = _itemGrid.GetTileInGrid(_rectTransform, Input.mousePosition, GetPivotOffset());
        _pivotBeforeDrag = _rectTransform.pivot;
        _desiredPivot = GetDesiredItemPivot(_selectedTile);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetRectTransformPivot(_desiredPivot);
        SetImageRaycastTarget(false);
        SetLastParent(transform.parent);
        transform.SetParent(transform.root.GetChild(0));
        transform.SetAsLastSibling();
        SetAnyItemIsBeingDragged(true);
        SetIsBeingDragged(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveImageToPosition(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetImageRaycastTarget(true);
        transform.SetParent(_lastParent);
        SetAnyItemIsBeingDragged(false);
        SetIsBeingDragged(false);
    }
    #endregion


    //-----------Metodos de la clase---------------------//

    private void SetLastParent(Transform lastParent) => _lastParent = lastParent;
    public void SetNewParent(Vector3 newPos)
    {
        transform.SetParent(_lastParent);
        transform.localPosition = newPos;
        //Debug.Log("Deberia estar en: " + newPos);
    }

    private Vector2 GetDesiredItemPivot(Vector2Int tilePos)
    {
        float xPivot = SetAxisPivot(tilePos.x, _itemGrid.GetGridWidth());
        float yPivot = SetAxisPivot(tilePos.y, _itemGrid.GetGridHeight());
        //Debug.Log($"Pivot deseado: {xPivot},{yPivot}");
        return new Vector2(xPivot, yPivot);
    }
    private float SetAxisPivot(int tilePos, int tilesQuantity)
    {
        float axisPivot = (1f / tilesQuantity) / 2f;
        for (int i = 0; i < tilePos; i++)
        {
            axisPivot += 1f / tilesQuantity;
        }
        return axisPivot;
    }
    private Vector2 GetPivotOffset()
    {
        return new Vector2(_rectTransform.rect.width * _rectTransform.pivot.x, _rectTransform.rect.height * _rectTransform.pivot.y);
    }
    private void SetRectTransformPivot(Vector2 newPivot)
    {
        _rectTransform.pivot = newPivot;
    }

    private void SetImageRaycastTarget(bool value)
    {
        if (_image == null) _image = GetComponent<Image>();
        _image.raycastTarget = value;
    }
    private void SetAnyItemIsBeingDragged(bool value) => _anyItemIsBeingDragged = value;
    public static bool GetAnyItemIsBeingDragged() => _anyItemIsBeingDragged;
    private void SetIsBeingDragged(bool value) => _isBeingDragged = value;
    private void MoveImageToPosition(Vector3 position) => transform.position = position;
    public Vector2Int GetSelectedTile() => _selectedTile;
}
