using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("Saliendo del juego...");

        // Sale de la app compilada
        Application.Quit();

        // Si estás en el editor, detiene el modo Play (no afecta al build)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
