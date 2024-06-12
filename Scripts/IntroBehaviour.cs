using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject introText;
    [SerializeField] private GameObject startGameSentence;

    private Animator _introTextAnimator;
    private Animator _startGameSentenceAnimator;
    private bool _isAnimationEnded;
    


    public bool IsAnimationEnded()
    {
        return _isAnimationEnded;
    }

    public void StartUpAnimation()
    {
        StartCoroutine(StartUpAnimationCoroutine());
    }
    
    IEnumerator StartUpAnimationCoroutine()
    {
        _isAnimationEnded = false;
        _introTextAnimator = introText.GetComponent<Animator>();
        _startGameSentenceAnimator = startGameSentence.GetComponent<Animator>();
        
        introText.SetActive(true);
        _introTextAnimator.SetTrigger("stage1Blink");
        yield return new WaitForSeconds(3);
        startGameSentence.SetActive(true);
        
    }
    
    
    void Update()
    {
        if (Input.GetKeyDown("space") && startGameSentence.activeSelf)
        {
            _introTextAnimator.SetTrigger("stage2Exit");
            startGameSentence.SetActive(false);
            _isAnimationEnded = true;
            
            GameController.Instance.startAnimationEnded.Invoke();
        }
        
    }
}
