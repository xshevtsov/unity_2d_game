using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BackGroundScroller : MonoBehaviour
{
    public float speed;

    private float _offset;
    private Material _mat;
    [SerializeField] private GameObject trashCan;
    
    [SerializeField] GameObject _busInside;
    [SerializeField] GameObject _busInsideWheels;
    
    private Animator _busInsideAnimator;
    private Animator _wheelsAnimator;

    private float _timer;
    private float _part;
    private float _divider;
    private Coroutine _speedUpCoroutine;

    public bool _isSpeedUpActive;
    public bool _stopped;
    private float _offSetForTrash;

    
    
    

    private void Awake()
    {
        _mat = GetComponent<Renderer>().material;
        _isSpeedUpActive = false;
        _stopped = true;
        _divider = 18f;
        _offset = 0f;
        _offSetForTrash = 0f;
        
        
        _busInsideAnimator = _busInside.GetComponent<Animator>();
        _wheelsAnimator = _busInsideWheels.GetComponent<Animator>();
        
        
        _speedUpCoroutine = StartCoroutine(SpeedUpCoroutine());
        
    }

   
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    IEnumerator SpeedUpCoroutine()
    {
        while (true)
        {
            // Debug.Log(_isSpeedUpActive);
            
            if (!_isSpeedUpActive && !_stopped)
            {
                if(_wheelsAnimator.GetBool("IsFast"))
                    _wheelsAnimator.SetBool("IsFast", false);
                for (float i = 12f; i < 80; i = i+0.5f) 
                {
                    var part = (_part) / i;
                    _offset += part;
                    // Debug.Log("STOP" + _offset);
                    _mat.SetTextureOffset("_MainTex", new Vector2(_offset, 0));
                    yield return new WaitForSeconds(0.02f);
                }
                if(_wheelsAnimator.GetBool("isGoing"))
                    _wheelsAnimator.SetBool("isGoing", false);
                if(_busInsideAnimator.GetBool("isMoving"))
                    _busInsideAnimator.SetBool("isMoving", false);
                
                _offSetForTrash = 0f;
                _stopped = true;
                
                
                
                
                
                _divider = 18f;
            }
            
            
            if (_isSpeedUpActive)
            {
                bool maxVelocity = true;
                if(!_wheelsAnimator.GetBool("isGoing"))
                    _wheelsAnimator.SetBool("isGoing", true);
                _stopped = false;
                _part = speed / _divider;
            
                if (_divider >= 1)
                {
                    _divider -= 0.15f;
                    maxVelocity = false;
                }

                if (maxVelocity)
                {
                    if(!_wheelsAnimator.GetBool("IsFast"))
                        _wheelsAnimator.SetBool("IsFast", true);
                    if(!_busInsideAnimator.GetBool("isMoving"))
                        _busInsideAnimator.SetBool("isMoving", true);
                }
            
                _offset += (_part) / 18f;
                _offSetForTrash += (_part) / 18f;
                // trashCan.transform.position = new Vector3(
                //     trashCan.transform.position.x - _offSetForTrash/10.25f,
                //     trashCan.transform.position.y,
                //     trashCan.transform.position.z);
                
                // Debug.Log("_offSetForTrash/14.7f" + _offSetForTrash/14.7f);
                // Debug.Log("_(_part) / 12f;" + (_part) / 12f);
                // Debug.Log("Riding" + _offset);
            
                _mat.SetTextureOffset("_MainTex", new Vector2(_offset, 0));
                
                
                
                
            }
            
            
            
            yield return new WaitForSeconds(0.02f);
            
            
            
             
        }
        
         
    }
    
    
    
    void Update()
    {
        
        

    }
}