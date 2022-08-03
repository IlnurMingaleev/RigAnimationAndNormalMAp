using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameTween : MonoBehaviour
{
    [SerializeField] private Image img;
    Color imgColor;
    // Start is called before the first frame update
    void Start()
    {
        imgColor = img.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tween() 
    {
        Color fromColor = imgColor;
        Color toColor = Color.green;

        LeanTween.value(img.gameObject, setColorCallback, fromColor, toColor, .25f);
        Invoke("SetColorBack", 2);

       
        
    }

    private void setColorCallback(Color c)
    {
        img.color = c;

        // For some reason it also tweens my image's alpha so to set alpha back to 1 (I have my color set from inspector). You can use the following

        var tempColor = img.color;
        tempColor.a = 1f;
        img.color = tempColor;
    }

    public void SetColorBack() 
    {
        Color fromColor = Color.green;
        Color toColor = imgColor;
        LeanTween.value(img.gameObject, setColorCallback, fromColor, toColor, .25f);
    }
}
