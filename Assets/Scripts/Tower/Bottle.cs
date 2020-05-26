using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bottle : Tower
{
    public override void Start()
    {
        base.Start();
        Money = 100;
        Plan.GetComponent<Plan>().Load(this);
        AttackCD = map.tower["Bottle"].AttackCD;
        Scope = map.tower["Bottle"].Scope;
        LevelUp1 = map.tower["Bottle"].LevelUp1;
        LevelUp2 = map.tower["Bottle"].LevelUp2;
        Dele1 = map.tower["Bottle"].Dele1;
        Dele2 = map.tower["Bottle"].Dele2;
        Dele3 = map.tower["Bottle"].Dele3;

    }
    
  
}
