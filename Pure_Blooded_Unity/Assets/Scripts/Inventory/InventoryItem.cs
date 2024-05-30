using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Variable static para controlar el abrir y cerrar del menu
    static bool _anyItemIsBeingDragged;

    //Variable que contiene la informacion del item
    private ItemSO _itemSO;

    //Componentes
    private Grid _itemGrid; //Componente Grid del item
    private RectTransform _rectTransform; //Componente RectTransform del item
    private Image _itemImage; //Componente Image del item

    private Rotation _itemRotation = new Rotation(); //Inicializacion del enum que guarda la rotacion actual del item
    private enum Rotation
    {
        right = 0,
        up  = 90,
        left = 180,
        down = 270,
    }

    
    private bool _isBeingDragged;

    private Transform _lastParent;
    private Vector3 _lastPosition;
    private Rotation _lastRotation;
    private Vector2Int _selectedTile; //VectorInt que almacena el tile seleccionado para draggear
    private Vector2 _lastPivot; //Vector que almacena el pivot deseado para draggear
    private Vector2 _mousePosBeforeDrag;
    private bool _hasMovedToOtherPos;


    //--------------------------------

    private void Start()
    {
        LoadComponents();
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
        _mousePosBeforeDrag = Input.mousePosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Desactivamos el raycastTarget del item al moverlo para que cuando lo soltemos el mouse detecte el inventario y no la imagen del item
        SetImageRaycastTarget(false);

        InventoryGridController invController = GameObject.Find("Inventario").GetComponent<InventoryGridController>();
        invController.SetInventoryOccupancyStateOnDrag(_mousePosBeforeDrag, _selectedTile, _itemGrid, (int)_itemRotation, false);

        //Guardamos la posicion, pivot, rotacion y parent antes de arrastrar el objeto
        SetLastPivot();
        SetLastPosition(_rectTransform.localPosition);
        SetLastRotation();
        SetLastParent(transform.parent);

        //Cambiamos el pivot al mover el item
        SetRectTransformPivot(GetDesiredItemPivot(_selectedTile));

        //Ponemos el item fuera del inventario y al final de la jerarquia para que al moverlo se muestre por encima de las demas imagenes
        transform.SetParent(transform.root.GetChild(0));
        transform.SetAsLastSibling();

        //Establecemos los valores de arrastrado en verdadero al mover el item
        SetAnyItemIsBeingDragged(true);
        SetIsBeingDragged(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Movemos el item a la posicion del mouse
        MoveImageToPosition(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Activamos el raycastTarget de la imagen de nuevo para que el mouse pueda detectar el item al querer arrastrarlo nuevamente
        SetImageRaycastTarget(true);

        //Aplicamos 
        transform.SetParent(_lastParent);
        SetRectTransformPivot(_lastPivot);
        SetRectTransformPosition(_lastPosition);
        SetRectTransformRotation(_lastRotation);
        if (!_hasMovedToOtherPos)
        {
            InventoryGridController invController = GameObject.Find("Inventario").GetComponent<InventoryGridController>();
            invController.SetInventoryOccupancyStateOnDrag(_mousePosBeforeDrag, _selectedTile, _itemGrid, (int)_itemRotation, true);
        }
        else
        {
            _hasMovedToOtherPos = false;
        }

        //Establecemos los valores de arrastrado en falso al soltar el item
        SetAnyItemIsBeingDragged(false);
        SetIsBeingDragged(false);
    }
    #endregion


    //-----------Metodos de la clase---------------------//

    public Vector2Int GetSelectedTile() => _selectedTile;
    public void SetSelectedTile(Vector2Int tile)
    {
        _selectedTile = tile;
    }

    //Pivot
    private void SetLastPivot() => _lastPivot = _rectTransform.pivot; //Metodo para guardar el pivot antes de mover el objeto
    public void SetNewPivot() => SetLastPivot(); //Metodo para cambiar el ultimo pivot guardado por el nuevo pivot
    private Vector2 GetDesiredItemPivot(Vector2Int tilePos) //Metodo para obtener el pivot deseado dependiendo de que tile se quiere arrastrar el item
    {
        float xPivot = SetAxisPivot(tilePos.x, _itemGrid.GetGridWidth()); //Obtenemos el pivot en x
        float yPivot = SetAxisPivot(tilePos.y, _itemGrid.GetGridHeight()); //Obtenemo el pivot en y
        //Debug.Log($"Pivot deseado: {xPivot},{yPivot}");
        return new Vector2(xPivot, yPivot);
    }
    private float SetAxisPivot(int tilePos, int tilesQuantity) //Metodo para establecer el valor del axis del pivot
    {
        float axisPivot = (1f / tilesQuantity) / 2f; //El pivot inicial si se selecciona el tile (0,0) es igual a la mitad de, 1 dividido la cantidad de tiles del item
        for (int i = 0; i < tilePos; i++) //Por cada tile seleccionado mayor a 0 se le suma 1 dividido la cantidad de tiles del item
        {
            axisPivot += 1f / tilesQuantity;
        }
        return axisPivot;
    }
    private Vector2 GetPivotOffset() //Metodo para saber el offset del pivot al calcular en que tile del item se clickeo
    {
        return new Vector2(_rectTransform.rect.width * _rectTransform.pivot.x, _rectTransform.rect.height * _rectTransform.pivot.y);
    }
    //private Vector2Int GetPivotTile()
    //{
    //    Vector2Int pivotTile = new Vector2Int(_lastPivot.x );

    //    return pivotTile;
    //}
    private void SetRectTransformPivot(Vector2 newPivot) => _rectTransform.pivot = newPivot; //Metodo para cambiar el pivot del _rectTransform

    //Position
    private void SetLastPosition(Vector3 lastPos) => _lastPosition = lastPos; //Metodo para guardar la posicion antes de mover el objeto
    public void SetNewPosition(Vector3 newPos) => SetLastPosition(newPos); //Metodo para cambiar la ultima posicion guardada por la nueva al ser colocado en el nuevo slot del inv
    private void MoveImageToPosition(Vector3 position) => transform.position = position; //Metodo para mover el objeto mientras se arrastra
    private void SetRectTransformPosition(Vector3 position) => _rectTransform.localPosition = position;

    //Rotation
    private void SetLastRotation() => _lastRotation = GetItemRotation(); //Metodo para guardar la rotacion antes de mover el objeto
    public void SetNewRotation() => SetLastRotation(); //Metodo para cambiar la ultima rotacion guardada por la actual al ser colocado en el nuevo slot del inv
    private void ChangeItemRotation() //Metodo para cambiar la rotacion del item al apretar la tecla de rotar item al moverlo
    {
        switch (_itemRotation)
        {
            case Rotation.right:
                _itemRotation = Rotation.up;
                _rectTransform.SetLocalPositionAndRotation(_rectTransform.localPosition, Quaternion.Euler(0, 0, 90)); ;
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
    public int GetItemRotationDir() => (int)_itemRotation; //Metodo para obtener la direccion de rotacion actual del item
    private Rotation GetItemRotation() => _itemRotation; //Metodo para obtener la rotacion actual del item
    private void SetRectTransformRotation(Rotation rotation)
    {
        _rectTransform.localRotation = Quaternion.Euler(0f, 0f, (int)rotation);
        _itemRotation = rotation;
    }

    //Parent
    private void SetLastParent(Transform lastParent) => _lastParent = lastParent; //Metodo para guardar el anterior parent del transform



    private void SetImageRaycastTarget(bool value)
    {
        if (_itemImage == null) _itemImage = GetComponent<Image>();
        _itemImage.raycastTarget = value;
    }
    private void SetAnyItemIsBeingDragged(bool value) => _anyItemIsBeingDragged = value;
    public static bool GetAnyItemIsBeingDragged() => _anyItemIsBeingDragged;
    private void SetIsBeingDragged(bool value) => _isBeingDragged = value;
    public void SetHasMovedToOtherPos(bool value) => _hasMovedToOtherPos = value;



    //Metodos que se van a ejecutar al crear un nuevo item en el inventario (cuando se agarra el objeto)
    public void SetItemVariablesOnCreated(ItemSO itemSO, int itemRotation, Vector3 itemPosition)
    {
        LoadComponents();
        SetItemSO(itemSO);
        SetImageSprite();
        SetItemGridSize();
        SetInitialPivot();
        SetInitialPosition(itemPosition);
        SetInitialRotation(itemRotation);
        SetRectTransformSize();
    }
    private void LoadComponents()
    {
        _itemGrid = GetComponent<Grid>();
        _rectTransform = GetComponent<RectTransform>();
        _itemImage = GetComponent<Image>();
        
    }
    private void SetInitialPivot() => SetRectTransformPivot(GetDesiredItemPivot(Vector2Int.zero));
    private void SetInitialPosition(Vector3 newPos) => SetRectTransformPosition(newPos);
    private void SetInitialRotation(int rot) => SetRectTransformRotation((Rotation)rot);
    private void SetItemSO(ItemSO newItemSO) => _itemSO = newItemSO;
    private void SetImageSprite() => _itemImage.sprite = _itemSO.GetItemImage();
    private void SetItemGridSize()
    {
        _itemGrid.SetGridWidth(_itemSO.GetItemSizeInInventory().x);
        _itemGrid.SetGridHeight(_itemSO.GetItemSizeInInventory().y);
    }
    private void SetRectTransformSize() => _rectTransform.sizeDelta = new Vector2(_itemGrid.GetGridWidth() * _itemGrid.GetTileWidthSize(), _itemGrid.GetGridHeight() * _itemGrid.GetTileHeightSize());
    
}
