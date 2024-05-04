using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemViewPanel : MonoBehaviour
{
    private GameObject _itemRenderer;
    private GameObject _itemRendererContainer;
    private Transform _itemTransform;

    private string _itemType;

    private bool _hasItem;

    private void Start()
    {
        _itemRenderer = transform.root.transform.GetChild(1).gameObject;
        _itemRendererContainer = _itemRenderer.transform.GetChild(0).gameObject;
    }
    private void FixedUpdate()
    {
        if (GetHasItem())
        {
            _itemTransform.Rotate(0f, 25f * Time.fixedDeltaTime, 0f, Space.World);
        }
    }

    private bool GetHasItem()
    {
        if (_itemRenderer != null && _itemRendererContainer.transform.childCount != 0)
        {
            SetItemTransform();
            _hasItem = true;
            return true;
        }
        ClearItemTransform();
        _hasItem = false;
        return false;
    }
    private void SetItemTransform()
    {
        if (_itemTransform == null)
        {
            _itemTransform = _itemRendererContainer.transform.GetChild(0);
        }
    }
    public void ClearItemTransform()
    {
        if (_itemTransform != null)
        {
            Destroy(_itemTransform.gameObject);
            _itemTransform = null;
        }
    }

    public void SetViewPanelItem(ItemSO itemSO)
    {
        if (_itemRendererContainer.transform.childCount == 0)
        {
            SpawnItem(itemSO.GetItemPrefab());
            _itemType = itemSO.GetItemName();
        }
        else if (_itemRendererContainer.transform.childCount != 0)
        {
            if (!_itemType.Equals(itemSO.GetItemName()))
            {
                Destroy(_itemTransform.gameObject);
                SpawnItem(itemSO.GetItemPrefab());
                _itemType = itemSO.GetItemName();
            }
        }
    }
    private void SpawnItem(GameObject itemPrefab)
    {
        GameObject obj = Instantiate(itemPrefab, _itemRendererContainer.transform);
        _itemTransform = obj.transform;
        _itemTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }
}
