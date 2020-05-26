using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : MonoBehaviour {
    Animator Anima;

	void Start () {
        Anima = GetComponent<Animator>();
	}
	
    public void BiHit()
    {
        Anima.SetTrigger("IsDamage");
    }

    public void Die()
    {
        GameMode.GM.GameState = false;
        GameMode.GM.Lost = true;
        Anima.SetBool("IsDead",true);
    }

}
