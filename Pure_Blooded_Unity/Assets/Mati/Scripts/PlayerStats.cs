using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    //Stats
    private bool isRunning;
    private bool isBusy;

    public bool IsRunning
    {
        get { return isRunning; }
    }
    public void SetRunningState(bool value)
    {
        isRunning = value;
    }
}
