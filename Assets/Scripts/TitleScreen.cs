using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SmartLocalization;

public class TitleScreen : MonoBehaviour {

	public Text easyBest, mediumBest, hardBest, easy, medium, hard;
	public Button easyButton, mediumButton, hardButton;

	public GameManager gameManager;

	public AnimationClip titleScreenIn, titleScreenOut;

	public float OpenTitleScreen(){
		LanguageManager.Instance.ChangeLanguage ("es");

		easy.text = LanguageManager.Instance.GetTextValue ("Easy");
		medium.text = LanguageManager.Instance.GetTextValue ("Medium");
		hard.text = LanguageManager.Instance.GetTextValue ("Hard");

		gameObject.SetActive (true);
		SetBestScoresText ();
		GetComponent<Animator> ().Play (titleScreenIn.name);
		easyButton.interactable = true;
		mediumButton.interactable = true;
		hardButton.interactable = true;
		return titleScreenIn.length;
	}

	//Closing the title screen
	public float CloseTitleScreen(){
		easyButton.interactable = false;
		mediumButton.interactable = false;
		hardButton.interactable = false;
		GetComponent<Animator> ().Play (titleScreenOut.name);
		return titleScreenOut.length;
	}

	private void SetBestScoresText(){
		easyBest.text = GetBestTimeText(PlayerPrefs.GetInt ("Best" + GameManager.EASY, -1));
		mediumBest.text = GetBestTimeText(PlayerPrefs.GetInt ("Best" + GameManager.MEDIUM, -1));
		hardBest.text = GetBestTimeText(PlayerPrefs.GetInt ("Best" + GameManager.HARD, -1));
	}

	private string GetBestTimeText(int seconds){
		if (seconds == -1)
			return "--:--";
		else {
			int min = seconds / 60;
			int sec = seconds % 60;

			return min.ToString ("00") + ":" + sec.ToString ("00");
		}
	}

	private IEnumerator WaitForAnimation ( Animation animation )
	{
		do
		{
			yield return null;
		} while ( animation.isPlaying );
	}
}
