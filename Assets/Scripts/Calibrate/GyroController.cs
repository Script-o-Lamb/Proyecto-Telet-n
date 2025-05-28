using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroController : MonoBehaviour
{
    public static GyroController Instance { get; private set; }
    private Gyroscope gyro;
    private bool gyroEnabled;
    public Vector3 calibration = Vector3.zero;  

    private bool useX = true;
    private bool useY = false;
    private bool useZ = false;

    public float Tilt { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    void Start()
    {
        gyroEnabled = SystemInfo.supportsGyroscope;

        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyro.updateInterval = 0.01f;
            Calibrate();
        }
        else
        {
            Debug.LogWarning("Este dispositivo no tiene giroscopio.");
        }
    }

    void Update()
    {
        if (gyroEnabled)
        {
            if (useX)
                Tilt = Input.gyro.gravity.x - calibration.x;
            else if (useY)
                Tilt = Input.gyro.gravity.y - calibration.y;
            else if (useZ)
                Tilt = Input.gyro.gravity.z - calibration.z;
        }
    }
    public void Calibrate()
    {
        if (!gyroEnabled) return;

        calibration = Input.gyro.gravity;

        // Detectar orientación según eje
        Vector3 gravity = calibration;

        if (Mathf.Abs(gravity.z) > 0.8f)
        {
            // Dispositivo está en vertical (en mano)
            SetActiveAxes(true, false, true); // usar X y Z
        }
        else if (Mathf.Abs(gravity.y) > 0.8f)
        {
            // Dispositivo plano (en mesa)
            SetActiveAxes(true, true, false); // usar X y Y
        }
        Debug.Log($"Calibración guardada: {calibration}");

        // Guardar los valores de calibración en PlayerPrefs
        PlayerPrefs.SetFloat("CalibrateX", calibration.x);
        PlayerPrefs.SetFloat("CalibrateY", calibration.y);
        PlayerPrefs.SetFloat("CalibrateZ", calibration.z);
        PlayerPrefs.Save();
    }
    public void LoadCalibration()
    {
        if (PlayerPrefs.HasKey("CalibrateX"))
        {
            calibration.x = PlayerPrefs.GetFloat("CalibrateX");
            calibration.y = PlayerPrefs.GetFloat("CalibrateY");
            calibration.z = PlayerPrefs.GetFloat("CalibrateZ");
            Debug.Log($"Calibración cargada: {calibration}");
        }
    }
    // Ejes para el minijuego actual
    public void SetActiveAxes(bool useX, bool useY, bool useZ)
    {
        this.useX = useX;
        this.useY = useY;
        this.useZ = useZ;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("test");
    }
    public Vector2 GetTiltVector()
    {
        if (!gyroEnabled) return Vector2.zero;

        Vector3 raw = Input.gyro.gravity;
        Vector3 adjusted = raw - calibration;

        float x = useX ? adjusted.x : 0f;
        float y = useY ? adjusted.y : 0f;
        float z = useZ ? adjusted.z : 0f;

        if (useY)
            return new Vector2(x, -y); // Adelante/atrás con Y si está plano
        else
            return new Vector2(x, z);  // Adelante/atrás con Z si está vertical
    }
}