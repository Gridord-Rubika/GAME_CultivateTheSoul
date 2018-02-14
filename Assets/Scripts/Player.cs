using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public enum CultivationTechniques
    {
        MORTAL = 0,
        EARTH,
        HEAVEN,
        IMMORTAL
    }

    public string id;
    public int soulLevel;
    public int soulForce;
    public int soulStone;
    public CultivationTechniques cultivationTechnique;
    public DateTime lastTimeFarmed;
}
