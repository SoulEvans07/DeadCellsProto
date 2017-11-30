using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HighScoreMenu : MonoBehaviour {

	public TextMeshProUGUI goldLabel;
	public TextMeshProUGUI highscoreLabel;
	public TextMeshProUGUI scoreLabel;

	void Start(){
		int gold = 0;
		int highscore = 0;

		if(PlayerPrefs.HasKey("player-gold"))
			gold = PlayerPrefs.GetInt ("player-gold");

		if(PlayerPrefs.HasKey("highscore"))
			highscore = PlayerPrefs.GetInt ("highscore");

		if (highscore < gold) {
			// change text to "new highscore"
			highscoreLabel.text = "New Highscore!";
			highscore = gold;
			PlayerPrefs.SetInt("highscore", gold);
		}

		// set player score
		goldLabel.text = gold.ToString();
		// set highscore
		scoreLabel.text = highscore.ToString();
	}

	public void MainMenu(){
		SceneManager.LoadScene ("Menu");
	}

	public void ClearHighscore(){
		PlayerPrefs.DeleteKey("highscore");
		scoreLabel.text = 0.ToString();
	}
}
