using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConductorBehaviour : MonoBehaviour
{


    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private RoofEnterences _roofEnterences;
    private Animator anim;

    private Rigidbody2D rb;
    
    private bool _isFacingRight;
    private float _speed;
    private float _divider;
    private float _part;
    private Coroutine _passiveMovement;
    private bool _isPassiveRunning;
    private bool _isAggressiveRunning;
    private Coroutine _aggressiveMovement;


    public bool IsFacingRight()
    {
        return _isFacingRight;
    }
    public bool IsPassiveRunning()
    {
        return _isPassiveRunning;
    }

    public bool IsAgressiveRunning()
    {
        return _isAggressiveRunning;

    }

    public void StartPassiveCoroutine()
    {
        _passiveMovement = StartCoroutine(ConductorPassiveMovementCoroutine());
        if(_aggressiveMovement != null)
            StopCoroutine(_aggressiveMovement);
        _isPassiveRunning = true;
        _isAggressiveRunning = false;

    }
    public void StartAggressiveCoroutine()
    {
        _aggressiveMovement = StartCoroutine(ConductorAgressiveMovementCoroutine());
        if(_passiveMovement != null)
         StopCoroutine(_passiveMovement);
        _isAggressiveRunning = true;
        _isPassiveRunning = false;
    }
    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _isFacingRight = false;
        
        _speed = 3.1f;
        _divider = 7f;
        _part = 0f;
    }




    IEnumerator ConductorPassiveMovementCoroutine()
    {
        while (true)
        {
           
            
            if(anim.GetBool("isRunning"))
            {
                anim.SetBool("isRunning",false);
                    
            }
            
            if(Random.Range(1f, 2f) > 1.7f)
            {
                rotate();
            }
            
            _part = _speed / _divider;

            if (_divider >= 1)
            {
                if(anim.GetBool("isWalking"))
                {
                    anim.SetBool("isWalking",false);
                    
                }
                
                yield return new WaitForSeconds(1f);
                _divider -= Random.Range(1f, 5f);
            }

            if (_divider < 1)
            {
                if(!anim.GetBool("isWalking"))
                {
                    anim.SetBool("isWalking",true);
                    
                }
                _divider = Random.Range(1f, 5f);
            }
            
            
            yield return new WaitForSeconds(0.15f);
            rb.velocity = new Vector2(_isFacingRight ? 1 * _part : -1 * _part, rb.velocity.y);
            
           
            
            
            
        }
    }
    
    IEnumerator ConductorAgressiveMovementCoroutine()
    {
        while (true)
        {


            if (isPlayerBehind())
            {
                rotate();
            }
            if(anim.GetBool("isWalking"))
                anim.SetBool("isWalking", false);
                
            if(!anim.GetBool("isRunning"))
            {
                anim.SetBool("isRunning",true);
                    
            }
            // transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.015f);
            
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z) , 4.8f*Time.deltaTime);


            yield return null;
        }
    }
    
    
    IEnumerator ConductorAttack()
    {
        
        
        _roofEnterences.SetColldidersFalse();
        
        if(_passiveMovement != null)
            StopCoroutine(_passiveMovement);
        if(_aggressiveMovement != null)
            StopCoroutine(_aggressiveMovement);
        
        if(anim.GetBool("isWalking"))
            anim.SetBool("isWalking", false);
        
        if(anim.GetBool("isRunning"))
            anim.SetBool("isRunning", false);



        player.StopPlayerMovement();
        player.GetComponent<Animator>().SetBool("isTakingDamage",true);
        
        if (!anim.GetBool("isAttaking"))
        {
            anim.SetBool("isAttaking",true);
        }
        yield return new WaitForSeconds(1.5f);
        
        if (anim.GetBool("isAttaking"))
        {
            anim.SetBool("isAttaking",false);
        }

        GameController.Instance.GameOver();
        // player.GetComponent<Rigidbody2D>().simulated = true;


    }

    public bool isPlayerBehind()
    {
        if (player.transform.position.x < transform.position.x && _isFacingRight)
        {
            return true;
        }

        if (player.transform.position.x > transform.position.x && !_isFacingRight)
        {
            return true;
        }

        return false;
    }
    
    
    private void rotate()
    {
        _isFacingRight = !_isFacingRight;
        if (_isFacingRight)
        {
                
            transform.localRotation = Quaternion.Euler(0,180,0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0,0,0);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BusWalls"))
        {
            rotate();


        }
        if (collision.gameObject.CompareTag("Player"))
        {

            if (isPlayerBehind())
            {
                rotate();
            }
            
            
            StartCoroutine(ConductorAttack());

        }
        
        
        
        
    }

    private void Update()
    {
        
    }
}
