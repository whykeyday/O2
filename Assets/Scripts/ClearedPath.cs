using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearedPath : MonoBehaviour
{
    public void pathCompleted()
    {
        if(SceneManager.GetActiveScene().name == "Bicycle3")
        {
            StaticData.bicyclePath = true;
            Debug.Log("cleared bicycle path");
        }
        else if(SceneManager.GetActiveScene().name == "Fire3")
        {
            StaticData.firePath = true;
            Debug.Log("cleared fire path");
        }
        else if(SceneManager.GetActiveScene().name == "Human4")
        {
            StaticData.humanPath = true;
            Debug.Log("cleared human path");
        }
        else if(SceneManager.GetActiveScene().name == "Sun2")
        {
            StaticData.sunPath = true;
            Debug.Log("cleared sun path");
        }
    }
}
