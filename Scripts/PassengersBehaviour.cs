using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassengersBehaviour : MonoBehaviour
{
    private Coroutine _coinAppearCoroutine;

    private Coroutine _busStopCorotine;
    private Coroutine _busGoCorotine;
    
    private List<Transform> _passengers;
    private List<PassengerObject> _passengersWithParams;
    private Transform _gameObjectToAppear;
    
    [SerializeField] 
    AudioSource coinFatigue;
    
    public bool isBusStopCoroutineFinished ;
    public bool isBusGoCoroutineFinished;
    

    [SerializeField] BackGroundScroller backgroundScroller;


    public bool getIsBusStopCoroutineFinished()
    {
        return isBusStopCoroutineFinished;
    }
    public bool getIsBusGoCoroutineFinished()
    {
        return isBusGoCoroutineFinished;
    }
    
    
    
    void Start()
    {
        isBusStopCoroutineFinished = true;
        isBusGoCoroutineFinished = true;
        _passengersWithParams = new List<PassengerObject>();
        
        _passengers = GetComponentsInChildren<Transform>().ToList();
        
        foreach (var passenger in (_passengers.ToList()))
        {
            
            if (passenger.name.Contains("coin") || passenger.name.Equals("Passengers"))
            {
                _passengers.Remove(passenger);
                
            }
            else
            {
                passenger.gameObject.SetActive(false);
                PassengerObject passangerWithParam = new PassengerObject();
                passangerWithParam.passenger = passenger.gameObject;
                passangerWithParam.hasCoin = false;
                passangerWithParam.startTime = DateTime.Now;
                passangerWithParam.timePassed = 0;
                
                // Debug.Log(passenger.name);
                
                
                _passengersWithParams.Add(passangerWithParam);
            }
        }

        // _coinAppearCoroutine = StartCoroutine(CoinAppearCoroutine());

    }
    

    public void StartBusStopCorotine()
    {
        
        _busStopCorotine = StartCoroutine(BusStopCorotine());
        if(_busGoCorotine != null)
            StopCoroutine(_busGoCorotine);
       

    }
    public void StartBusGoCoroutine()
    {
        _busGoCorotine = StartCoroutine(BusGoCorotine());
        if(_busStopCorotine != null)
            StopCoroutine(_busStopCorotine);
      
    }
    
    IEnumerator BusStopCorotine()
    {
        while (true)
        {
            isBusStopCoroutineFinished = false;
            
            var listSize = _passengersWithParams.Count;
            // Debug.Log(listSize);

            // Random.InitState((int)DateTime.Now.Ticks);
            var passengerToAppearIndex = Random.Range(0, listSize-1);
            
            // Debug.Log(passengerToAppearIndex);
            
            var passengerToAppearGameObject = _passengersWithParams.ElementAt(passengerToAppearIndex).passenger.gameObject;
            
            var passengerWithParams = _passengersWithParams.ElementAt(passengerToAppearIndex);
            
            var passangerCoin = passengerToAppearGameObject.transform.GetChild(0).gameObject;
            
            

            foreach (var passengerWithParamsElement in (_passengersWithParams.ToList()))
            {
                var passengerWithParamsElementCoin =
                    passengerWithParamsElement.passenger.gameObject.transform.GetChild(0).gameObject;
                
                if (passengerWithParamsElement.hasCoin)
                {
                    
                    Debug.Log(passengerWithParamsElement.passenger.name);
                    yield return new WaitForSeconds(0.15f);
                    
                    passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color = Color.red;
                    coinFatigue.Play();
                    GameController.Instance.AddScoreConductor(100);
                    
                    
                    passengerWithParamsElement.hasCoin = false;
                    passengerWithParamsElement.timePassed = 0;
                    passengerWithParamsElement.startTime = DateTime.Now;
                    
                    
                    var _opacity = 1f;
                    while (true)
                    {
                        _opacity -= 0.15f;
                    
                        passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color = 
                            new Color(passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color.r,
                                passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color.g,
                                passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color.b, _opacity);
                        
                        passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color = 
                            new Color(passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.r,
                                passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.g,
                                passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.b, _opacity);

                        if (_opacity <= 0f)
                        {
                            passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color = 
                                new Color(passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color.r,
                                    passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color.g,
                                    passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color.b, 0f);
                            passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color = 
                                new Color(passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.r,
                                    passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.g,
                                    passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.b, 0f);
                            break;
                        }
                        
                    
                
                    
                        yield return new WaitForSeconds(.05f);
                    }
                    
                    passengerWithParamsElement.passenger.GetComponent<SpriteRenderer>().color = Color.white;
                    passengerWithParamsElement.passenger.SetActive(false);
                    passengerWithParamsElementCoin.SetActive(false);
                    
                    



                    
                }
            }

            if (!passengerToAppearGameObject.activeSelf)
            {
                passengerToAppearGameObject.SetActive(true);
                passangerCoin.SetActive(false);
                passengerWithParams.hasCoin = false;
                passengerWithParams.timePassed = 0;
                passengerWithParams.startTime = DateTime.Now;
                
                passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
                new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
                    passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
                    passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            
                
                
                var _opacity = 0f;
            
                while (true)
                {
                    _opacity += 0.15f;
                    passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
                        new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
                            passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
                            passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, _opacity);

                    if (_opacity >= 1f)
                    {
                        passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
                            new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
                                passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
                                passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, 1f);
                        
                        break;
                    }
                    
                    
                    yield return new WaitForSeconds(.05f);
                }
            
            }
            // else
            // {
            //     passengerWithParams.timePassed  = (DateTime.Now - passengerWithParams.startTime).Seconds;
            //     // Debug.Log(passengerWithParams.timePassed);
            //     
            //     if(passengerWithParams.timePassed > 8f)
            //     {
            //         var _opacity = 1f;
            //         while (true)
            //         {
            //             _opacity -= 0.15f;
            //         
            //             passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
            //                 new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
            //                     passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
            //                     passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, _opacity);
            //             
            //             passangerCoin.GetComponent<SpriteRenderer>().color = 
            //                 new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
            //                     passangerCoin.GetComponent<SpriteRenderer>().color.g,
            //                     passangerCoin.GetComponent<SpriteRenderer>().color.b, _opacity);
            //
            //             if (_opacity <= 0f)
            //             {
            //                 passangerCoin.GetComponent<SpriteRenderer>().color = 
            //                     new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
            //                         passangerCoin.GetComponent<SpriteRenderer>().color.g,
            //                         passangerCoin.GetComponent<SpriteRenderer>().color.b, 0f);
            //                 passangerCoin.GetComponent<SpriteRenderer>().color = 
            //                     new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
            //                         passangerCoin.GetComponent<SpriteRenderer>().color.g,
            //                         passangerCoin.GetComponent<SpriteRenderer>().color.b, 0f);
            //                 break;
            //             }
            //             
            //     
            //     
            //         
            //             yield return new WaitForSeconds(.05f);
            //         }
            //         
            //         passengerToAppearGameObject.SetActive(false);
            //         passangerCoin.SetActive(false);
            //         passengerWithParams.hasCoin = false;
            //         
            //     }
            //
            //     
            // }

            isBusStopCoroutineFinished = true;
            yield return new WaitForSeconds(0.25f);
            
            
            
        }
    }
    
    IEnumerator BusGoCorotine()
    {
        while (true)
        {
            isBusGoCoroutineFinished = false;

            var listSize = _passengersWithParams.Count;
            // Debug.Log(listSize);

            // Random.InitState((int)DateTime.Now.Ticks);
            var passengerToAppearIndex = Random.Range(0, listSize-1);
            
            // Debug.Log(passengerToAppearIndex);
            
            var passengerToAppearGameObject = _passengersWithParams.ElementAt(passengerToAppearIndex).passenger.gameObject;
            
            var passengerWithParams = _passengersWithParams.ElementAt(passengerToAppearIndex);


            var passangerCoin = passengerToAppearGameObject.transform.GetChild(0).gameObject;
            
            // Debug.Log(passangerCoin.name);


            foreach (var passengerWithParamsElement in (_passengersWithParams.ToList()))
            {
                
                passengerWithParamsElement.timePassed  = (DateTime.Now - passengerWithParamsElement.startTime).Seconds;
                var passengerWithParamsElementCoin =
                    passengerWithParamsElement.passenger.gameObject.transform.GetChild(0).gameObject;
                
                if (passengerWithParamsElement.timePassed > 7 && passengerWithParamsElement.timePassed < 10f && !passengerWithParamsElement.hasCoin && passengerWithParamsElement.passenger.gameObject.activeSelf)
                {
                    passengerWithParamsElement.passenger.transform.GetChild(1).gameObject.SetActive(true);
                }
                
                // Debug.Log(passengerWithParams.timePassed);

                if (passengerWithParamsElement.timePassed > 10f && !passengerWithParamsElement.hasCoin && passengerWithParamsElement.passenger.gameObject.activeSelf)
                {
                    passengerWithParamsElement.passenger.transform.GetChild(1).gameObject.SetActive(false);
                    passengerWithParamsElementCoin.SetActive(true);
                    // Debug.Log(passengerWithParams.timePassed + "WITHOUT COIN");
                    passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color = 
                        new Color(passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.r,
                            passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.g,
                            passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.b, 0f);
                    
                    

                    var _opacity = 0f;
                    while (true)
                    {
                        _opacity += 0.05f;
                    
                        passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color = 
                            new Color(passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.r,
                                passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.g,
                                passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.b, _opacity);

                        if (_opacity >= 1f)
                        {
                            passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color = 
                                new Color(passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.r,
                                    passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.g,
                                    passengerWithParamsElementCoin.GetComponent<SpriteRenderer>().color.b, 1f);
                            break;
                        }
                        
                
                
                    
                        yield return new WaitForSeconds(.005f);
                    }

                    passengerWithParamsElement.timePassed = 0;
                    passengerWithParamsElement.hasCoin = true;
                    passengerWithParamsElement.startTime = DateTime.Now;
                    
                    

                }
                
                
                
                
                
                
            }


            



            isBusGoCoroutineFinished = true;
            yield return new WaitForSeconds(0.25f);
        }
    }
    
    
    
    
    
    // IEnumerator CoinAppearCoroutine()
    // {
    //     while (true)
    //     {
    //         
    //         var listSize = _passengersWithParams.Count;
    //         // Debug.Log(listSize);
    //
    //         // Random.InitState((int)DateTime.Now.Ticks);
    //         var passengerToAppearIndex = Random.Range(0, listSize-1);
    //         
    //         // Debug.Log(passengerToAppearIndex);
    //         
    //         var passengerToAppearGameObject = _passengersWithParams.ElementAt(passengerToAppearIndex).passenger.gameObject;
    //         
    //         var passengerWithParams = _passengersWithParams.ElementAt(passengerToAppearIndex);
    //
    //
    //         var passangerCoin = passengerToAppearGameObject.transform.GetChild(0).gameObject;
    //         
    //         // Debug.Log(passangerCoin.name);
    //         
    //         if (!passengerToAppearGameObject.activeSelf)
    //         {
    //             passengerToAppearGameObject.SetActive(true);
    //             passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
    //             new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
    //                 passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
    //                 passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, 0f);
    //         
    //             
    //             passangerCoin.SetActive(false);
    //         
    //             
    //     
    //     
    //     
    //             var _opacity = 0f;
    //         
    //             while (true)
    //             {
    //                 _opacity += 0.05f;
    //                 passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
    //                     new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
    //                         passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
    //                         passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, _opacity);
    //             
    //                 passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                     new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                         passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                         passangerCoin.GetComponent<SpriteRenderer>().color.b, _opacity);
    //
    //                 if (_opacity >= 1f)
    //                 {
    //                     passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
    //                         new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
    //                             passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
    //                             passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, 1f);
    //             
    //                     passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                         new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                             passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                             passangerCoin.GetComponent<SpriteRenderer>().color.b, 1f);
    //                     break;
    //                 }
    //             
    //             
    //                 
    //                 yield return new WaitForSeconds(.1f);
    //             }
    //         
    //         }
    //         else
    //         {
    //            
    //             
    //             passengerWithParams.timePassed  = (DateTime.Now - passengerWithParams.startTime).Seconds;
    //             // Debug.Log(passengerWithParams.timePassed);
    //
    //             if (passengerWithParams.timePassed > 10f && !passengerWithParams.hasCoin)
    //             {
    //                 // Debug.Log(passengerWithParams.timePassed + "WITHOUT COIN");
    //                 passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                     new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                         passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                         passangerCoin.GetComponent<SpriteRenderer>().color.b, 0f);
    //                 passangerCoin.SetActive(true);
    //
    //                 var _opacity = 0f;
    //                 while (true)
    //                 {
    //                     _opacity += 0.05f;
    //                 
    //                     passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                         new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                             passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                             passangerCoin.GetComponent<SpriteRenderer>().color.b, _opacity);
    //
    //                     if (_opacity >= 1f)
    //                     {
    //                         passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                             new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                                 passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                                 passangerCoin.GetComponent<SpriteRenderer>().color.b, 1f);
    //                         break;
    //                     }
    //                     
    //             
    //             
    //                 
    //                     yield return new WaitForSeconds(.1f);
    //                 }
    //
    //                 passengerWithParams.timePassed = 0;
    //                 passengerWithParams.hasCoin = true;
    //
    //             }
    //             if(passengerWithParams.timePassed > 15f && passengerWithParams.hasCoin)
    //             {
    //                 // Debug.Log(passengerWithParams.timePassed + "WITH COIN");
    //
    //                 var _opacity = 1f;
    //                 while (true)
    //                 {
    //                     _opacity -= 0.05f;
    //                 
    //                     passengerToAppearGameObject.GetComponent<SpriteRenderer>().color = 
    //                         new Color(passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.r,
    //                             passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.g,
    //                             passengerToAppearGameObject.GetComponent<SpriteRenderer>().color.b, _opacity);
    //                     
    //                     passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                         new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                             passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                             passangerCoin.GetComponent<SpriteRenderer>().color.b, _opacity);
    //
    //                     if (_opacity <= 0f)
    //                     {
    //                         passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                             new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                                 passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                                 passangerCoin.GetComponent<SpriteRenderer>().color.b, 0f);
    //                         passangerCoin.GetComponent<SpriteRenderer>().color = 
    //                             new Color(passangerCoin.GetComponent<SpriteRenderer>().color.r,
    //                                 passangerCoin.GetComponent<SpriteRenderer>().color.g,
    //                                 passangerCoin.GetComponent<SpriteRenderer>().color.b, 0f);
    //                         break;
    //                     }
    //                     
    //             
    //             
    //                 
    //                     yield return new WaitForSeconds(.1f);
    //                 }
    //                 
    //                 passengerToAppearGameObject.SetActive(false);
    //                 passangerCoin.SetActive(false);
    //                 passengerWithParams.hasCoin = false;
    //                 
    //             }
    //
    //             
    //         }
    //         
    //         
    //         
    //         
    //     
    //         yield return new WaitForSeconds(5f);
    //
    //
    //
    //     }
    //
    //
    // }

    
    void Update()
    {
        foreach (var passenger in _passengersWithParams.ToList())
        {
            if (!passenger.passenger.gameObject.transform.GetChild(0).GetComponent<Collider2D>().isTrigger)
            {
                Debug.Log("DETECTED PLAYER COLLECTED COIN " + passenger.passenger.name);
                passenger.hasCoin = false;
                passenger.timePassed = 0;
                passenger.startTime = DateTime.Now;
                passenger.passenger.gameObject.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;

            }
        }
       
    }
}
