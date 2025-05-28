using UnityEngine;

public class GyroVisualReference : MonoBehaviour
{

    void Update()
    {

        if (!SystemInfo.supportsGyroscope || GyroController.Instance == null)
            return;

        Vector3 raw = Input.gyro.gravity;
        Vector3 calibration = GyroController.Instance.calibration;
        Vector3 tilt = raw - calibration;

        transform.rotation = Quaternion.Euler(
            tilt.y * 90f,
            0f,
            -tilt.x * 90f
        );
    }
}