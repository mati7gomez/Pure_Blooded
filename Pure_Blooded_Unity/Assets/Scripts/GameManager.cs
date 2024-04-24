using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool _gamePaused;
    public static bool GamePaused => _gamePaused;
    public void SetGamePausedState(bool value)
    {
        _gamePaused = value;
    }


}
