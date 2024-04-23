using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDemo : MonoBehaviour
{
    private IDataService dataManager = new DataManager();
    private PlayerStatsSave playerStats = new PlayerStatsSave();
    ItemStats itemStats = new ItemStats();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerStats.Items.Add(itemStats);
            dataManager.SaveData("/PlayerStats.json", playerStats, false);
        }
    }
}
