using SaveBilly;
using UnityEngine;

[SelectionBase]
public class BlendFood : FoodBase
{
    public override void SetValue(float newValue)
    {
        var nv = Mathf.InverseLerp(0, _consumeValue, newValue);
        
        var smr = GetComponent<SkinnedMeshRenderer>();
        smr.SetBlendShapeWeight(0,nv * 100);
        smr.SetBlendShapeWeight(1, (1 - nv)*100);

        if (newValue < 0)
        {
            StartCoroutine(ShrinkToTarget(transform, BillyController.instance.mouthTransform));
        }
    }
}
