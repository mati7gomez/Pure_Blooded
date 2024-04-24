using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private IDataService _dataManager;

    private void Awake()
    {
        _dataManager = new DataManager();
    }

    public void SaveGame()
    {
        //Logica de guardado
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        playerStats.Items.Add(itemStats);
    //        dataManager.SaveData("/PlayerStats.json", playerStats, false);
    //    }
    //}
}
