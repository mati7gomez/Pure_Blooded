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

    private Grid _itemGrid; //Objeto grid del item
    private RectTransform _rectTransform; //Componente rect transform del item

    private Rotation _itemRotation = new Rotation(); //Inicializacion del enum que guarda la rotacion actual del item
    private enum Rotation
    {
        right = 0,
        up  = 90,
        left = 180,
        down = 270,
    }

    private Image _image;
    private bool _isBeingDragged;

    private Transform _lastParent;
    private Vector3 _lastPosition;
    private Rotation _lastRotation;
    private Vector2Int _selectedTile; //VectorInt que almacena el tile seleccionado para draggear
    private Vector2 _desiredPivot; //Vector que almacena el pivot deseado para draggear


    //--------------------------------

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
        if (_isBeingDragged && Input.GetKeyDown(KeyCode.R))
        {
            ChangeItemRotation();
        }
    }


    //----------------Implementacion interfaces----------------//


    #region Interfaces
    public void OnPointerDown(PointerEventData eventData)
    {
        _selectedTile = _itemGrid.GetTileInGrid(_rectTransform, Input.mousePosition, GetPivotOffset());
        _desiredPivot = GetDesiredItemPivot(_selectedTile);
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetImageRaycastTarget(false);

        _lastRotation = _itemRotation;
        SetLastPosition(_rectTransform.localPosition);
        SetRectTransformPivot(_desiredPivot);
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
        //_rectTransform.localPosition = _lastPosition;
        _rectTransform.SetLocalPositionAndRotation(_lastPosition, Quaternion.Euler(0, 0, (int)_lastRotation));
        _itemRotation = _lastRotation;
        //_rectTransform.pivot = _desiredPivot;


        SetAnyItemIsBeingDragged(false);
        SetIsBeingDragged(false);
    }
    #endregion


    //-----------Metodos de la clase---------------------//

    private void SetLastParent(Transform lastParent) => _lastParent = lastParent;
    private void SetLastPosition(Vector3 lastPos) => _lastPosition = lastPos;
    public void SetNewPosition(Vector3 newPos) => _lastPosition = newPos;
    public void SetNewRotation()
    {
        _lastRotation = GetItemRotation();
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
    private void ChangeItemRotation()
    {
        switch (_itemRotation)
        {
            case Rotation.right:
                _itemRotation = Rotation.up;
                _rectTransform.SetLocalPositionAndRotation(_rectTransform.localPosition, Quaternion.Euler(0,0,90)); ;
                break;
            case Rotation.up:
                _itemRotation = Rotation.left;
                _rectTransform.SetLocalPositionAndRotation(_rectTransform.localPosition, Quaternion.Euler(0, 0, 180));
                break;
            case Rotation.left:
                _itemRotation = Rotation.down;
                _rectTransform.SetLocalPositionAndRotation(_rectTransform.localPosition, Quaternion.Euler(0, 0, 270));
                break;
            case Rotation.down:
                _itemRotation = Rotation.right;
                _rectTransform.SetLocalPositionAndRotation(_rectTransform.localPosition, Quaternion.Euler(0, 0, 0)); ;
                break;
        }
    }
    public int GetItemRotationDir()
    {
        return (int)_itemRotation;
    }
    private Rotation GetItemRotation()
    {
        return _itemRotation;
    }
}
