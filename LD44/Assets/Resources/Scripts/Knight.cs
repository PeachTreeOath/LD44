using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy
{
    const float KNIGHT_MASS = 4;
    const float KNIGHT_SPEED = 10f;
    const int KNIGHT_GOLD_CAPACITY = 1;
    const float KNIGHT_STUN_TIME = 2;
    const float KNIGHT_GOLD_GRAB_RANGE = 10f;
    const float KNIGHT_GOLD_GATHER_TIME = 3;
    
    public Knight() : base(KNIGHT_MASS, KNIGHT_SPEED, KNIGHT_GOLD_CAPACITY,
            KNIGHT_STUN_TIME, KNIGHT_GOLD_GRAB_RANGE, KNIGHT_GOLD_GATHER_TIME)
    {
    }
}
