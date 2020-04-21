using System.Collections;
using SaveBilly;
using UnityEngine;

[SelectionBase]
public class ChunkFood : FoodBase
{
   public GameObject[] chunks;

   private int lastIndex;

   public override void SetValue(float v)
   {
      var t = Mathf.InverseLerp(0, _consumeValue, v) * chunks.Length;

      var divIndex = Mathf.RoundToInt(t);

      for (int i = 0; i < chunks.Length; i++)
      {
         var chunk = chunks[i];

         if (i < divIndex)
         {
            if (!chunk.activeSelf) chunk.SetActive(true);
         }
         else if (chunk.activeSelf)
         {
            StartCoroutine(ShrinkToTarget(chunk.transform, BillyController.instance.mouthTransform));
         }
      }
   }
}