using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public static GameManager instance;

    float timeBeforeNextUpdate = 0;

    private int _numberOfClickAfterLastUpdate;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        _numberOfClickAfterLastUpdate = 0;
        NetworkManager.instance.UpdatePlayer();
    }

    private void Update()
    {
        if(timeBeforeNextUpdate - Time.deltaTime <= 0)
        {
            timeBeforeNextUpdate = 0;
        }
        else
        {
            timeBeforeNextUpdate -= Time.deltaTime;
        }
    }

    public void TryMining()
    {
        if(timeBeforeNextUpdate == 0)
        {

        }
    }

    public void TrySendLatestClicks()
    {

    }
}
