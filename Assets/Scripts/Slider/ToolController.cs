using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class ToolController : MonoBehaviour
{
    [SerializeField] private Transform beginDot;
    public Transform endDot;
    [SerializeField] private Transform accelerationHandle;
    [SerializeField] private Transform speedHandle;
    [SerializeField] private Transform decelerationHandle;
    [SerializeField] private LineRenderer lineRenderer;
    private Transform mainCam;
    public Vector3 endDotOffset;

    private void Awake()
    {
        endDotOffset = new Vector3(0.02f, 0f, 0f);
    }
    private void Start()
    {
        mainCam = Camera.main.transform;
        UpdateLineRendererPositions();
    }

    private void Update()
    {
        UpdateLineRendererPositions();
    }

    public void UpdateLineRendererPositions() 
    {

        
        Vector3[] handlePositions = new Vector3[5] {
            beginDot.position,
            accelerationHandle.position,
            speedHandle.position,
            decelerationHandle.position,
            endDot.position
        };
        //lineRenderer.gameObject.SetActive(false);
        lineRenderer.SetPositions(handlePositions);
        //lineRenderer.gameObject.SetActive(false);
    }

    
}
