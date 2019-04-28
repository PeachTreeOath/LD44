using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    const int WIZARD_MASS = 2;
    const int WIZARD_SPEED = 3;
    const int WIZARD_GOLD_CAPACITY = 2;
    const int WIZARD_STUN_TIME = 2;
    const int WIZARD_GOLD_GRAB_RANGE = 1;
    const int WIZARD_GOLD_GATHER_TIME = 4;
    
    public Wizard() : base(WIZARD_MASS, WIZARD_SPEED, WIZARD_GOLD_CAPACITY,
            WIZARD_STUN_TIME, WIZARD_GOLD_GRAB_RANGE, WIZARD_GOLD_GATHER_TIME)
    {
    }
}
