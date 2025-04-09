using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    float speed = 0.1f; // Speed of the object

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 acceleration = Vector3.zero;

        // Check if the device has an accelerometer
        if (SystemInfo.supportsAccelerometer)
        {
            // remap the device acceleration axis to game coordinates:
            // 1) XY plane of the device is mapped onto XZ plane
            // 2) rotated 90 degrees around Y axis

            acceleration.x = -Input.acceleration.y;
            acceleration.y = Input.acceleration.x;

            Debug.Log("Accelerometer: " + acceleration);

            // Recaudos para cuando no esta normalizado el vector.
            if ( acceleration.sqrMagnitude > 1)
            {
                acceleration.Normalize();
            }

            // Move the object based on the accelerometer input
            transform.position += acceleration * speed * Time.deltaTime;

        }
    }
}
