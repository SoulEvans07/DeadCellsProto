using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	public GameObject player;

	public void Respawn(){
		Instantiate (player, transform.position, transform.rotation);
	}
}
