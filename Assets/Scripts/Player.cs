using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //Singletone
    public static Player obj;
    
    public int lives = 3;
    public bool isGrounded = false;
    public bool isMoving = false;
    public bool isInmune = false;

    public float speed = 5f;
    public float jumpForce = 3f;
    public float movHor;

    public float inmuneTimeCnt = 0f;
    public float inmuneTime = 0.5f;

    public LayerMask groundLayer;

    public float radious = 0.3f;
    public float groundRayDist = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;

    void Awake()
    {
        obj = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movHor = Input.GetAxisRaw("Horizontal");
        isMoving = (movHor != 0);
        
        isGrounded = Physics2D.CircleCast(transform.position, radious, Vector3.down, groundRayDist, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space))
            jump();
        
        anim.SetBool("isMoving",isMoving);
        anim.SetBool("isGrounded",isGrounded);

        flip(movHor);

        if(transform.position.y <= -1)
        {
            FXManager.obj.showPop(transform.position);
            Game.obj.gameOver();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);
    }

    
    public void jump()
    {
        if (!isGrounded) return;

        rb.velocity = Vector2.up * jumpForce;
    }
    
    
    
    public void flip(float _xValue)
    {
        Vector3 theScale = transform.localScale;

        if (_xValue < 0)
            theScale.x = Mathf.Abs(theScale.x) * -1;
        else
        if (_xValue>0)
            theScale.x = Mathf.Abs(theScale.x);

        transform.localScale = theScale;
    }
    
    public void getDamaged()
    {
        lives--;
        if(lives<=0)
        {
            //this.gameObject.SetActive(false);
            FXManager.obj.showPop(transform.position);
            Game.obj.gameOver();
        }
    }

    public void bounceAfterKilledEnemy()
    {
        rb.velocity = Vector2.up * 2.5f;
    }

    public void addLive()
    {
        lives++;

        if(lives > Game.obj.maxLives)
            lives = Game.obj.maxLives;
    }

    void OnDestroy()
    {
        obj = null;
    }
}
