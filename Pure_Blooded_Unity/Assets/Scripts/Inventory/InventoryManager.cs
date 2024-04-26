using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private GameObject _itemPrefab;

    // Propiedad pública para acceder a la instancia del Singleton


    // Asegúrate de que el Singleton persista entre las escenas
    private void Awake()
    {

    }


    public bool AddItem(ItemSO item)
    {
        if (HasInventorySpace(out int slotIndex))
        {
            SpawnItem(item, slotIndex);
            return true;
        }
        return false;
    }
    public bool HasInventorySpace(out int slotIndex)
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
    private void SpawnItem(ItemSO item, int slotIndex)
    {
        GameObject inventoryItem = Instantiate(_itemPrefab, _inventorySlots[slotIndex].transform);
        inventoryItem.GetComponent<InventoryItem>().SetItemAttributes(item);
    }
}
