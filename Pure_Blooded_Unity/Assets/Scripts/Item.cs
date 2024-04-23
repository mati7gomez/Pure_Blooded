using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IPickable, IInteractable
{
    [SerializeField] private ItemSO _itemSO;


    private bool _canInteract;

    private bool _canBePicked;

    private void Start()
    {
        _canInteract = _itemSO.GetInteractState();
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
            ToggleInteractCanvas(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleInteractCanvas(false);
        }
    }

    //Implementacion de interfaces

    //IPickable
    public void Pick()
    {
        ToggleInteractCanvas(false);
        //if (InventoryManager.Instance.has)
    }

    public void ToggleInteractCanvas(bool enabled)
    {
        Canvas pickableItemCanvas = GameObject.Find("InteractCanvas").GetComponent<Canvas>();
        pickableItemCanvas.enabled = enabled;
        _canBePicked = enabled;
    }

    //IInteractable

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
