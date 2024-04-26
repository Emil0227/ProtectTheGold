using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class quitGameButton : MonoBehaviour
{
    public void OnPointerClick()
    {
        Application.Quit();
    }
}

