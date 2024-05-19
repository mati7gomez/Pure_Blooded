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
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPickable, IInteractable
{
    [SerializeField] private ItemSO _itemSO; //Scriptable object del item

    private GameObject _player;

    private bool _canBePicked = false; //Bool para saber si el jugador esta en rango para agarrar el objeto

    private bool _isReading = false;
    private bool _isExaminating = false;
    
    [Header("Examinate")]

    private CinemachineBrain _mainCamera;
    private GameObject _objectExaminated;
    private Item3DExamin _item3DExamin;

    private void Start()
    {
        ToggleNotaCanvas(false);
        _item3DExamin = FindObjectOfType<Item3DExamin>();
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

            if(Input.GetKeyDown(KeyCode.T) && !_isExaminating){
                _player.GetComponent<PlayerController>().CanMove = false;
                
                GameObject _rawImage = _item3DExamin.gameObject.transform.Find("FondoExaminar").gameObject;
                _rawImage.GetComponent<RawImage>().enabled = true;

                _objectExaminated = _item3DExamin.Examine(_itemSO);
                _isExaminating = true;
            } 
            
            if(Input.GetKeyDown(KeyCode.Y)) {
                _player.GetComponent<PlayerController>().CanMove = true;
                
                GameObject _rawImage = _item3DExamin.gameObject.transform.Find("FondoExaminar").gameObject;
                _rawImage.GetComponent<RawImage>().enabled = false;
                
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
