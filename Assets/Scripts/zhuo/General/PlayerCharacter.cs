using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public CharacterConfig config;

    public virtual void HandleConfigUpdate()
    {
        maxHp = config.maxHp;

    }
}
