using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float startTime;
    private bool isActive;
    private float finishTime;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) 
        {
            float time = Time.time;
            if (time >= finishTime) 
            {
                isActive = false;
            }
        }
    }

    public void SetupTimer(float duration) 
    {
        startTime = Time.time;
        isActive = true;
        finishTime = startTime + duration;
    }
}
