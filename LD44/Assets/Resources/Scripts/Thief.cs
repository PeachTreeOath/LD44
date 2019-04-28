using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Enemy
{
    const float THIEF_MASS = 2;
    const float THIEF_SPEED = 25f;
    const int THIEF_GOLD_CAPACITY = 2;
    const float THIEF_STUN_TIME = 2;
    const float THIEF_GOLD_GRAB_RANGE = 10f;
    const float THIEF_GOLD_GATHER_TIME = 1;
    
    public Thief() : base(THIEF_MASS, THIEF_SPEED, THIEF_GOLD_CAPACITY,
            THIEF_STUN_TIME, THIEF_GOLD_GRAB_RANGE, THIEF_GOLD_GATHER_TIME)
    {
    }
}
