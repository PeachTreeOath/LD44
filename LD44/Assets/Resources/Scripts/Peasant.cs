using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : Enemy
{
    const float PEASANT_MASS = 2;
    const float PEASANT_SPEED = 10f;
    const int PEASANT_GOLD_CAPACITY = 1;
    const float PEASANT_STUN_TIME = 2;
    const float PEASANT_GOLD_GRAB_RANGE = 10f;
    const float PEASANT_GOLD_GATHER_TIME = 3;
    
    public Peasant() : base(PEASANT_MASS, PEASANT_SPEED, PEASANT_GOLD_CAPACITY,
            PEASANT_STUN_TIME, PEASANT_GOLD_GRAB_RANGE, PEASANT_GOLD_GATHER_TIME)
    {
    }
}
