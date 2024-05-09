using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    private Vector2 _lastPivot; //Vector que almacena el pivot deseado para draggear


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
        Debug.Log(_selectedTile);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Desactivamos el raycastTarget del item al moverlo para que cuando lo soltemos el mouse detecte el inventario y no la imagen del item
        SetImageRaycastTarget(false);

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
        SetRectTransformPosition();
        SetRectTransformRotation();


        //Establecemos los valores de arrastrado en falso al soltar el item
        SetAnyItemIsBeingDragged(false);
        SetIsBeingDragged(false);
    }
    #endregion


    //-----------Metodos de la clase---------------------//

    public Vector2Int GetSelectedTile() => _selectedTile;

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
        float axisPivot = (1f / tilesQuantity) / 2f; //El pivot inicial si se selecciona el tile (0,0) es igual a la mitad de 1 dividido la cantidad de tiles del item
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
    private void SetRectTransformPivot(Vector2 newPivot) => _rectTransform.pivot = newPivot; //Metodo para cambiar el pivot del rectTransform

    //Position
    private void SetLastPosition(Vector3 lastPos) => _lastPosition = lastPos; //Metodo para guardar la posicion antes de mover el objeto
    public void SetNewPosition(Vector3 newPos) => SetLastPosition(newPos); //Metodo para cambiar la ultima posicion guardada por la nueva al ser colocado en el nuevo slot del inv
    private void MoveImageToPosition(Vector3 position) => transform.position = position; //Metodo para mover el objeto mientras se arrastra
    private void SetRectTransformPosition() => _rectTransform.localPosition = _lastPosition;

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
    private void SetRectTransformRotation()
    {
        _rectTransform.localRotation = Quaternion.Euler(0f, 0f, (int)_lastRotation);
        _itemRotation = _lastRotation;
    }

    //Parent
    private void SetLastParent(Transform lastParent) => _lastParent = lastParent; //Metodo para guardar el anterior parent del transform



    private void SetImageRaycastTarget(bool value)
    {
        if (_image == null) _image = GetComponent<Image>();
        _image.raycastTarget = value;
    }
    private void SetAnyItemIsBeingDragged(bool value) => _anyItemIsBeingDragged = value;
    public static bool GetAnyItemIsBeingDragged() => _anyItemIsBeingDragged;
    private void SetIsBeingDragged(bool value) => _isBeingDragged = value;
    
}
