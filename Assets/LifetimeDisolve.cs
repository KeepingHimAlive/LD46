using System.Collections;
using SaveBilly;
using UnityEngine;
public class LifetimeDisolve : MonoBehaviour
{
    public float Lifetime = 5;
    private bool dead;

    public ShrinkCurve curve;
    
    void Update()
    {
        if (dead) return;
        Lifetime -= Time.deltaTime;

        if (Lifetime < 0)
        {
            dead = true;
            StartCoroutine(ShrinkAndDestory(1));
        }        
    }

    private IEnumerator ShrinkAndDestory(float duration = 1)
    {
        var startsize = transform.localScale;
        float t = 0;

        do
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(startsize, Vector3.zero, t);
            yield return null;
            
        } while (t < 1);
        
        Destroy(gameObject);
    }
}
