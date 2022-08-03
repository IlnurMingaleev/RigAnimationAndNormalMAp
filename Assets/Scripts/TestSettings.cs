using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSettings : MonoBehaviour
{
    [SerializeField] private Image img;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Tween() 
    {
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0.5f, 1f).setDelay(1f);
        Invoke("SetAlphaBack", 2);
    }
    public void SetAlphaBack() 
    {
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 1f, 1f).setDelay(1f);
    }
}
