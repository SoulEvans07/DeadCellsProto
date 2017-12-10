using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {
	void Start () {
		
	}
	
	void Update ()
	{
		if (transform.childCount == 0 && PlayerController.instance != null)
			StartCoroutine(StartEndGame());
	}

	IEnumerator StartEndGame()
	{
		yield return new WaitForSeconds(2f);
		PlayerController.instance.Die();
	}
}
