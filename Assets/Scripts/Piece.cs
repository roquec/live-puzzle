using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {

	public const int FLAT = 0;
	public const int MALE = 1;
	public const int FEMALE = 2;

	//[HideInInspector]
	public int nPiecesX, nPiecesY, posX, posY, top, right, bottom, left;
	private bool isBackground;

	public PuzzleManager puzzleManager;

	public Shader puzzleShader; //Shader for the borders of the pieces

	public Texture defaultTexture; //Default texture in case no Camera is found

	public Texture blackTexture;

	public bool border = false;

	public Texture topMaleBorder, topFemaleBorder, topFlatBorder, rightMaleBorder, rightFemaleBorder, rightFlatBorder; //Textures use to alpha mask in order to get the puzzle piece shape

	public Texture topMale, topFemale, topFlat, rightMale, rightFemale, rightFlat;

	private float textAspect;

	public float correctRelativePosX, correctRelativePosY;


	public float rotation;
	/*
	[Header("TESTING VARIABLES")]
	public int numeroPiezasX = 2;
	public int numeroPiezasY = 2;
	public int posicionX = 1;
	public int posicionY = 1;
	public int topShape = 0;
	public int rightShape = 0;
	public int bottomShape = 0;
	public int leftShape = 0;*/

	// Use this for initialization
	void Start () {
		//SetPieceParams (defaultTexture, numeroPiezasX, numeroPiezasY, posicionX, posicionY, topShape, rightShape, bottomShape, leftShape);
		rotation = Random.Range(0f,360f);
	}

	public bool verbose = false;

	// Update is called once per frame
	void Update () {
		if (!isBackground) {

			//transform.eulerAngles = new Vector3 (Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, rotation);
			transform.rotation = Camera.main.transform.rotation;
			//transform.Rotate (new Vector3 (0, 0, rotation - Camera.main.transform.eulerAngles.z), Space.Self);
			//Vector3 deltaRotation = Camera.main.transform.eulerAngles - previousRotation;
			Vector3 position = transform.position;

			Vector3 vectorA = Vector3.ProjectOnPlane ((position.x == 0 && position.z == 0)? Vector3.right : Vector3.up, position);
			Vector3 vectorB = Vector3.ProjectOnPlane (transform.up, position);
			Vector3 referenceRight = Vector3.Cross(Vector3.up, vectorA);
			float sign = Mathf.Sign(Vector3.Dot(vectorB, referenceRight));
			sign = position.y > 0 ? sign : -sign;
			float angle = Vector3.Angle(vectorA, vectorB);
			float finalAngle = sign * angle;

			transform.Rotate (new Vector3 (0, 0, rotation - finalAngle), Space.Self);
			if (verbose) {
				Debug.Log ("Camera rotation: " + Camera.main.transform.eulerAngles);
				Debug.Log ("Piece position: " + position);
				Debug.Log ("VectorA : " + vectorA);
				Debug.Log ("VectorB : " + vectorB);
				Debug.Log ("Angle: " + angle);
				Debug.Log ("Final angle: " + finalAngle);
				Debug.Log ("Final rotation: " + transform.eulerAngles);
			}
				
			if (isInPosition ())
				puzzleManager.PieceInPlace (this);
		}
		//transform.Rotate (new Vector3 (Camera.main.transform.rotation.x - transform.rotation.x, Camera.main.transform.rotation.x - transform.rotation.y, 0));
	}


	private bool isInPosition(){
		float x = Vector3.Dot(transform.position, Camera.main.transform.right.normalized);
		float y = Vector3.Dot(transform.position, Camera.main.transform.up.normalized);
		float z = Vector3.Dot(transform.position, Camera.main.transform.forward.normalized);
		float rot = Camera.main.transform.eulerAngles.z;

		if(z > 0)
			if (Mathf.Abs (x - correctRelativePosX) < (transform.localScale.x * 0.15f))
				if (Mathf.Abs (y - correctRelativePosY) < (transform.localScale.y * 0.15f))
					if (Mathf.Abs (rot - transform.eulerAngles.z) < 15f)
						return true;

		return false;
	}


	public void SetPieceParams(int nPiecesX, int nPiecesY, int posX, int posY, int top, int right, int bottom, int left, bool isBackground, PuzzleManager puzzleManager){

		this.nPiecesX = nPiecesX;
		this.nPiecesY = nPiecesY;
		this.posX = posX;
		this.posY = posY;
		this.top = top;
		this.right = right;
		this.bottom = bottom;
		this.left = left;
		this.isBackground = isBackground;
		this.puzzleManager = puzzleManager;

		//NOT REVERSE
		float scHeight = Camera.main.orthographicSize * 2f;
		float scWidth = scHeight * Camera.main.aspect;
		float scAspect = Camera.main.aspect;

		correctRelativePosX = (0 - scWidth / 2) + (scWidth / (nPiecesX * 2)) + ((posX-1) * scWidth / nPiecesX);
		correctRelativePosY = (0 - scHeight / 2) + (scHeight / (nPiecesY * 2)) + ((posY-1) * scHeight / nPiecesY);

		//REVERSED because the camera input is rotated
		/*float scWidth = Camera.main.orthographicSize * 2f;
		float scHeight = scWidth * Camera.main.aspect;
		float scAspect = scWidth / scHeight;*/

		Texture mainText = blackTexture;
		if(!isBackground) mainText = DeviceCameraManager.getCamTexture ();
		textAspect = mainText.width / (float)mainText.height;

		//Tamaño del Quad según la cantidad de piezas en que se divide la pantalla
		transform.localScale = new Vector3(scWidth/nPiecesX *2, scHeight/nPiecesY *2,  1);

		Vector2 tiling = GetTiling (scAspect, scHeight, scWidth);
		Vector2 offset = GetOffset (scAspect, scHeight, scWidth, tiling.x, tiling.y);

		Texture topText, rightText, bottomText, leftText;

		if(isBackground){
			if(top == MALE) topText = topMale; else if (top == FEMALE) topText = topFemale; else topText = topFlat;
			if(right == MALE) rightText = rightMale; else if (right == FEMALE) rightText = rightFemale; else rightText = rightFlat;
			if(bottom == MALE) bottomText = topMale; else if (bottom == FEMALE) bottomText = topFemale; else bottomText = topFlat;
			if(left == MALE) leftText = rightMale; else if (left == FEMALE) leftText = rightFemale; else leftText = rightFlat;
		} else {
			if(top == MALE) topText = topMaleBorder; else if (top == FEMALE) topText = topFemaleBorder; else topText = topFlatBorder;
			if(right == MALE) rightText = rightMaleBorder; else if (right == FEMALE) rightText = rightFemaleBorder; else rightText = rightFlatBorder;
			if(bottom == MALE) bottomText = topMaleBorder; else if (bottom == FEMALE) bottomText = topFemaleBorder; else bottomText = topFlatBorder;
			if(left == MALE) leftText = rightMaleBorder; else if (left == FEMALE) leftText = rightFemaleBorder; else leftText = rightFlatBorder;
		}

		SetPieceMaterial (mainText, tiling, offset, topText, rightText, bottomText, leftText);
	}


	private void SetPieceMaterial(Texture mainText, Vector2 mainTiling, Vector2 mainOffset, Texture top, Texture right, Texture bottom, Texture left){

		this.GetComponent<Renderer> ().material = new Material (puzzleShader);

		//Main
		this.GetComponent<Renderer> ().material.SetTexture("_MainTex", mainText);
		this.GetComponent<Renderer> ().material.SetTextureScale ("_MainTex", mainTiling);
		this.GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", mainOffset);
		//Top
		this.GetComponent<Renderer> ().material.SetTexture("_Top", top);
		this.GetComponent<Renderer> ().material.SetTextureScale ("_Top", new Vector2 (1, 2));
		this.GetComponent<Renderer> ().material.SetTextureOffset ("_Top", new Vector2 (0, -1));
		//Right
		this.GetComponent<Renderer> ().material.SetTexture("_Right", right);
		this.GetComponent<Renderer> ().material.SetTextureScale ("_Right", new Vector2 (2, 1));
		this.GetComponent<Renderer> ().material.SetTextureOffset ("_Right", new Vector2 (-1, 0));
		//Bottom
		this.GetComponent<Renderer> ().material.SetTexture("_Bottom", bottom);
		this.GetComponent<Renderer> ().material.SetTextureScale ("_Bottom", new Vector2 (1, -2));
		this.GetComponent<Renderer> ().material.SetTextureOffset ("_Bottom", new Vector2 (0, 1));
		//Left
		this.GetComponent<Renderer> ().material.SetTexture("_Left", left);
		this.GetComponent<Renderer> ().material.SetTextureScale ("_Left", new Vector2 (-2, 1));
		this.GetComponent<Renderer> ().material.SetTextureOffset ("_Left", new Vector2 (1, 0));

	}

	//Calculate the tiling for the main texture
	private Vector2 GetTiling(float scAspect, float scHeight, float scWidth){
		float scaleX, scaleY;

		if (scAspect >= textAspect) {
			//La escala en el eje X no necesita ajuste
			scaleX = 1f / nPiecesX;
			//Ajustar la scala en el eje Y por el cambio de proporcion entre la cámara y la pantalla
			scaleY = (scHeight / (scWidth/textAspect)) / nPiecesY;
		} else {
			//Ajustar la scala en el eje X por el cambio de proporcion entre la cámara y la pantalla
			scaleX = (scWidth / (textAspect * scHeight)) / nPiecesX;
			//La escala en el eje Y no necesita ajuste
			scaleY = 1f / nPiecesY;
		}

		return new Vector2 (scaleX*2, scaleY*2);
	}

	private Vector2 GetOffset(float scAspect, float scHeight, float scWidth, float tilingX, float tilingY){

		float offsetCutoffX = 0, offsetCutoffY = 0;

		//Offset range [0, 1-scale]
		//Calculamos el offset sobrante(no entra en la pantalla). (%sobrante = 1-relacion anchuras) / 2
		if (scAspect >= textAspect) {
			offsetCutoffY = ((1 - (scHeight / (scWidth/textAspect))) / 2);
		} else {
			offsetCutoffX = ((1 - (scWidth / (textAspect * scHeight))) / 2);
		}

		float offsetCenterX = (1 - tilingX) / 2;
		float offsetMinX = offsetCenterX - (1 - (tilingX / 2)) / 2 + offsetCutoffX;
		float offsetMaxX = offsetCenterX + (1 - (tilingX / 2)) / 2 - offsetCutoffX;
		float offsetWidthX = offsetMaxX - offsetMinX;
		float offsetX = offsetMinX + offsetWidthX / (nPiecesX-1) * (posX - 1);

		float offsetCenterY = (1 - tilingY) / 2;
		float offsetMinY = offsetCenterY - (1 - (tilingY / 2)) / 2 + offsetCutoffY;
		float offsetMaxY = offsetCenterY + (1 - (tilingY / 2)) / 2 - offsetCutoffY;
		float offsetWidthY = offsetMaxY - offsetMinY;
		float offsetY = offsetMinY + offsetWidthY / (nPiecesY-1) * (posY - 1);

		return new Vector2 (offsetX, offsetY);
	}
		
}
