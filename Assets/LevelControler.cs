using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelControler : MonoBehaviour
{
   private BillyController billy;
   private FoodSpawner spawner;

   public GameObject GameOverScreen;
   public TMP_Text GameOverText;
   public TimePanel timePanel;
   private static LevelControler _instance;
   public static LevelControler Instance => _instance;

   private void Awake()
   {
      if (_instance != null)
      {
         Destroy(this);
      }
      _instance = this;
      billy = FindObjectOfType<BillyController>();
      spawner = FindObjectOfType<FoodSpawner>();
   }

   public void GameOver()
   {
      var agent = billy.GetComponent<NavMeshAgent>();

      agent.enabled = false;
      spawner.enabled =false;
      billy.enabled = false;
      timePanel.enabled = false;

      var t = timePanel.time;
      GameOverText.text = $"Game Over!\nYou lived for: {t:F2} Seconds!";
      GameOverScreen.SetActive(true);
   }

   public void Restart()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      //GameOverScreen.SetActive( false);
   }
   
   public void Exit()
   {
      Application.Quit(-1);
   }
}
