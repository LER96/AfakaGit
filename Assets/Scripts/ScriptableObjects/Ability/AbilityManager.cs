using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
   public List<Ability> abilities;

    private float _cooldownTime;
    private float _activeTime;

    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState _state = AbilityState.ready;

    private void Update()
    {
        switch (_state)
        {
            case AbilityState.ready:
                break;
            case AbilityState.active:
                break;
            case AbilityState.cooldown:
                break;
            default:
                break;
        }
    }
}
