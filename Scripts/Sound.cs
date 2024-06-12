// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Sound : MonoBehaviour
// {
//
//     public AudioSource magadan;
//     public AudioSource subwaySurf;
//
//
//     // Start is called before the first frame update
//     void Start()
//     {
//
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         //ИГРОВАЯ АКТИВНОСТЬ САУНДТРЕКОВ
//         if (Script.gameActivity_Music == 0)//тут должен быть 0
//         {
//             subwaySurf.Play();
//             Script.gameActivity_Music = 1;
//         }
//
//         if (Script.gameActivity_Music == 1)
//         {
//             if (Script.gameActivity_Process == 3)
//             {
//                 subwaySurf.Pause();
//                 magadan.Play();
//                 Script.gameActivity_Music = 2;
//             }
//         }
//
//         if (Script.gameActivity_Music == 2)
//         {
//             if (Script.gameActivity_Process == 4)
//             {
//                 magadan.Stop();
//                 subwaySurf.Play();
//                 Script.gameActivity_Music = 3;
//             }
//         }
//
//     }
// }
