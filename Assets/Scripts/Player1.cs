using UnityEngine;

public class Player1 : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // keeps player alive across scenes
    }
}
