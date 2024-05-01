using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Variable static para saber si alguno de los items del inventario esta siendo arrastrado (esto sirve para
    //que no se pueda cerrar el inventario hasta soltar el item, y no generar bugs)
    private static bool _dragging;
    public static bool GetDraggingState() => _dragging;

    private ItemSO _itemSO;

    //Atributos que se mostraran en el inventario, como para visualizarlos y su descripcion
    private string _itemName;
    private string _itemDescription;
    private Image _itemImage;
    private GameObject _itemRenderPrefab;

    private Transform _rootTransform; //Este transform debe ser el canvas donde se encuentra el menu del inventario y del item equipado
    private Transform _parentAfterDrag; //Transform para establecer el nuevo parent luego de arrastrar el item por el inventario

    private void Awake()
    {
        _rootTransform = transform.root.transform.GetChild(0);
        _itemImage = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData) //Metodo de la interfaz IBeginDragHandler para controlar cuando empieza el drag del item
    {
        _dragging = true; //...
        Debug.Log("Comienza el drag"); //...
        _parentAfterDrag = transform.parent; //...
        transform.SetParent(_rootTransform); //... Establecemos como parent el canvas
        transform.SetAsLastSibling(); //... Lo ponemos al final para que se muestre la imagen del item arriba del canvas y los paneles
        _itemImage.raycastTarget = false; //... Le saca el raycasTarget al componente Image ya que al dropear el item draggeado debe detectar si hay un InventorySlot debajo (si no se desactiva, detecta la imagen y no se coloca en el slot correcto, sino que vulve a su _parentAfterDrag)
    }
    public void OnDrag(PointerEventData eventData) //Metodo de la interfaz IDragHandler para controlar mientras se arrastra el item
    {
        Debug.Log("Draggeando");//...
        transform.position = Input.mousePosition; //Cada vez que movemos el mouse, movemos el item hacia su posicion
    }
    public void OnEndDrag(PointerEventData eventData) //Metodo de la interfaz IEndDragHandler para controlar cuando se termina el drag (soltamos el item)
    {
        Debug.Log("Termina el drag");//...
        transform.SetParent(_parentAfterDrag); //Vulve a su slot o al nuevo
        _itemImage.raycastTarget = true; //Volvemos a activar el raycasTarget para poder arrastrar el item en otra ocasion
        _dragging = false;//...
    }




    public void SetItemAttributes(ItemSO itemSO) //Metodo que se ejecuta desde el InventoryManager2 para establecer los atributos del item al ser instanciad en un slot
    {
        _itemSO = itemSO;
        _itemName = itemSO.GetItemName();
        _itemDescription = itemSO.GetItemDescription();
        if (_itemImage == null) _itemImage = GetComponent<Image>();
        _itemImage.sprite = itemSO.GetItemImage();
        _itemRenderPrefab = itemSO.GetItemPrefab();
    }
    public ItemSO GetItemSO() => _itemSO;




    public void SetNewParent(Transform newParentTransform) //Metodo para establecer el nuevo parent del item (un invenotry slot) si es que se coloco en otro slot q no sea el mismo
    {
        _parentAfterDrag = newParentTransform;
    }
    
    
}
