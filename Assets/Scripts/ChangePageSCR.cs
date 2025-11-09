using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangePageSCR : MonoBehaviour
{
    [SerializeField] public string previousPage;
    [SerializeField] public string option1;
    [SerializeField] public string option2;
    [SerializeField] public string option3;
    [SerializeField] public string option4;
    [SerializeField] public string endingPage;

    public void gotoPreviousPage()
    {
        SceneManager.LoadScene(previousPage);
    }

    public void gotoOption1()
    {
        if(StaticData.bicyclePath && StaticData.firePath && StaticData.humanPath && StaticData.sunPath)
        {
            SceneManager.LoadScene(endingPage);
        }
        else
        {
            SceneManager.LoadScene(option1);
        }
    }

    public void gotoOption2()
    {
        SceneManager.LoadScene(option2);
    }

    public void gotoOption3()
    {
        SceneManager.LoadScene(option3);
    }

    public void gotoOption4()
    {
        if(SceneManager.GetActiveScene().name == "Ending")
        {
            StaticData.bicyclePath = false;
            StaticData.firePath = false;
            StaticData.humanPath = false;
            StaticData.sunPath = false;
        }

        SceneManager.LoadScene(option4);
    }

    public void gotoTitle()
    {
        // Reset all progress when returning to title
        StaticData.bicyclePath = false;
        StaticData.firePath = false;
        StaticData.humanPath = false;
        StaticData.sunPath = false;
        
        SceneManager.LoadScene("Title");
    }
}
