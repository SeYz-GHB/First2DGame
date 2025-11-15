using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1Spawn : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically find the SpawnPoint in the loaded scene
        GameObject spawn = GameObject.Find("SpawnPoint1");
        if (spawn != null)
        {
            transform.position = spawn.transform.position; // move player there
        }
    }
}
