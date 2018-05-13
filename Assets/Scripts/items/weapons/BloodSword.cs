using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSword : Weapon {
	
	[ShowOnly] public float offsetX = 0.2f;
	[ShowOnly] public float offsetY = 0f;
	
	private void init() {
		equipmentName = "Blood Sword";
		spriteName = "BloodSword";
		attackAnim = "AtkSaberA";
        
		description = "";
		dps = 109;
		extraInfo = "Applies bleed effect on enemies.";
	}
	
	public BloodSword() {
		init();
	}

	private void Start() {
		init();
	}
	
	public override bool Use() {
		if (attackCooldownValue > 0f)
			return false;
			
		attackCooldownValue = attackCooldown;
		
		Vector3 offset = new Vector3(Headless.instance.transform.localScale.x * offsetX, offsetY, 0);
		GameObject fx = Instantiate(attackFx, transform.position + offset, transform.rotation);
		GameObject ef = Instantiate(effect);
		ef.SetActive(false);
		fx.GetComponent<AttackFx>().dot = ef.GetComponent<DamageOverTime>();
		fx.transform.parent = transform;
		Vector3 preScale = fx.transform.localScale;
		fx.transform.localScale = new Vector3(Headless.instance.transform.localScale.x * preScale.x, preScale.y, preScale.z);
		fx.GetComponent<AttackFx>().damage =  (int) (dps * attackCooldown);
		
		Destroy(fx, 1);
		
		return true;
	}
}
