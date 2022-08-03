using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayTween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        LeanTween.cancelAll();
        LeanTween.rotateZ(gameObject, 50, 4).setEasePunch();
    }
}
