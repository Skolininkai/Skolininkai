using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelBounds : MonoBehaviour
{
    public BoxCollider resetTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
