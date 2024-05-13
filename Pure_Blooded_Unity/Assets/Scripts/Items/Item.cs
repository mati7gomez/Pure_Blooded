using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class Item : MonoBehaviour, IPickable, IInteractable
{
    [SerializeField] private ItemSO _itemSO; //Scriptable object del item

    private bool _canBePicked = false; //Bool para saber si el jugador esta en rango para agarrar el objeto

    private bool _isReading = false;

    private void Start()
    {
        ToggleNotaCanvas(false);
    }
    private void Update()
    {
        if (_canBePicked)
        {
            if (Input.GetButtonDown("Interact"))
            {
                _canBePicked = false;
                //Estoy preparando un condicional para la notas de texto que encuentre por el camino
                if(gameObject.CompareTag("NotaTexto")){
                    ReadNote(true);
                } else {
                    Pick();
                }  
            }
        }
    }

    //Triggers del item
    //Activacion y desactivacion del canvas de interactuar

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleInteractCanvas(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleNotaCanvas(false);
            ToggleInteractCanvas(false);
        }
    }
    //Metodos de la clase

    private ItemSO GetItemSO()
    {
        return _itemSO;
    }



    //Implementacion de interfaces

    //IPickable
    public void Pick()
    {
        ToggleInteractCanvas(false);
        InventoryManager inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        if (inventoryManager.AddItem(_itemSO))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No hay espacio para agregar el item");
        }
    }

    //IInteractable

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void ToggleInteractCanvas(bool enabled)
    {
        Canvas pickableItemCanvas = GameObject.Find("InteractCanvas").GetComponent<Canvas>();
        pickableItemCanvas.enabled = enabled;
        _canBePicked = enabled;
    }

    public void ReadNote(bool enabled){
        ToggleNotaCanvas(enabled);
        _isReading = enabled;
    }
    public void ToggleNotaCanvas(bool enabled){
        if(gameObject.CompareTag("NotaTexto")){
            GameObject pickableItemCanvas = GameObject.Find("NotasCanvas");
            pickableItemCanvas.GetComponent<Canvas>().enabled = enabled;

            GameObject imagen = pickableItemCanvas.transform.Find("FondoNota").gameObject;
            GameObject texto = imagen.transform.Find("Nota Text").gameObject;
            
            texto.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Nota>().texto;
        }
}
}
