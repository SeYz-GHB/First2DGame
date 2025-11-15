using UnityEngine;
using TMPro;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private float direction = 1f;
    private TextMeshProUGUI winText;

    void Start()
    {
        // Find the WinText in the scene
        GameObject textObj = GameObject.Find("WinText");
        if (textObj != null)
            winText = textObj.GetComponent<TextMeshProUGUI>();

        // Hide win text at start
        if (winText != null)
            winText.gameObject.SetActive(false);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    public void SetDirection(bool facingRight)
    {
        direction = facingRight ? 1f : -1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If bullet hits Player1
        if (other.CompareTag("Player1"))
        {
            Debug.Log("=== PLAYER 2 WINS! ===");

            // Show win text
            if (winText != null)
            {
                winText.text = "Player 2 Wins!";
                winText.gameObject.SetActive(true);
            }

            // Stop Player1 movement
            Player1Movement p1 = other.GetComponent<Player1Movement>();
            if (p1 != null)
                p1.DisableMovement();

            // Stop Player2 movement
            GameObject player2 = GameObject.FindWithTag("Player2");
            if (player2 != null)
            {
                Player2Control p2 = player2.GetComponent<Player2Control>();
                if (p2 != null)
                    p2.enabled = false;
            }

            Destroy(gameObject); // destroy the bullet
        }
    }
}
