using System.Collections;
using TMPro;
using UnityEngine;

namespace SaveBilly
{
   [SelectionBase]
   public abstract class FoodBase : MonoBehaviour
   {
      [SerializeField] public FoodType foodType;
      [SerializeField] protected float _consumeValue = 8;
      [SerializeField] protected float _currentValue;
      
      public ShrinkCurve curve;

      private bool consumed = false;

      public abstract void SetValue(float newValue);

      public void EatFood(float rate)
      {
         if (consumed == false)
         {
            _currentValue -= rate * Time.deltaTime;

            SetValue(_currentValue);
            if (_currentValue < 0 )
            {
               consumed = true;
               StartCoroutine(delayedInvoke());
            }
         }
      }

      IEnumerator delayedInvoke()
      {
         yield return new WaitForSeconds(.5f);
         OnFinishedHandler?.Invoke();
      }

      public delegate void OnFinished(); 
      public OnFinished OnFinishedHandler; 
      
      protected virtual void Start()
      {
         _currentValue = _consumeValue;
      }
      
      // protected virtual void OnValidate()
      // {
      //    SetValue(_currentValue);
      // }
      //
      protected IEnumerator ShrinkToTarget(Transform obj, Transform target, float duration = .5f)
      {
         var startPos = obj.position;
         var startScale = obj.localScale;

         float t = 0;
         do
         {
            t += Time.deltaTime / duration;
            var tClamped = Mathf.Clamp01(t);
            obj.localScale = Vector3.Lerp(startScale, Vector3.zero, curve.curve.Evaluate(tClamped));
            obj.position = Vector3.Lerp(startPos, target.position, tClamped);
            yield return null;
         } while (t < 1);
      }
   }
}