using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour {

	public GameObject gameScreen;
	public Text time, bestTime, endTime, endBest;
	public GameObject instructions, endGame;

	public void SetGameTime(int seconds){
		time.text = GetTimeText (seconds);
	}

	public void SetBestTime(int seconds){
		bestTime.text = GetTimeText (seconds);
	}

	public void OpenGameScreen(int difficulty){
		gameScreen.SetActive (true);
		instructions.SetActive (true);
		endGame.SetActive (false);
		SetBestTime (PlayerPrefs.GetInt ("Best" + difficulty, -1));
		Invoke ("CloseInstructions", 5.0f);
	}

	public void CloseInstructions(){
		instructions.SetActive (false);
	}

	public void OpenEndGame(int time, int oldBest){
		endGame.SetActive (true);
		endTime.text = GetTimeText (time);

		if (oldBest == -1 ||time < oldBest)
			endBest.text = "NEW RECORD";
		else
			endBest.text = "RECORD " + GetTimeText (oldBest);
	}

	public void CloseGameScreen(){
		gameScreen.SetActive (false);
		instructions.SetActive (false);
		endGame.SetActive (false);
	}

	private string GetTimeText(int seconds){
		if (seconds == -1)
			return "--:--";
		else {
			int min = seconds / 60;
			int sec = seconds % 60;

			return min.ToString ("00") + ":" + sec.ToString ("00");
		}
	}
}
