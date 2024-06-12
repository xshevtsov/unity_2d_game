using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    private SpriteRenderer CoinSpriteRanderer;
    private Animator CoinAnimator;
    private Collider2D Collider;
    
    
    [SerializeField] 
    AudioSource coinCollect;
    
    private void Awake()
    {
         CoinSpriteRanderer = gameObject.GetComponent<SpriteRenderer>();
         CoinAnimator = gameObject.GetComponent<Animator>();
         Collider = gameObject.GetComponent<Collider2D>();
    }

    IEnumerator animationCoinDissapear()
    {
        yield return new WaitForSeconds(0.2f);
        var _opacity = 1f;
        while (true)
        {
            _opacity -= 0.15f;
            
            CoinSpriteRanderer.color = 
                new Color(CoinSpriteRanderer.color.r,
                    CoinSpriteRanderer.color.g,
                    CoinSpriteRanderer.color.b, _opacity);

            if (_opacity <= 0f)
            {
                
                CoinSpriteRanderer.color = 
                    new Color(CoinSpriteRanderer.color.r,
                        CoinSpriteRanderer.color.g,
                        CoinSpriteRanderer.color.b, 0f);
                break;
            }
            yield return new WaitForSeconds(.05f);
        }
        gameObject.SetActive(false);
    }
    
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            GameController.Instance.AddScorePlayer(100);
            Collider.isTrigger = false;
            CoinAnimator.SetTrigger("isCollected");

            coinCollect.Play();
            
            
            StartCoroutine(animationCoinDissapear());
            
            
        }
        

    }
}
