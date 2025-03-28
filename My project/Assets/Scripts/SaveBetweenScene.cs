using UnityEngine;

public class SaveBetweenScene : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
