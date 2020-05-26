using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float Speed; //移动速度
    public int attack;
    public int Level;
    public int Attack { get { return attack * Level; } } //攻击力
    public bool IsBoom;
    public Animator Anima;

    public void boom()
    {
        Anima.SetTrigger("IsExplode");
        IsBoom = true;
        Destroy(gameObject, 1f);
    }

    public virtual void Move()
    {
       

    }
}
