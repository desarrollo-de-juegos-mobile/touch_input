using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        // podemos crear un objeto 3D , puede ser un cubo para manipularlo con el giro
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, 0);
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.transform.parent = transform; // make the cube a child of this object
        cube.transform.localPosition = new Vector3(0, 0, 0);
        cube.transform.localRotation = Quaternion.identity; // reset rotation
        cube.transform.localScale = new Vector3(1, 1, 1); // set scale
        cube.GetComponent<Renderer>().material.color = Color.red; // set color
        cube.AddComponent<Rigidbody>(); // add a rigidbody to the cube
        cube.GetComponent<Rigidbody>().useGravity = false; // disable gravity
        cube.GetComponent<Rigidbody>().isKinematic = false; // make it kinematic


    }

    // Update is called once per frame
    void Update()
    {
        GyroModifyCamera();
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
        GUILayout.Label("iphone width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);
    }


    /********************************************/

    // The Gyroscope is right-handed.  Unity is left handed.
    // Make the necessary change to the camera.
    void GyroModifyCamera()
    {
        transform.rotation = GyroToUnity(Input.gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
