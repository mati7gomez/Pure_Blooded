using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Color _defaultColor; //Color del fondo del slot (esto creo q con el diseño se va a quitar)



    private void Awake()
    {
        _defaultColor = gameObject.GetComponent<Image>().color;
    }



    public void OnDrop(PointerEventData eventData) //Metodo de la interfaz IDropHandler para controlar cuando se suelta un InvenotryItem arriba del slot
    {
        if (transform.childCount == 0) //Si no hay ningun child (ningun InventoryItem en el slot)
        {
            GameObject itemDropped = eventData.pointerDrag; //Referencia del objeto que esta siendo arrastrado
            InventoryItem draggableItem = itemDropped.GetComponent<InventoryItem>(); //Accedemos al script InvItem del objeto
            draggableItem.SetNewParent(transform); //Establecemos como nuevo parent del InvItem al slot donde se solto el item
        }
    }

   
    public void OnPointerEnter(PointerEventData eventData) //Visuales del slot
    {
        if (transform.childCount == 0)
        {
            gameObject.GetComponent<Image>().color = Color.gray;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData) //Visuales del slot
    {
        gameObject.GetComponent<Image>().color = _defaultColor;
    }
}
