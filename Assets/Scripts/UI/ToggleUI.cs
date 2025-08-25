using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    [Header("Arrastra aquí todos los GameObjects de UI que quieras ocultar/mostrar")]
    public GameObject[] uiPanels;

    private bool isVisible = true; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            isVisible = !isVisible;

            foreach (GameObject panel in uiPanels)
            {
                panel.SetActive(isVisible);
            }
        }
    }
}