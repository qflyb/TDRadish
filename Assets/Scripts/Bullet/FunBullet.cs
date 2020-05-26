using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunBullet : Bullet {

    public Vector3 vect;

    public void Start()
    {
        Speed = 3f;
        attack = 50;
        Anima = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        transform.Rotate(transform.forward*360*Time.deltaTime);
    }

    public override void Move()
    {
        if (IsBoom)
            return;

        if (vect != null)
        {
            transform.Translate(vect * Speed * Time.deltaTime, Space.World);
            GameObject[] monsters = GameMode.GM.monsters;

            foreach(var mons in monsters)
            {
                if(Vector3.Distance(transform.position,mons.transform.position)<0.3f)
                {
                    if (!IsBoom)
                    {
                        mons.GetComponent<Monster>().BeHit(Attack);
                        boom();
                    }
                }
            }

        }
        if (!IsBoom && !GameMode.GM.MapRect.Contains(transform.position))
            boom();

    }
}
