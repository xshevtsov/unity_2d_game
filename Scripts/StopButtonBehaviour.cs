using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButtonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject pressSpace;
    [SerializeField] private GameObject gameOver;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("space") && pressSpace.activeSelf)
        {
            gameOver.SetActive(false);
            GameController.Instance.GameRestart();
        }
        
    }
}
