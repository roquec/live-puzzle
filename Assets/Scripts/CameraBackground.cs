using UnityEngine;
using System.Collections;

public class CameraBackground : MonoBehaviour {

	public Camera cam;
	public Transform darker;

	void Start () {


		float scHeight = Camera.main.orthographicSize * 2f;
		float scWidth = scHeight * Camera.main.aspect;
		float scAspect = Camera.main.aspect;

		Texture camTexture = DeviceCameraManager.getCamTexture ();
		float textAspect = camTexture.width/ (float)camTexture.height;

		this.GetComponent<Renderer> ().material.mainTexture = camTexture;

		if(scAspect >= textAspect) transform.localScale = new Vector3(scWidth, scWidth / textAspect, 1);
		else transform.localScale = new Vector3(textAspect * scHeight, scHeight, 1);

		/*
		Texture camTexture = DeviceCameraManager.getCamTexture ();
		this.GetComponent<Renderer> ().material.mainTexture = camTexture;

		float scHeight = Camera.main.orthographicSize * 2f;
		float scWidth = scHeight * Camera.main.aspect;
		transform.localScale = new Vector3(scWidth, scHeight, 1);*/
	}
}

