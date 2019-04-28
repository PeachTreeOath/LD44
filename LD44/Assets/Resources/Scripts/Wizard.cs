using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    const float WIZARD_MASS = 2;
    const float WIZARD_SPEED = 10f;
    const int WIZARD_GOLD_CAPACITY = 1;
    const float WIZARD_STUN_TIME = 4;
    const float WIZARD_GOLD_GRAB_RANGE = 25f;
    const float WIZARD_GOLD_GATHER_TIME = 3;
    
    public Wizard() : base(WIZARD_MASS, WIZARD_SPEED, WIZARD_GOLD_CAPACITY,
            WIZARD_STUN_TIME, WIZARD_GOLD_GRAB_RANGE, WIZARD_GOLD_GATHER_TIME)
    {
    }
}
