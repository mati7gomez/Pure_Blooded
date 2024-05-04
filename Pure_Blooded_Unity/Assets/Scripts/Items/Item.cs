using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Item : MonoBehaviour, IPickable, IInteractable
{
    [SerializeField] private ItemSO _itemSO; //Scriptable object del item

    private GameObject _player; //Referencia del jugador

    private bool _canBePicked = false; //Bool para saber si el jugador esta en rango para agarrar el objeto

    private void Start()
    {

    }
    private void Update()
    {
        if (_canBePicked)
        {
            if (Input.GetButtonDown("Interact"))
            {
                _canBePicked = false;
                Pick();
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
            ToggleInteractCanvas(false);
            _player = null;
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
        InventoryManager2 inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager2>();
        if (inventoryManager.AddItem(GetItemSO()))
        {
            PlayerAnimatorController playerAnimator = _player.transform.GetChild(0).GetComponent<PlayerAnimatorController>();
            PlayerController playerController = _player.GetComponent<PlayerController>();
            playerAnimator.SetTrigger("Pick");
            playerController.CanMove = false;
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
}
