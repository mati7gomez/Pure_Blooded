using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InventoryManager2 : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _inventorySlots; //Array de slots donde se van a guardar los items (hay que asiganarlos manualmente desde la escena)
    [SerializeField] private GameObject _inventoryItemPrefab; //Prefab de tipo InventoryItem para poder spawnearlos al agarrarlos
    private GameObject _inventoryMenu; //Gmame object donde se encuentran los paneles del inventario, view y descripcion del item
    private ItemViewPanel _itemViewPanel;

    private bool _inventoryOpened; //Obvio xdxd
    public bool InventoryOpened => _inventoryOpened;

    private void Start()
    {
        _itemViewPanel = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<ItemViewPanel>();
        _inventoryMenu = transform.GetChild(0).transform.GetChild(0).gameObject; //Esta dentro del hijo del panel, por eso es x2
        _inventoryMenu.SetActive(false);
    }

    private void Update()
    {
        OpenAndCloseInventory();
    }


    public bool AddItem(ItemSO item) //Metodo para agregar item (se llama desde el script item y desde su funcion Pickable())
    {
        if (HasInventorySpace(out int slotIndex)) //Si hay espacio...
        {
            SpawnItem(item, slotIndex);
            return true; //Devolvemos true ya que se pudo spawnear el item
        }
        return false; //Devolvemos false si no hay mas espacio
    }
    private bool HasInventorySpace(out int slotIndex) //Metodo para saber si hay espacio en alguno de los slots del inventario (tambien hay un int out donde se almacena el slot donde hay espacio)
    {
        InventorySlot slot;
        for (int i = 0; i < _inventorySlots.Length; i++) //Recorremos cada slot para ver si hay o no un objeto child en el mismo
        {
            slotIndex = i;
            slot = _inventorySlots[i];
            if (slot.transform.childCount == 0)
            {
                return true; //Si no hay ningun child devuelve true ya que hay espacio
            }
        }
        slotIndex = 0;
        return false; //Si hay un child devuelve false ya que no hay espacio
    }
    private void SpawnItem(ItemSO item, int slotIndex) //Metodo para spawnear el item en el slot
    {
        GameObject inventoryItem = Instantiate(_inventoryItemPrefab, _inventorySlots[slotIndex].transform); //Intanciamos un objeto de tipo InventoryItem en la escena y le ponemos de padre el slot donde va
        inventoryItem.GetComponent<InventoryItem>().SetItemAttributes(item); //Le ponemos los atributos a ese item, dependiendo de cual Item fue agarrado (Por ahora se establece el nombre, descripcion y su imagen)
    }



    private void OpenAndCloseInventory() //Metodo para abrir y cerrar el inventario
    {
        if (Input.GetButtonDown("Inventory") && !InventoryItem.GetDraggingState()) //Solo podemos abrir o cerrar el inventario si apretamos el boton de inventario (actualmente tab) y si ningun InventoryItem esta siendo draggeado 
        {
            if (!_inventoryOpened) OpenInventory();
            else CloseInvenotry();
        }
    }
    private void OpenInventory() //...
    {
        
        _inventoryMenu.SetActive(true);
        _inventoryOpened = true;
    } 
    private void CloseInvenotry() //...
    {
        ClearItemViewPanel();
        _inventoryMenu.SetActive(false);
        _inventoryOpened = false;
    } 



    private void ClearItemViewPanel()
    {
        _itemViewPanel.ClearItemTransform();
    }

    public void OnItemSelected(ItemSO itemSO)
    {
        _itemViewPanel.SetViewPanelItem(itemSO);
    }


}
