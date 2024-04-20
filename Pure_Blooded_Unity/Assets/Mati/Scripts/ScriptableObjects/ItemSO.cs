using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable objects/Item")]
public class ItemSO : ScriptableObject
{
    //General
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemImage;

    //States
    [SerializeField] private bool canInteract;

    //Public getters
    public string GetItemName() => itemName;
    public string GetItemDescription() => itemDescription;
    public Sprite GetItemImage() => itemImage;
    public bool GetInteractState() => canInteract;
}
