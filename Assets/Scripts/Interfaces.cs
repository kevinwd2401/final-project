using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamagable
{
    int Health { get; set;}
    void InflictDamage(int dmg, Transform bulletTrans);

}

public interface IBoid
{
    Vector3 GetVelocity();
    Vector3 GetPosition();
}