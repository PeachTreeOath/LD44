using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : Enemy
{
    const int PEASANT_MASS = 2;
    const int PEASANT_SPEED = 3;
    const int PEASANT_GOLD_CAPACITY = 2;
    const int PEASANT_STUN_TIME = 2;
    const int PEASANT_GOLD_GRAB_RANGE = 1;
    const int PEASANT_GOLD_GATHER_TIME = 4;
    
    public Peasant() : base(PEASANT_MASS, PEASANT_SPEED, PEASANT_GOLD_CAPACITY,
            PEASANT_STUN_TIME, PEASANT_GOLD_GRAB_RANGE, PEASANT_GOLD_GATHER_TIME)
    {
    }
}
