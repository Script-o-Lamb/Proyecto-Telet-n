using UnityEngine;

public class CoinGroup : MonoBehaviour
{
    private void OnEnable()
    {
        ActivateAllChildren();
    }

    private void ActivateAllChildren()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(true);
        }
    }
}
