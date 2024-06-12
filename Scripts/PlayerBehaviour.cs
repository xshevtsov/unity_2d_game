using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private Coroutine _playerMovement;

    private Rigidbody2D rb;

    private bool isJumping = false;
    private bool isFacingRight = true;
    public bool isShowingYelling = false;
    private float _timer = 0f;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        _playerMovement = StartCoroutine(PlayerMovement());
    }

    public void StopPlayerMovement()
    {
        if (_playerMovement != null)
        {
            StopCoroutine(_playerMovement);
            _playerMovement = null;
        }
        
    }

    public void StartPlayerMovement()
    {
        if(_playerMovement == null)
        {
            _playerMovement = StartCoroutine(PlayerMovement());
        }

    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }


    IEnumerator PlayerMovement()
    {
        
        
        
        while (true)
        {
            if (Input.GetButton("Jump"))
            {
                if (!isJumping)
                {
                    anim.SetTrigger("Jump");

                    rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    isJumping = true;
                }
            }

            

            if (Input.GetAxis("Horizontal") != 0)
            {
            
            
            
                if (Input.GetAxis("Horizontal") < 0 && isFacingRight)
                {
                    isFacingRight = false;
                    transform.localRotation = Quaternion.Euler(0,180,0);
                
                }
                else if (Input.GetAxis("Horizontal") > 0 && !isFacingRight)
                {
                    isFacingRight = true;
                    transform.localRotation = Quaternion.Euler(0,0,0);
                }
                
                if (!anim.GetBool("isWalking"))
                {
                    anim.SetBool("isWalking",true);
                }
                yield return new WaitForSeconds(0.15f);
            
            
            
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 6.5f, rb.velocity.y);
                
            }
            else if (anim.GetBool("isWalking"))
            {
                anim.SetBool("isWalking",false);
            }
        
        
        
        
            yield return null;
        }
    }


    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 1f)
        {
            _timer = 0;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            isShowingYelling = false;
        }
        if (Input.GetKey("up"))
        {
            if (!isShowingYelling)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                isShowingYelling = true;

            }

        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BusFloor") || collision.gameObject.CompareTag("BusRoof")  && isJumping)
        {
            
            isJumping = false;
        }
        
    }
}
