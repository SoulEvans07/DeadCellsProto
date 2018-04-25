using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSword : Weapon {
	
	private void init() {
		equipmentName = "Blood Sword";
		spriteName = "BloodSword";
		attackAnim = "AtkSaberA";
        
		description = "";
		dps = 13;
		extraInfo = "Applies bleed effect on enemies.";
	}
	
	public BloodSword() {
		init();
	}

	private void Start() {
		init();
	}
	
	public override void Use() {
		if (attackCooldownValue.Equals(0f)) {
			attackCooldownValue = attackCooldown;
            
			GameObject fx = Instantiate(attackFx, transform.position, transform.rotation);
			GameObject ef = Instantiate(effect);
			ef.SetActive(false);
			fx.GetComponent<AttackFx>().dot = ef.GetComponent<DamageOverTime>();
			fx.transform.parent = transform;
            
			Destroy(fx, 1);
		}
	}
}
