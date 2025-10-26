using UnityEngine;
using TMPro;


public class Player1Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public Animator animator;
    public TextMeshProUGUI winText;


    Rigidbody2D rb;
    bool isGrounded = true;
    bool hasWon = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (hasWon) return; // stop moving if game already won

        HandleMovement();
        HandleJump();
        UpdateAnimation();
        CheckWinCondition();
    }

    void HandleMovement()
    {
        float move = 0;
        if (Input.GetKey(KeyCode.A)) move = -speed;
        if (Input.GetKey(KeyCode.D)) move = speed;

        rb.velocity = new Vector2(move, rb.velocity.y);

        if (move > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            if (animator != null && HasParameter(animator, "Jump"))
                animator.SetTrigger("Jump");
            isGrounded = false;
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;
        float horizontal = Mathf.Abs(rb.velocity.x);

        if (HasParameter(animator, "isRunning"))
            animator.SetBool("isRunning", horizontal > 0.01f);

        if (HasParameter(animator, "isFalling"))
            animator.SetBool("isFalling", rb.velocity.y < -0.01f && !isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                if (animator != null && HasParameter(animator, "isFalling"))
                    animator.SetBool("isFalling", false);
            }
        }
    }

    // Player1Movement.cs
void CheckWinCondition()
{
    GameObject player2 = GameObject.FindWithTag("Player2");
    
    // Debug current position
    Debug.Log("Player1 X: " + transform.position.x);
    
    if (player2 == null)
    {
        Debug.LogWarning("Player2 not found! Make sure Player2 has tag 'Player2'");
        return;
    }
    
    if (winText == null)
    {
        Debug.LogWarning("WinText is null in CheckWinCondition!");
        return;
    }

    // Change from 10f to 3f or 4f
    if (transform.position.x >= 3.5f) // ← CHANGE THIS VALUE
    {
        Debug.Log("=== PLAYER 1 WINS! ===");
        winText.text = "Player 1 Wins! (Escaped!)";
        winText.gameObject.SetActive(true);
        hasWon = true;

        Player2Movement p2 = player2.GetComponent<Player2Movement>();
        if (p2 != null) p2.DisableMovement();
        DisableMovement();
    }
}
    bool HasParameter(Animator anim, string paramName)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
            if (param.name == paramName) return true;
        return false;
    }

    public void DisableMovement()
    {
        rb.velocity = Vector2.zero;
        enabled = false;
    }
}
