using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitScript : MonoBehaviour {

    public void QuitGameScene()
    {
        GameManager.instance.TrySendLatestClicks();
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        GameManager.instance.TrySendLatestClicks();
    }
}
