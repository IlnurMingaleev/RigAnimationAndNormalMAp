using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLeanTween : MonoBehaviour
{
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject startGame;
    [SerializeField] private GameObject back;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackButtonClicked() 
    {
        LeanTween.cancelAll();
        LeanTween.scale(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 2).setEasePunch();
        

    }
    public void StartGame()
    {
        LeanTween.alpha(gameObject, 0.0f, 2).setEasePunch();
    }





    public void CancelTween() 
    {
        LeanTween.cancelAll();
    }
}
