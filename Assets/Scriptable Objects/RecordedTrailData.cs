using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrailData", menuName = "Trail/Recorded Trail Data")]
public class RecordedTrailData : ScriptableObject
{
    public List<float> recordedYPositions = new();
}
