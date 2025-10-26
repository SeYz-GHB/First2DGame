using UnityEngine;
using TMPro;


public class Player2Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public Animator animator;
    public TextMeshProUGUI winText;

    public KeyCode jumpKey = KeyCode.UpArrow;
    public KeyCode attackKey = KeyCode.RightControl;

    Rigidbody2D rb;
    bool isGrounded = true;
    int direction = 1;
    bool hasWon = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (hasWon) return;

        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimation();
        CheckWinCondition();
    }

void HandleMovement()
{
    float move = 0;
    if (Input.GetKey(KeyCode.LeftArrow)) move = -speed;
    if (Input.GetKey(KeyCode.RightArrow)) move = speed;

    rb.velocity = new Vector2(move, rb.velocity.y);

    // Try REVERSED flipping
    if (move > 0) 
        transform.localScale = new Vector3(-1, 1, 1); // REVERSED - Face right
    else if (move < 0) 
        transform.localScale = new Vector3(1, 1, 1); // REVERSED - Face left
}
    void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            if (animator != null && HasParameter(animator, "Jump"))
                animator.SetTrigger("Jump");
            isGrounded = false;
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Debug.Log("Player2 attacks!");
            if (animator != null && HasParameter(animator, "Attack"))
                animator.SetTrigger("Attack");
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

    void Flip()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Player2Movement.cs
void CheckWinCondition()
{
    GameObject player1 = GameObject.FindWithTag("Player1");
    if (player1 == null || winText == null) return;

    // Player2 wins by catching Player1 (getting close enough)
    float distance = Vector2.Distance(transform.position, player1.transform.position);
    
    if (distance < 1f) // adjust catch distance as needed
    {
        winText.text = "Player 2 Wins! (Caught Player 1!)";
        winText.gameObject.SetActive(true);
        hasWon = true;

        Player1Movement p1 = player1.GetComponent<Player1Movement>();
        if (p1 != null) p1.DisableMovement();
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
