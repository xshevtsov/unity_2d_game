using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TrolleybusBehaviour : MonoBehaviour
{


    private Camera _cam;
    public int animStage; 

    public float opacity;

    private GameObject _trolleybus;
    private GameObject _wheels;
    private Animator _wheelsAnimator;
    private GameObject _busNoWheels;
    private SpriteRenderer _busNoWheelsSprite;
    private GameObject _busInside;
    private Animator _busInsideAnimator;
    private GameObject _busInsideWheels;
    private Animator _busInsideWheelsAnimator;
    private GameObject _floor;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject conductor;
    [SerializeField] private GameObject trashcan; 

    private float _timer;

    void Start()
    {   
        
        _trolleybus = gameObject;
        _busNoWheels = _trolleybus.transform.Find("BusNoWheels").gameObject;
        _busNoWheelsSprite = _busNoWheels.GetComponent<SpriteRenderer>();

        _wheels = _trolleybus.transform.Find("Wheels").gameObject;
        _wheelsAnimator = _wheels.GetComponent<Animator>();

        _busInside = _trolleybus.transform.Find("BusInside").gameObject;
        _busInsideAnimator = _busInside.GetComponent<Animator>();
        
        _floor = _busInside.transform.Find("Floor").gameObject;

        _busInsideWheels = _busInside.transform.Find("BusInsideWheels").gameObject;
        _busInsideWheelsAnimator = _busInsideWheels.GetComponent<Animator>();

        _cam = Camera.main;

        opacity = 1f;
        animStage = 0;
        _timer = 0.0f;


    }
    
    
    
    

    
    
    
    
    public void StartUpAnimation()
    {
        animStage = 1;
    }
    


    void Update()
    {
        _timer += Time.deltaTime;
        
        if (animStage == 1){  

            if (_busNoWheels.transform.position.x < 1)
            {
                _busNoWheels.transform.position = new Vector3
                (_busNoWheels.transform.position.x + 3f * Time.deltaTime,
                    _busNoWheels.transform.position.y,
                    _busNoWheels.transform.position.z);

                _wheels.transform.position = new Vector3
                (_wheels.transform.position.x + 3f * Time.deltaTime,
                    _wheels.transform.position.y,
                    _wheels.transform.position.z);


                if (_busNoWheels.transform.position.x > -1.7)
                {
                    _cam.orthographicSize -= 1.2f * Time.deltaTime;
                }

                if (_busNoWheels.transform.position.x > 0.10)
                {
                    animStage = 2;
                }
                

            }

            if (_busNoWheels.transform.position.x < -2)
            {
                _wheelsAnimator.SetBool("isGoing", true);
                
            }
            else
            {
                _wheelsAnimator.SetBool("isGoing", false);
                
            }
            



        }

        if (animStage == 2)
        {

            if (!_floor.activeSelf)
            {
                _floor.SetActive(true);
            }

            _cam.orthographicSize -= 1.2f * Time.deltaTime;
            if (_cam.orthographicSize <= 3.5)
            {
                if (!_busInside.activeSelf)
                {
                    _busInside.SetActive(true);
                }

                _busNoWheelsSprite.color = new Color(
                    _busNoWheelsSprite.color.r,
                    _busNoWheelsSprite.color.g,
                    _busNoWheelsSprite.color.b,
                    opacity);

                opacity = opacity - 0.4f * Time.deltaTime;
            }

            if (_cam.orthographicSize <= 2.9f)
            {
                animStage = 3;
            }
            
        }

        if (animStage == 3)
        {
            player.SetActive(true);
            conductor.SetActive(true);
            trashcan.SetActive(false);
            
            _busNoWheelsSprite.color = new Color(
                _busNoWheelsSprite.color.r,
                _busNoWheelsSprite.color.g,
                _busNoWheelsSprite.color.b,
                opacity);
            opacity = opacity - 0.4f * Time.deltaTime;
            if (opacity <= 0f)
            {
                
                animStage = 4;

            }
            
        }

        if (animStage == 4)
        {
            _cam.transform.position = new Vector3(
                _cam.transform.position.x,
                _cam.transform.position.y,
                _cam.transform.position.z);
            _cam.orthographicSize = _cam.orthographicSize + 0.7f * Time.deltaTime;
            if (_cam.orthographicSize >= 5.4)
            {
                _cam.orthographicSize = 5.4f;
                _cam.transform.position = new Vector3(
                    _cam.transform.position.x,
                    0.05f,
                    _cam.transform.position.z);
                
                animStage = 5;
                _timer = 0;
                _busNoWheels.SetActive(false);
                
                _wheels.SetActive(false);
                _busInsideWheels.SetActive(true);
                
                
                GameController.Instance.busAnimationEnded.Invoke();
            }
            
        }

        


    }
}