using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy
{
    const int KNIGHT_MASS = 2;
    const int KNIGHT_SPEED = 3;
    const int KNIGHT_GOLD_CAPACITY = 2;
    const int KNIGHT_STUN_TIME = 2;
    const int KNIGHT_GOLD_GRAB_RANGE = 1;
    const int KNIGHT_GOLD_GATHER_TIME = 4;
    
    public Knight() : base(KNIGHT_MASS, KNIGHT_SPEED, KNIGHT_GOLD_CAPACITY,
            KNIGHT_STUN_TIME, KNIGHT_GOLD_GRAB_RANGE, KNIGHT_GOLD_GATHER_TIME)
    {
    }
}
