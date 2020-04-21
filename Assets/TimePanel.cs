using TMPro;
using UnityEngine;

[ExecuteAlways]
public class TimePanel : MonoBehaviour
{
   public TMP_Text Minutes;
   public TMP_Text Seconds;

   public float time;

   private void Start()
   {
      Restart();
   }

   // Update is called once per frame
   void Update()
   {
      time += Time.deltaTime;
      UpdateUIText();
   }

   void UpdateUIText()
   {
      Seconds.text = Mathf.FloorToInt(time).ToString();
   }

   void Restart()
   {
      time = 0;
   }
}