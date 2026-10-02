using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitionPattern {
    public class GameController : MonoBehaviour {
        public GameObject friendlyObj;
        public GameObject enemyObj;
        
        //Change materials to detect which enemy is the closest
        public Material enemyMaterial;
        public Material closestEnemyMaterial;
        
        //To get a cleaner workspace, parent all soldiers to these empty gameobjects
        public Transform enemyParent;
        public Transform friendlyParent;
        
        //Store all soldiers in these lists
        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();
        
        //Save the closest enemies to easier change back its material
        List<Soldier> closestEnemies = new List<Soldier>();

        //Grid data
        float mapWidth = 50f;
        int cellSize = 10;

        //Number of soldiers on each team
        int numberOfSoldiers = 100;

        //The Spatial Partition grid
        Grid grid;

		
        void Start() {
            grid = new Grid((int)mapWidth, cellSize);
            
            for (int i = 0; i < numberOfSoldiers; i++) {
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));

                newEnemy.transform.parent = enemyParent;

                randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;

                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));

                newFriendly.transform.parent = friendlyParent;
            }
        }
	
	
        void Update() {
            for (int i = 0; i < enemySoldiers.Count; i++) {
                enemySoldiers[i].Move();
            }

            for (int i = 0; i < closestEnemies.Count; i++) {
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }

            closestEnemies.Clear();

            for (int i = 0; i < friendlySoldiers.Count; i++) {
                Soldier closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);

                if (closestEnemy != null) {
                    closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;

                    closestEnemies.Add(closestEnemy);
                    friendlySoldiers[i].Move(closestEnemy);
                }
            }
        }

        Soldier FindClosestEnemySlow(Soldier soldier) {
            Soldier closestEnemy = null;

            float bestDistSqr = Mathf.Infinity;

            for (int i = 0; i < enemySoldiers.Count; i++) {
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;
                if (distSqr < bestDistSqr) {
                    bestDistSqr = distSqr;

                    closestEnemy = enemySoldiers[i];
                }
            }

            return closestEnemy;
        }
    }
}