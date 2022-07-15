using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Test : MonoBehaviour {

	public const string IDLE	= "Anim_Idle";
	public const string RUN		= "Anim_Run";
	public const string ATTACK	= "Anim_Attack";
	public const string DAMAGE	= "Anim_Damage";
	public const string DEATH	= "Anim_Death";

	Animation anim;

	void Start () {

		anim = GetComponent<Animation>();
		
	}
	
	public void IdleAni (){
		anim.CrossFade (IDLE);
	}

	public void RunAni (){
		anim.CrossFade (RUN);
	}

	public void AttackAni (){
		anim.CrossFade (ATTACK);
	}

	public void DamageAni (){
		anim.CrossFade (DAMAGE);
	}

	public void DeathAni (){
		anim.CrossFade (DEATH);
	}

}
