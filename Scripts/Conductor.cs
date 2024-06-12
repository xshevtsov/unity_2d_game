// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Conductor : MonoBehaviour
// {
//
//     //кондуктор
//     public GameObject conductor;
//     Animator conductorAnimator;
//     SpriteRenderer conductorSprite;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         conductorAnimator = conductor.GetComponent<Animator>();
//         conductorSprite = conductor.GetComponent<SpriteRenderer>();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         if (Script.gameActivity_Conductor)
//         {
//             //ИГРОВАЯ АКТИВНОСТЬ КОНДУКТОРА
//             if (Script.gameActivity_Conductor)
//             {
//
//                 if (Script.gameActivity_Process == 7)
//                 {
//                     WalkConductor();
//                 }
//             }
//         }
//     }
//
//     private int conductorMoveDirection = -1;
//     private void WalkConductor()
//     {
//
//         conductorAnimator.SetBool("isWalk", true);
//
//         Vector3 directionConductor = transform.right * conductorMoveDirection;
//
//         if (conductor.transform.position.x < -7.3)
//         {
//             //directionConductor = transform.right;
//             conductorSprite.flipX = false;
//             conductorMoveDirection = 1;
//         }
//         else if (conductor.transform.position.x > 5.5)
//         {
//             //directionConductor = -transform.right;
//             conductorSprite.flipX = true;
//             conductorMoveDirection = -1;
//         }
//
//         conductor.transform.position = Vector3.MoveTowards(conductor.transform.position,
//             conductor.transform.position + directionConductor, 1.3f * Time.deltaTime);
//     }
// }
