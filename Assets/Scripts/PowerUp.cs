using UnityEngine;
using System.Collections;

public class PowerUp : ScriptableObject
{
    public PowerUpType type;

    public int stackCount;

    public PowerUp(PowerUpType type)
    {
        this.type = type;
        this.stackCount = 1;
    }

    public int getStackCount()
    {
        return stackCount;
    }

    public PowerUpType getType()
    {
        return type;
    }

    public int incrementStackCount()
    {
        return ++stackCount;
    }
}
