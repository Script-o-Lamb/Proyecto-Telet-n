using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAngleData", menuName = "Angle/Recorded Angle Data")]
public class RecordedAngleData : ScriptableObject
{
    [Header("Lista de ángulos registrados durante la sesión")]
    public List<float> recordedAngles = new();

    public void AddAngle(float angle)
    {
        recordedAngles.Add(angle);
    }

    public void ClearData()
    {
        recordedAngles.Clear();
    }
}