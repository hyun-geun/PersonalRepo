using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//hp, op, isDead / OnDamage, OnDead
public abstract class LifeEntity : MonoBehaviour
{
    public int healthPoint
    {
        get;
        protected set;
    }
    public int offencePoint
    {
        get;
        protected set;
    }
    public bool isDead
    {
        get;
        protected set;
    }

    public abstract void OnDamage(int damage);
    public abstract void OnDead();
}