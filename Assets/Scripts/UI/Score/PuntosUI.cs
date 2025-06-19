using TMPro;
using UnityEngine;

public class PuntosUI : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    void Update()
    {
        textMesh.text = GameFlowManager.Instance.GetPuntosActuales().ToString("0");
    }
}
