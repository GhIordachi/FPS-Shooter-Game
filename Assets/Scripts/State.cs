using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    //The base class for all states
    public virtual State Tick(ZombieManager zombieManager)
    {
        return this;
    }
}
