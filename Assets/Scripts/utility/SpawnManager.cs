using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	public GameObject player;

	private void Start() {
		if (Headless.instance != null)
			Headless.instance.transform.position = transform.position;
	}

	public void Respawn(){
		Instantiate (player, transform.position, transform.rotation);
	}
}
