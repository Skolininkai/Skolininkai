using UnityEngine;

public class StopTrigger : MonoBehaviour
{
    [SerializeField] private WallSpawner wallSpawner;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("finish trigger entered");
        if (other.CompareTag("Player"))
        {
            wallSpawner.StopSpawning();
        }
    }
}
