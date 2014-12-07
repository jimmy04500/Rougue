﻿using UnityEngine;
using System.Collections;

public class Rage : TimedEffect {

    Status tStat;
	
	protected override void ApplyEffect (){
        tStat = target.GetComponent<Status>();

		handleTargetNull ();
        tStat.setSpeedx(tStat.getSpeedx() + 0.6f);
		tStat.damagex+=3f;
		tStat.modAttackSpeed+=1.2f;
		tStat.modHealthRegen+=1f;
		tStat.isRaged = true;
        tStat.refreshStats();
	}
	
	protected override void EndEffect (){
		handleTargetNull ();
        tStat.setSpeedx(tStat.getSpeedx() - 0.6f);
		tStat.damagex-=3f;
		tStat.modAttackSpeed-=1.2f;
		tStat.modHealthRegen-=1f;
		tStat.rage = 0;
		tStat.isRaged = false;
        tStat.refreshStats();
		base.EndEffect ();
	}
	
}
