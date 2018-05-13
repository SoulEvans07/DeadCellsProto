using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour {
	
	public GameObject popUp;
	public Vector3 popOffset = new Vector3(0, -0.75f, -4.5f);
	public String nextLevel;
	[ShowOnly] public GameObject popUpInst;
	[ShowOnly] public Collider2D inside = null;

	void NextLevel() {
		if (nextLevel.Equals("HighScore")) {
			PlayerPrefs.SetInt("player-gold", Headless.instance.gold);
			if (AudioManager.instance != null) {
				AudioManager.instance.Stop("ActionIntro");
				AudioManager.instance.Stop("ActionLoop");
			}
		}
		SceneManager.LoadScene(nextLevel);
	}
	
	protected void OnTriggerEnter2D(Collider2D other) {
		if(!other.CompareTag("Player") || Headless.instance.inventory.watching || CanvasManager.instance == null || popUpInst != null)
			return;
		inside = other;
		popUpInst = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
		Headless.instance.inventory.watching = true;
		popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
	}

	protected void Update() {
		if (inside != null && Input.GetButtonDown(InputManager.INTERACT)) {
			Headless.instance.inventory.watching = false;
			Destroy(popUpInst);
			popUpInst = null;
			inside = null;
			NextLevel();
		}
	}

	protected void OnTriggerStay2D(Collider2D other) {
		if(!other.CompareTag("Player") || CanvasManager.instance == null)
			return;
		if (popUpInst == null && !Headless.instance.inventory.watching) {
			inside = other;
			popUpInst = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
			Headless.instance.inventory.watching = true;
		}
		if (popUpInst != null) {
			popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
		}
	}

	protected void OnTriggerExit2D(Collider2D other) {
		if(!other.CompareTag("Player") || popUpInst == null)
			return;
		inside = null;
		Headless.instance.inventory.watching = false;
		Destroy(popUpInst);
		popUpInst = null;
	}
}
