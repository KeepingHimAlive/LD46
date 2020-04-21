using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
   public Vector3 spawnArea;

   //a value of one will be only goodFood;
   [Range(0, 1)] public float goodFoodBias = .5f;

   public float spawRate = 1;

   public BillyController billy;

   public List<GameObject> goodFoods;
   public List<GameObject> goodDrinks;

   public List<GameObject> badFoods;
   public List<GameObject> badDrinks;

   public float spawnTimer;
   [SerializeField]private float _bias;

   public float badFoodRate = .01f;

   private void Start()
   {
      Time.timeScale = 1;
   }

   private void Update()
   {
      spawnTimer -= Time.deltaTime;
      goodFoodBias -= badFoodRate * Time.deltaTime;
      
      
      if (spawnTimer < 0)
      {
         //spawn something and the time till the next one
         spawnTimer += 1 / spawRate;


         var rVal = Random.Range(0, 1f);
         var rVal2 = Random.Range(0, 1f);
         var isGood = rVal < goodFoodBias;

         var diff = billy.hunger - billy.thirst;
         var hungry = diff > 0;
         
         _bias = (diff / 100f + 1)/2 ;

         var spawnFood = rVal2 < _bias; 
         
         GameObject newObject;
         if (isGood)
         {
            newObject = Instantiate(spawnFood 
               ? goodFoods[Random.Range(0, goodFoods.Count)] 
               : goodDrinks[Random.Range(0, goodDrinks.Count)], transform);
         }
         else
         {
            newObject = Instantiate(spawnFood 
               ? badFoods[Random.Range(0, badFoods.Count)] 
               : badDrinks[Random.Range(0, badDrinks.Count)], transform);
         }

         var size = spawnArea / 2;
         bool pointValid = false;

         Vector3 randomPoint = Vector3.zero;
         while (!pointValid)
         {
            randomPoint = new Vector3(Random.Range(-size.x, size.x), 0, Random.Range(-size.z, size.z));
            pointValid = NavMesh.SamplePosition(randomPoint, out var hit, .25f, NavMesh.AllAreas );
            randomPoint = hit.position;
         }
         newObject.transform.position = randomPoint;
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.matrix = transform.localToWorldMatrix;
      Gizmos.DrawWireCube(Vector3.zero, new Vector3(spawnArea.x, 1, spawnArea.z));
   }
}