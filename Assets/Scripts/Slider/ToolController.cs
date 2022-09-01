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
    [SerializeField] private Transform endDot;
    [SerializeField] private Transform accelerationHandle;
    [SerializeField] private Transform speedHandle;
    [SerializeField] private Transform decelerationHandle;
    [SerializeField] private LineRenderer lineRenderer;

    private void Start()
    {
        UpdateLineRendererPositions();
    }



    public void UpdateLineRendererPositions() 
    {
        Vector3[] handlePositions = new Vector3[5] { beginDot.position,
            accelerationHandle.position,
            speedHandle.position,
            decelerationHandle.position,
            endDot.position
        };
        lineRenderer.SetPositions(handlePositions);
    }

    
}
