using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour {

    //public GameObject player; // First Person Controller parent node
    //public GameObject head; // First Person Controller camera

	public float x, y, z;

    // The initials orientation
    private float initialOrientationX;
    private float initialOrientationY;
    private float initialOrientationZ;

    // Use this for initialization
    void Start()
    {
        // Activate the gyroscope
        Input.gyro.enabled = true;

        // Save the firsts values
        //initialOrientationX = Input.gyro.rotationRateUnbiased.x;
        //initialOrientationY = Input.gyro.rotationRateUnbiased.y;
        //initialOrientationZ = -Input.gyro.rotationRateUnbiased.z;

        //Quaternion attitude = Input.gyro.attitude;
        //initialOrientationX = attitude.x;
        //initialOrientationY = attitude.y;
        //initialOrientationZ = attitude.z;

        var camParent = new GameObject("camParent"); // make a new parent
        camParent.transform.position = transform.position; // move the new parent to this transform position
        transform.parent = camParent.transform; // make this transform a child of the new parent

        camParent.transform.eulerAngles = new Vector3(90, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
		//Debug.Log ("ROTATION W = (" + transform.eulerAngles.x + "," + transform.eulerAngles.y + "," + transform.eulerAngles.z + ")");
		//Debug.Log ("ROTATION L = (" + transform.localEulerAngles.x + "," + transform.localEulerAngles.y + "," + transform.localEulerAngles.z + ")");
        // Rotate the player and head using the gyroscope rotation rate
        //player.transform.Rotate(0, initialOrientationY - Input.gyro.rotationRateUnbiased.y, 0);
        //head.transform.Rotate(initialOrientationX - Input.gyro.rotationRateUnbiased.x, 0, initialOrientationZ + Input.gyro.rotationRateUnbiased.z);

        //gameObject.transform.Rotate(initialOrientationX - Input.gyro.rotationRateUnbiased.x, initialOrientationY - Input.gyro.rotationRateUnbiased.y, initialOrientationZ + Input.gyro.rotationRateUnbiased.z);

        //gameObject.transform.Rotate(initialOrientationX - Input.gyro.attitude.x, initialOrientationY - Input.gyro.attitude.y, initialOrientationZ + Input.gyro.attitude.z);
        var fix = new Quaternion(0, 0, 1, 0);
		if (SystemInfo.supportsGyroscope)
			gameObject.transform.localRotation = Input.gyro.attitude * fix;
		else {
			gameObject.transform.RotateAround (Vector3.zero, Vector3.up, y);
			gameObject.transform.RotateAround (Vector3.zero, Vector3.right, x);
			gameObject.transform.RotateAround (Vector3.zero, Vector3.forward, z);
		}
    }
}
