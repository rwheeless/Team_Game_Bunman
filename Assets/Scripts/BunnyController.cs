﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BunnyController : MonoBehaviour
{
    public float speed;
    private float boostTimer;

    private Rigidbody2D rb2d;

    private bool gameOver;
    private bool boosting;

    public Text scoreText;
    public Text healthText;

    private int score;
    private int health;

    public Camera MainCamera;

    public Animator animator;

    public bool Vertical;
    public bool Horizontal;
    int direction = 1;

    Vector2 lookDirection = new Vector2(1,0);
   
   
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        gameOver = false;
        score = 0;
        SetScoreText ();
        health = 3;
        SetHealthText ();

        boostTimer = 0;
        boosting = false;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");

        float moveVertical = Input.GetAxis ("Vertical");

        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

        rb2d.AddForce (movement * speed);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (boosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= 3)
            {
                speed = 3;
                boostTimer = 0;
                boosting = false;
            }
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
        
        animator.SetFloat("Move X", lookDirection.x);
        animator.SetFloat("Move Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            other.gameObject.SetActive (false);
            score = score + 1;
            SetScoreText ();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            /*other.gameObject.SetActive (false);*/
            health = health - 1;
            SetHealthText ();
        }

        if (other.gameObject.CompareTag("speed"))
        {
            other.gameObject.SetActive (false);
            boosting = true;
            speed = 10;
        }

        if (other.gameObject.CompareTag("berry"))
        {
            other.gameObject.SetActive (false);
            score = score + 3;
            SetScoreText ();
        }

        if (other.gameObject.CompareTag("enter1"))
        {
            gameObject.transform.position = new Vector3(10, 0.0f, -10);
        }

        if (other.gameObject.CompareTag("exit1"))
        {
            gameObject.transform.position = new Vector3(-10, 0.0f, -10);
        }

        if (other.gameObject.CompareTag("enter2"))
        {
            gameObject.transform.position = new Vector3(-42, -3, -6);
        }

        if (other.gameObject.CompareTag("exit2"))
        {
            gameObject.transform.position = new Vector3(-42, 4, -6);
        }
    }
    void SetScoreText ()
    {
        scoreText.text = "Score: " + score.ToString ();

        if (score == 36)
        {
            gameObject.transform.position = new Vector3(-42, 0.0f, -6);
            MainCamera.transform.position = new Vector3(-42, 0.0f, -8);
        }

        if (score == 72)
        {
            //winText.text = "You Win!";
            //Destroy(gameObject);
            gameOver = true;
        }
        
    }

    void SetHealthText ()
    {
        healthText.text = "Lives: " + health.ToString ();

        if (health <= 0)
        {
            speed = 0;
            gameOver = true;
            health = 0;
        }
    }
}
