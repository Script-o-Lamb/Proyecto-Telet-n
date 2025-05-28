using UnityEngine;
using UnityEngine.UI;

public class GyroAngleDisplay : MonoBehaviour
{
    public Text displayText;

    void Update()
    {
        if (!SystemInfo.supportsGyroscope || GyroController.Instance == null)
            return;

        Vector3 gravity = Input.gyro.gravity;
        Vector3 calib = GyroController.Instance.calibration;
        Vector3 tilt = gravity - calib;


        float rollDegrees = -tilt.x * 90f;
        float pitchDegrees = tilt.y * 90f;

        displayText.text = $"Inclinaci�n lateral : {rollDegrees:F2}�\n" +
                            $"Inclinaci�n frontal : {pitchDegrees:F2}�";
    }
}