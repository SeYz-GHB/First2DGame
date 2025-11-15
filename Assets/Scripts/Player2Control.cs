using UnityEngine;

public class Player2Control : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    public SpriteRenderer charactorHand;
    public SpriteRenderer weapon_gun_side;

    public Sprite bodySide;
    public Sprite bodyJump;
    public Sprite gunSide;
    public Sprite gunShoot;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        Shoot();
    }

    void Move()
    {
        // LEFT 
        // LEFT
if (Input.GetKey(KeyCode.LeftArrow))
{
    transform.Translate(Vector2.left * speed * Time.deltaTime);

    charactorHand.flipX = false;
    weapon_gun_side.flipX = false;
    facingRight = false; // ← LEFT = facingRight false

    if (isGrounded)
        charactorHand.sprite = bodySide;

    weapon_gun_side.sprite = gunSide;
}

// RIGHT
if (Input.GetKey(KeyCode.RightArrow))
{
    transform.Translate(Vector2.right * speed * Time.deltaTime);

    charactorHand.flipX = true;
    weapon_gun_side.flipX = true;
    facingRight = true; // ← RIGHT = facingRight true

    if (isGrounded)
        charactorHand.sprite = bodySide;

    weapon_gun_side.sprite = gunSide;
}

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            charactorHand.sprite = bodyJump; // Change to jump sprite
            isGrounded = false;
        }
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            weapon_gun_side.sprite = gunShoot;
            
            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetDirection(facingRight);
                }
            }
            
            StartCoroutine(ReturnGunSprite());
        }
    }

    System.Collections.IEnumerator ReturnGunSprite()
    {
        yield return new WaitForSeconds(0.1f);
        weapon_gun_side.sprite = gunSide;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        charactorHand.sprite = bodySide; // Return to normal sprite when landing
    }
}