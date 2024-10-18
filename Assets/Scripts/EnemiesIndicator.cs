// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemiesIndicator : MonoBehaviour
// {
//     [HideInInspector] public List<BaseEnemyStateMachine> enemies;

//     // Start is called before the first frame update
//     void Start()
//     {
//         this.enemies = GetComponent<WaveSpawner>().enemies;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         this.enemies = GetComponent<WaveSpawner>().enemies;

//         foreach(BaseEnemyStateMachine enemy in enemies)
//         {
//             Debug.Log(enemy.name + " - " + Camera.main.WorldToViewportPoint(enemy.transform.position));

//             Vector3 enemyPositionInScreen = Camera.main.WorldToViewportPoint(enemy.transform.position);
//             if(enemyPositionInScreen.x < 0 || enemyPositionInScreen.y < 0)
//             {
//                 Debug.Log(enemy.name + " - " + Camera.main.WorldToViewportPoint(enemy.transform.position));
//             }
//         }
//     }
// }
