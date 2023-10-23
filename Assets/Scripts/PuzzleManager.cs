using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour {

	public GameObject piece; //Prefab
	public GameManager gameManager;

	public int nPiecesX;
	public int nPiecesY;

	public int piecesInPlace = 0;

	public Texture defaultTexture; //Default texture in case no Camera is found

	public Camera backgroundCam;

	private GameObject[,] puzzle;
	private GameObject[,] puzzleBackground;

	public const int PIECES_EASY = 2, PIECES_MEDIUM = 4, PIECES_HARD = 6; //Number of pieces in the Y axis 

	int count;
	public float bgPiecesAnimTime = 0.7f;

	public float CreatePuzzle(int difficulty){
		
		nPiecesY = GetYPieces (difficulty);
		nPiecesX = GetSquareDivision (nPiecesY);

		puzzle = new GameObject[nPiecesX, nPiecesY];
		puzzleBackground = new GameObject[nPiecesX, nPiecesY];

		CreatePieces ();
		CreateBackgroundPieces ();

		float time = (1-Mathf.Pow(bgPiecesAnimTime, nPiecesX*nPiecesY+1)) / (1 - bgPiecesAnimTime) - 1; //sum = (1 - r^(n+1))/(1-r)
		return time;
	}

	private void CreatePieces(){

		for (int x = 0; x < nPiecesX; x++) {
			for (int y = 0; y < nPiecesY; y++) {

				int top, right, bottom, left;

				if (x == 0) left = Piece.FLAT;
				else left = OppositeBorder (puzzle [x - 1,y].GetComponent<Piece> ().right);

				if (x == nPiecesX - 1) right = Piece.FLAT;
				else right = RandomBorder ();

				if (y == 0) bottom = Piece.FLAT;
				else bottom = OppositeBorder (puzzle [x,y-1].GetComponent<Piece> ().top);

				if (y == nPiecesY - 1) top = Piece.FLAT;
				else top = RandomBorder ();

				puzzle[x,y] = CreatePiece (x, y, top, right, bottom, left);
				puzzle [x, y].SetActive (false);
			}
		}
	}
 
	public void ActivatePieces(){
		for (int x = 0; x < nPiecesX; x++) {
			for (int y = 0; y < nPiecesY; y++) {
				puzzle [x, y].SetActive (true);
			}
		}
	}



	private void CreateBackgroundPieces(){
		count = 0;
		Invoke ("CreateBackgrounPiece", 0f);
	}

	private void CreateBackgrounPiece(){

		if (count == nPiecesX * nPiecesY) {return;}

		int x, y;

		do {
			x = Random.Range (0, nPiecesX);
			y = Random.Range (0, nPiecesY);
		} while (puzzleBackground [x,y] != null);

		Piece piece = puzzle[x,y].GetComponent<Piece> ();
		puzzleBackground[x,y] = CreateBackgroundPiece(x,y, piece.top, piece.right, piece.bottom, piece.left);

		count++;
		Invoke ("CreateBackgrounPiece", Mathf.Pow(bgPiecesAnimTime, count));
	}

	private GameObject CreatePiece(int posX, int posY, int top, int right, int bottom, int left){
		Vector3 randomPosition = Random.onUnitSphere * 30;
		GameObject p = (GameObject)Instantiate (piece, randomPosition, Random.rotation);
		p.transform.parent = transform;
		p.GetComponent<Piece> ().SetPieceParams (nPiecesX, nPiecesY, posX +1 , posY+1, top, right, bottom, left, false, this);
		p.name = "Piece(" + posX + ", " + posY + ")";
		return p;
	}

	private GameObject CreateBackgroundPiece(int posX, int posY, int top, int right, int bottom, int left){

		float scHeight = backgroundCam.orthographicSize * 2f;
		float scWidth = scHeight * backgroundCam.aspect;

		float x = (0 - scWidth / 2) + (scWidth / (nPiecesX * 2)) + (posX * scWidth / nPiecesX);
		float y = (0 - scHeight / 2) + (scHeight / (nPiecesY * 2)) + (posY * scHeight / nPiecesY);
		float z = 10;

		GameObject p = (GameObject)Instantiate (piece, new Vector3(), backgroundCam.transform.rotation);
		p.transform.parent = backgroundCam.transform;
		p.transform.localPosition = new Vector3 (x, y, z);
		p.GetComponent<Piece> ().SetPieceParams (nPiecesX, nPiecesY, posX +1 , posY+1, top, right, bottom, left, true, this);
		p.name = "Piece(" + posX + ", " + posY + ")";
		p.layer = LayerMask.NameToLayer("Background");
		return p;
	}

	private int OppositeBorder(int border){
		if (border == Piece.MALE)
			return Piece.FEMALE;
		else if (border == Piece.FEMALE)
			return Piece.MALE;
		else
			return Piece.FLAT;
	}

	private int RandomBorder(){
		return Random.Range (1, 3);
	}
		
	private int GetYPieces(int difficulty){
		switch (difficulty) {
			case GameManager.EASY: return PIECES_EASY;
			case GameManager.MEDIUM: return PIECES_MEDIUM;
			case GameManager.HARD: return PIECES_HARD;
			default: return PIECES_EASY;
		}
	}

	private int GetSquareDivision(int y){
		float scHeight = backgroundCam.orthographicSize * 2f;
		float scWidth = scHeight * backgroundCam.aspect;

		return Mathf.RoundToInt(y * scWidth / scHeight);
	}



	public void PieceInPlace(Piece piece){
		piecesInPlace++;

		Destroy (puzzleBackground [piece.posX - 1, piece.posY - 1]);
		Destroy (puzzle [piece.posX - 1, piece.posY - 1]);

		if (piecesInPlace == (nPiecesX * nPiecesY))
			gameManager.EndGame ();
	}

	public void ClearPuzzle(){

		for (int x = 0; x < nPiecesX; x++) {
			for (int y = 0; y < nPiecesY; y++) {
				Destroy (puzzleBackground [x, y]);
				Destroy (puzzle [x, y]);
			}
		}

		piecesInPlace = 0;
		nPiecesX = 0;
		nPiecesY = 0;
	}
}
