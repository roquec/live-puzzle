using UnityEngine;
using System.Collections;

public static class DeviceCameraManager{
	
	static Texture camTexture;

	public static Texture getCamTexture(){
		if (camTexture)
			return camTexture;
		else {
			camTexture = GetMainTexture ();
			return camTexture;
		}
	}

	private static Texture GetMainTexture(){

		WebCamDevice[] devices = WebCamTexture.devices;

		//Look for the camera
		for (int i = 0; i < devices.Length; i++) {
			if (!devices[i].isFrontFacing) {
				WebCamTexture CameraTexture = new WebCamTexture(devices[i].name);
				CameraTexture.Play ();
				return CameraTexture;
			}
		}

		//No camera found
		Debug.Log ("No hay camara disponible");
		return Resources.Load ("default") as Texture;
	}
}
