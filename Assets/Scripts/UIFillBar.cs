using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UIFillBar : MonoBehaviour
{
    private Image _maskImage;
    
    public Image _fillImage;

    public TMP_Text valueText;

    [Range(0,1)] public float value;

    public float minValue;
    public float maxValue;

    private bool stopFlashing = false;
    
    private void OnValidate()
    {
        if (_maskImage)
        {
            SetValue(value * 100);
        }
    }

    public void SetValue(float newValue)
    {

        //Debug.Log(name + ": " +  newValue);
        if (_maskImage == null)
            return;
        
        var lastValue = value;
        value = Mathf.InverseLerp(minValue, maxValue, newValue);

        valueText.text = $"{newValue:f0}%";
        
        _maskImage.fillAmount = value;
        if (_fillImage != null)
        {
            if (value > .8f && lastValue < .8f)
            {
                StartCoroutine(StartFlashing());
            }
        
            if (value < .8f && lastValue > .8f)
            {
                stopFlashing = true;
            }
        }
    }

    private IEnumerator StartFlashing()
    {
        Color initialColor = _fillImage.color;
        float t = 0f;

        while (!stopFlashing)
        {
            t += Time.deltaTime*5;
            _fillImage.color = Color.Lerp(initialColor, Color.red, (Mathf.Sin(t - Mathf.PI *.5f) + 1)*.5f);
            yield return null;
        }

        _fillImage.color = initialColor;
        stopFlashing = false;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _maskImage = GetComponentInChildren<Mask>().GetComponent<Image>();
    }
}
