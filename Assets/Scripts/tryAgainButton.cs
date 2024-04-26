using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class tryAgainButton : MonoBehaviour
{
    public void OnPointerClick()
    {
        levelManager.SceneIndex = 0;
        levelManager.CumulativeGoldCount = 0;
        SceneManager.LoadScene(0);
    }
}
