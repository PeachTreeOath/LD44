using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Enemy
{
    const int THIEF_MASS = 2;
    const int THIEF_SPEED = 3;
    const int THIEF_GOLD_CAPACITY = 2;
    const int THIEF_STUN_TIME = 2;
    const int THIEF_GOLD_GRAB_RANGE = 1;
    const int THIEF_GOLD_GATHER_TIME = 4;
    
    public Thief() : base(THIEF_MASS, THIEF_SPEED, THIEF_GOLD_CAPACITY,
            THIEF_STUN_TIME, THIEF_GOLD_GRAB_RANGE, THIEF_GOLD_GATHER_TIME)
    {
    }
}
