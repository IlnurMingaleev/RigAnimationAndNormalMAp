using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    Dictionary<string, int> handles = new Dictionary<string, int>();
    public Slider mainSlider;
    Vector3 position;

    private void Awake()
    {
        handles.Add("AccelerationSlider", 1);
        handles.Add("SpeedSlider", 2);
        handles.Add("DeselerationSlider", 3);
    }
    // Start is called before the first frame update
    void Start()
    {
        mainSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
