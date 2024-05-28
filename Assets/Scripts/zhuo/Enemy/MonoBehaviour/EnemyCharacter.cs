using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public Room generateRoom;
    protected override void OnDisable()
    {
        base.OnDisable();
        if (gameObject.tag == "Enemy" && generateRoom != null)
            generateRoom.killThisRoomEnemy(this.gameObject);
    }
}
