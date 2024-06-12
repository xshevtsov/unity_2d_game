using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofEnterences : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour player;
    private bool _isUp;
    private BoxCollider2D[] _collidersList;

    private void Start()
    {
        _collidersList = GetComponents<BoxCollider2D>();
    }

    public bool IsPlayerUp()
    {
        return _isUp;
    }

    public void ResetColliders()
    {
        _isUp = false;
        foreach (var boxCollider2D in (_collidersList))
        {
            boxCollider2D.isTrigger = true;
        }
    }
    public void SetColldidersFalse()
    {
        foreach (var boxCollider2D in (_collidersList))
        {
            boxCollider2D.isTrigger = false;
        }
    }
    
   
    IEnumerator liftPlayerUpCoroutine()
    {
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        while (player.transform.position.y < 3.85)
        {
            player.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y +0.25f,
                player.transform.position.z);
            yield return new WaitForSeconds(0.02f);
        }
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<Rigidbody2D>().AddForce(player.IsFacingRight() ? Vector2.right * 4 : Vector2.left * 4, ForceMode2D.Impulse);

        


    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isUp)
        {
            StartCoroutine(liftPlayerUpCoroutine());
            _isUp = true;
            

            foreach (var boxCollider2D in (_collidersList))
            {
                boxCollider2D.isTrigger = false;
            }

        }
        else
        {
            _isUp = false;
        }
        

    }

    private void Update()
    {
            if(Input.GetKey("down"))
            {
                if (_collidersList != null)
                {
                    foreach (var boxCollider2D in (_collidersList))
                    {
                        boxCollider2D.isTrigger = true;
                        
                    }
                }
                _isUp = false;
                
            }
            
    }
}
