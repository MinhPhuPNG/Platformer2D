using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5f;
    float jumpForce = 5f;
    int maxJumps = 2;
    int jumpsRemaining = 0; 
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite flowerSprite;
    public TextMeshProUGUI LevelCompleteText;
    public TextMeshProUGUI SpaceText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
        }
            // player velocity
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(Keyboard.current.spaceKey.wasPressedThisFrame && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
        }

        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        if(Time.timeScale == 0f)
        {
            if(Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                RestartLevel();
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") || other.CompareTag("Enemy"))
        {
            RestartLevel();
        }
        if (other.CompareTag("Collectible"))
        {
            if (flowerSprite != null)
            {
                spriteRenderer.sprite = flowerSprite;
            }

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Goal"))
        {
            if (spriteRenderer.sprite == flowerSprite)
        {            
            LevelCompleteText.gameObject.SetActive(true);
            SpaceText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("You need a flower to finish the level.");
        }
        }
    }
    
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
