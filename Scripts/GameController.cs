using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{

    public UnityEvent startAnimationEnded = new UnityEvent();
    public UnityEvent busAnimationEnded = new UnityEvent();
    public static GameController Instance;

    [SerializeField] private IntroBehaviour intro;
    [SerializeField] private TrolleybusBehaviour trolley;
    [SerializeField] private BackGroundScroller backgroundScroller;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject trashCan;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject conductor;
    [SerializeField] private RoofEnterences roofEnterences;
    [SerializeField] private ConductorBehaviour conductorBehaviour;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private PassengersBehaviour passengersBehaviour;
    [SerializeField] private GameObject gameOver;



    public AudioSource magadan;
    public AudioSource subway;
    
    


    [SerializeField] private GameObject playScore;
    
    private int _playerScore = 0;
    private int _conductorScore = 0;
    private int _stopsCount = 0;
    
    


    public void AddScorePlayer(int amount)
    {
        var insideText = playScore.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        var outSideText = playScore.transform.GetChild(0).GetChild(1).gameObject;
        _playerScore += amount;
        
        insideText.GetComponent<TextMeshProUGUI>().SetText(_playerScore.ToString());        
        outSideText.GetComponent<TextMeshProUGUI>().SetText(_playerScore.ToString());
    }

    public void AddScoreConductor(int amount)
    {
        var insideText = playScore.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        var outSideText = playScore.transform.GetChild(1).GetChild(1).gameObject;
        _conductorScore += amount;
        
        // Debug.Log(insideText.GetComponent<TextMeshPro>());
        insideText.GetComponent<TextMeshProUGUI>().SetText(_conductorScore.ToString());        
        outSideText.GetComponent<TextMeshProUGUI>().SetText(_conductorScore.ToString());
        
    }

    public void AddStopsCount(int amount)
    {
        var insideText = playScore.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        var outSideText = playScore.transform.GetChild(2).GetChild(0).gameObject;
        _stopsCount += amount;

        
        insideText.GetComponent<TextMeshProUGUI>().SetText("STOPS: " + _stopsCount.ToString());        
        outSideText.GetComponent<TextMeshProUGUI>().SetText("STOPS: " + _stopsCount.ToString());
    }
    
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
        magadan.Play();
        magadan.loop = true;
        
        
        
        startAnimationEnded.AddListener(BusStart);
        busAnimationEnded.AddListener(GameStart);
        intro.StartUpAnimation();
        
        



    }

    void BusStart()
    {
        
        trolley.StartUpAnimation();
        
        
    }

    IEnumerator MoveBackGroundAdditions()
    {
        while (trashCan.gameObject.transform.position.x >= -19.25)
        {
            trashCan.transform.position = new Vector3(
                trashCan.transform.position.x - 0.05f,
                trashCan.transform.position.y,
                trashCan.transform.position.z);
    
            yield return new WaitForSeconds(0.02f);
        }
        
    }
    
    IEnumerator BusGoBusStop()
    {
        yield return new WaitForSeconds(0.2f);
        
        
        
        while (true)
        {

            
            
            if (backgroundScroller._stopped)
            {
                while (true)
                {
                    // Debug.Log(passengersBehaviour.isBusGoCoroutineFinished);
                    if (passengersBehaviour.isBusGoCoroutineFinished)
                    {
                        Debug.Log("isBusGoCoroutineFinished");
                        break;
                    }
            
                    yield return null;
                }
                passengersBehaviour.StartBusStopCorotine();
                Debug.Log("STOPPING STARTED");
                AddStopsCount(1);
                
            }
            // yield return new WaitForSeconds(0.2f);
            // if (backgroundScroller._stopped)
            //     passengersBehaviour.StartBusStopCorotine();
            yield return new WaitForSeconds(Random.Range(2f,4f));
            backgroundScroller._isSpeedUpActive = !backgroundScroller._isSpeedUpActive;
            
            
            
            while (true)
            {
                // Debug.Log(passengersBehaviour.isBusStopCoroutineFinished);
                if (passengersBehaviour.isBusStopCoroutineFinished)
                {
                    // Debug.Log(passengersBehaviour.isBusStopCoroutineFinished);
                    Debug.Log("isBusStopCoroutineFinished");
                    break;
                }
            
                yield return null;
            }
            passengersBehaviour.StartBusGoCoroutine();
            Debug.Log("MOVING STARTED");
            yield return new WaitForSeconds(Random.Range(5f,10f));
            
            
           
        }
        
    }

    

    IEnumerator ConductorStateChangeCoroutine()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (!roofEnterences.IsPlayerUp())
            {
                if (!conductorBehaviour.isPlayerBehind() || playerBehaviour.isShowingYelling)
                {
                    if(!conductorBehaviour.IsAgressiveRunning())
                        conductorBehaviour.StartAggressiveCoroutine();
                }
                
            }
            else
            {
                if(!conductorBehaviour.IsPassiveRunning())
                    conductorBehaviour.StartPassiveCoroutine();
            }


            
        }
        

    }

    void GameStart()
    {
        magadan.Stop();
        
        
        subway.Play();
        subway.loop = true;
        
        
        background.gameObject.SetActive(false);
        backgroundScroller.gameObject.SetActive(true);
        playScore.SetActive(true);
        
        
        StartCoroutine(BusGoBusStop());
        StartCoroutine(ConductorStateChangeCoroutine());
        
        
    }


    public void GameOver()
    {
        gameOver.SetActive(true);
    }
    public void GameRestart()
    {
        AddScorePlayer(-_playerScore);
        AddScoreConductor(-_conductorScore);
        AddStopsCount(-_stopsCount);

        roofEnterences.ResetColliders();
        player.transform.position = new Vector3(-6.37f, 0.68f, 0f);
        player.gameObject.GetComponent<Animator>().SetBool("isTakingDamage", false);
        playerBehaviour.StartPlayerMovement();

        conductor.transform.position = new Vector3(5.8f, 0.7f, -0.5145456f);
        conductor.gameObject.GetComponent<Animator>().SetBool("isAttaking", false);
        conductorBehaviour.StartAggressiveCoroutine();
        

    }
   
   
}
