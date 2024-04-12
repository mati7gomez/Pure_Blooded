using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IPickable
{
    private bool _canBePicked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TogglePickCanvas(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TogglePickCanvas(false);
        }
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
    public void Pick()
    {
        TogglePickCanvas(false);
        Destroy(gameObject);
    }

    public void TogglePickCanvas(bool enabled)
    {
        Canvas pickableItem = GameObject.Find("InteractCanvas").GetComponent<Canvas>();
        pickableItem.enabled = enabled;
        _canBePicked = enabled;
    }
}
