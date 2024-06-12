using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;





    public class PassengerObject
    {
        public GameObject passenger;
        public System.DateTime startTime;
        public int timePassed;
        public bool hasCoin;
         
        public PassengerObject ()
        {
            passenger = null;
            startTime = new System.DateTime();
            timePassed = 0;
            hasCoin = false;
        }        
    }

