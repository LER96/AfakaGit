using System.Collections;
using System.Collections.Generic;

public class PlayerStatus 
{
    public float HP;
    public float maxHp;
    public float lightAttackDamage;
    public float heavyAttackDamage;

    public PlayerStatus(float hp, float max, float light, float heavy)
    {
        HP = hp;
        maxHp = max;
        lightAttackDamage = light;
        heavyAttackDamage = heavy;
    }
    public PlayerStatus()
    {

    }

    public void SetPlayerStatus(PlayerStatus p, float hpAdd, float damageAdd, float multi)
    {
        HP = p.HP + hpAdd * multi;
        maxHp = p.maxHp + hpAdd * multi;
        lightAttackDamage = p.lightAttackDamage + damageAdd * multi;
        heavyAttackDamage = p.heavyAttackDamage + damageAdd * multi;
    }
}
