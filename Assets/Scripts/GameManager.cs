using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public static GameManager instance;

    float timeBeforeNextMining = 0;

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
    }

    private void Update()
    {
        if(timeBeforeNextMining - Time.deltaTime <= 0)
        {
            timeBeforeNextMining = 0;
        }
        else
        {
            timeBeforeNextMining -= Time.deltaTime;
        }
    }

    public void TryMining()
    {
        if(timeBeforeNextMining == 0)
        {

        }
    }
}
