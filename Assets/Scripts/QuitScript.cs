using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitScript : MonoBehaviour {
    
    public void QuitGameScene()
    {
        GameManager.instance.CheckClicks();
        GameManager.instance.gameCanvas.SetActive(false);
        LoginManager.instance.loginCanvas.SetActive(true);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.CheckClicks();
        }
    }
}
