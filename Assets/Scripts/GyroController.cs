using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour
{
    private Gyroscope gyro;
    private bool gyroEnabled;

    void Start()
    {
        gyroEnabled = SystemInfo.supportsGyroscope;

        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyro.updateInterval = 10f;
        }
        else
        {
            Debug.Log("Este dispositivo no tiene giroscopio.");
        }
    }
    void FixedUpdate()
    {
        if (gyroEnabled)
        {
            Quaternion deviceRotation = gyro.attitude;
            Quaternion unityRotation = new Quaternion(-deviceRotation.x, -deviceRotation.y, deviceRotation.z, deviceRotation.w);
            transform.rotation = unityRotation;
        }
    }
}

