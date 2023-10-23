using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public const int NOT_STARTED = 0, EASY = 1, MEDIUM = 2, HARD = 3;

	public TitleScreen titleScreen;
	public GameScreen gameScreen;
	public PuzzleManager puzzleManager;

	private int gameState = NOT_STARTED; //0 = Not started; 1 = Easy; 2 = Medium; 3 = Hard

	public float time = 0;

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		titleScreen.OpenTitleScreen ();
	}

	void Update()
	{
		if (gameState != NOT_STARTED) {
			time += Time.deltaTime;
			gameScreen.SetGameTime ((int) time);
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (gameState == NOT_STARTED)
				Application.Quit ();
			else
				CancelGame ();

		}

	}
		
	public void StartGame(int difficulty){
		if (gameState == NOT_STARTED) {
			StartCoroutine(StartGameCoroutine (difficulty));
		}
	}

	public IEnumerator StartGameCoroutine(int difficulty){
		
		float animDuration = titleScreen.CloseTitleScreen ();
		yield return new WaitForSeconds(animDuration + 0.2f); //Wait for the title screen animation
		titleScreen.gameObject.SetActive (false);

		animDuration = puzzleManager.CreatePuzzle (difficulty);
		yield return new WaitForSeconds(animDuration + 0.5f); //Wait for the puzzle animation
		puzzleManager.ActivatePieces();
		gameScreen.OpenGameScreen (difficulty);

		time = 0;
		gameState = difficulty;
	}

	public void EndGame (){
		StartCoroutine(EndGameCourutine ());
	}

	public IEnumerator EndGameCourutine(){
		gameState = NOT_STARTED;
		int endTime = (int)time;

		int previousBest = PlayerPrefs.GetInt ("Best" + gameState, -1);

		if (previousBest == -1 || endTime < previousBest)
			PlayerPrefs.SetInt ("Best" + gameState, endTime);

		yield return new WaitForSeconds(2.0f);
		gameScreen.OpenEndGame (endTime, previousBest);
		yield return new WaitForSeconds(6.0f);
		ClearGame ();
		EndGameScreenChange ();
	}

	public void CancelGame(){
		ClearGame ();
		EndGameScreenChange ();
	}

	public void ClearGame(){
		puzzleManager.ClearPuzzle ();
		gameState = NOT_STARTED;
		time = 0;
	}

	public void EndGameScreenChange(){
		gameScreen.CloseGameScreen ();
		titleScreen.OpenTitleScreen ();
	}
}
