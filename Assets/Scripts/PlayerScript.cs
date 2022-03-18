using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;
    private Animator anim;

    public float speed;

    public Text score;
    public Text winText;
    public Text livesText;
    public Text loseText;

    public AudioSource winSound;
    public AudioSource BGMusic;

    private int scoreValue = 0;
    private int lives = 3;

    private int stage = 1;

    [SerializeField] float jumpForce = 100f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;
    bool isOnGround = false;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        score.text = "Score: " + scoreValue;
        livesText.text = "Lives: " + lives;

        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxisRaw("Horizontal");
        //float vertMovement = Input.GetAxis("Vertical");
        rb2d.velocity = new Vector2(hozMovement * speed, rb2d.velocity.y);

        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        anim.SetFloat("Speed", Mathf.Abs(hozMovement * speed));
        anim.SetBool("IsGrounded", isOnGround);

        if (isOnGround)
        {
            anim.ResetTrigger("JumpStarted");
            anim.ResetTrigger("JumpComplete");
        }

        if (hozMovement < 0) sprite.flipX = true;
        else if (hozMovement == 0) { }
        else sprite.flipX = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }    
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue;
            collision.gameObject.SetActive(false);

            if (scoreValue >= 4 && stage == 1)
            {
                transform.position = new Vector2(60, 0);
                startPos = new Vector2(60, 0);
                rb2d.velocity = Vector2.zero;
                lives = 3;
                stage = 2;
                livesText.text = "Lives: " + lives;
                score.text = "Score: " + scoreValue;
            }
            else if (scoreValue >= 8)
            {
                winText.gameObject.SetActive(true);
                gameObject.SetActive(false);
                BGMusic.Stop();
                winSound.Play();
            }
        }

        else if (collision.gameObject.tag == "Enemy")
        {
            ReduceLives();

            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "DeathPlane")
        {
            ReduceLives();

            rb2d.velocity = Vector2.zero;
            transform.position = startPos;
        }
    }

    private void ReduceLives()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives <= 0)
        {
            loseText.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                anim.SetTrigger("JumpStarted");
            }
        }
    }

    private void JumpEnded()
    {
        anim.SetTrigger("JumpComplete");
    }
}