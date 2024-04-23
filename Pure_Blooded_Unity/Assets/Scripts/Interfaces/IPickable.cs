using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    void Pick();
    void ToggleInteractCanvas(bool enabled);
}
