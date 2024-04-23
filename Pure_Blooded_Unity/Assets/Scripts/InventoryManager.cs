using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            // Si la instancia aún no está inicializada, la crea
            if (_instance == null)
            {
                // Busca una instancia existente en la escena
                _instance = FindObjectOfType<InventoryManager>();

                // Si no hay ninguna instancia en la escena, crea una nueva
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("InventoryManagerSingleton");
                    _instance = singletonObject.AddComponent<InventoryManager>();
                }
            }
            return _instance;
        }
    }


    [SerializeField] private InventorySlot[] _inventorySlots;

    // Propiedad pública para acceder a la instancia del Singleton


    // Asegúrate de que el Singleton persista entre las escenas
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private bool HasInventorySpace(out int slotIndex)
    {
        InventorySlot slot;
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            slotIndex = i;
            slot = _inventorySlots[i];
            if (slot.transform.childCount == 0)
            {
                
                return true;
            }
        }
        slotIndex = 0;
        return false;
    }
    private void AddItem(Item item)
    {
        int slotIndex;
        if (HasInventorySpace(out slotIndex))
        {
            SpawnItem();
        }
    }
    private void SpawnItem()
    {

    }

}
