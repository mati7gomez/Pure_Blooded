using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Item : MonoBehaviour, IPickable, IInteractable
{
    [SerializeField] private ItemSO _itemSO; //Scriptable object del item

    private GameObject _player; //Referencia al jugador

    private bool _canBePicked = false; //Bool para saber si el jugador esta en rango para agarrar el objeto

    private bool _isReading = false;
    private bool _isExaminating = false;
    private GameObject _itemInstance;
    

    [Header("Examinar")]
    private GameObject _objectExaminated;
    private GameObject _item3DExamin;

    private void Start()
    {
        ToggleNotaCanvas(false);
        _item3DExamin = GameObject.Find("Item3DExamin");
    }
    private void Update()
    {
        if (_canBePicked)
        {
            if (Input.GetButtonDown("Interact"))
            {
                _canBePicked = false;

                if(gameObject.CompareTag("NotaTexto")){
                    ReadNote(true);
                } else {
                    Pick();
                }  
            }

            if(Input.GetKeyDown(KeyCode.T) && !_isExaminating){
                _player.GetComponent<PlayerController>().CanMove = false;
                _item3DExamin.GetComponent<RawImage>().enabled = true;
                _objectExaminated = _item3DExamin.GetComponent<Item3DExamin>().Examine(_itemSO);
                _isExaminating = true;
            } 
            
            if(Input.GetKeyDown(KeyCode.Y)) {
                _player.GetComponent<PlayerController>().CanMove = true;
                _item3DExamin.GetComponent<RawImage>().enabled = false;
                Destroy(_objectExaminated);
                _isExaminating = false;
            }
        }
    }

    //Triggers del item
    //Activacion y desactivacion del canvas de interactuar

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject;
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
        _player.GetComponent<PlayerController>().PickUpItem(_itemSO);

        ToggleInteractCanvas(false);
        InventoryManager inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        if (inventoryManager.CanAddItem(_itemSO))
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
