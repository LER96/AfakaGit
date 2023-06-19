using System.Collections;
using System.Collections.Generic;

public class PlayerStatus 
{
    public float _movementSpeed;
    public float _dashCD;
    public float _heavyAttackCD;

    public PlayerStatus(float movementSpeed, float dashCD, float heavyAttackCD)
    {
        _movementSpeed = movementSpeed;
        _dashCD = dashCD;
        _heavyAttackCD = heavyAttackCD;
    }

    public PlayerStatus()
    {

    }

    public void SetPlayerStatus(PlayerStatus p, float speedAdd, float dashCdReduce, float heavyCdReduce, float multi)
    {
        _movementSpeed = p._movementSpeed + speedAdd * multi;
        _dashCD = p._dashCD - dashCdReduce * multi;
        _heavyAttackCD = p._heavyAttackCD - heavyCdReduce * multi;
    }
}
