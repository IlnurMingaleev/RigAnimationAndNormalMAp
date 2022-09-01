using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class OnHandleDrag : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    Dictionary<string, int> handles = new Dictionary<string, int>();
    private Vector2 previousHandlePosition;
    private Vector3 currentHandlePosition;
    private Vector2 dragMovement;
    private RectTransform rectTransform;
    private PointerEventData clickData;
    public bool selected = false;
    public UnityEvent onDragEvent;

    private void Awake()
    {
        handles.Add("AccelerationHandle", 1);
        handles.Add("SpeedHandle", 2);
        handles.Add("DecelerationHandle", 3);
        rectTransform = GetComponent<RectTransform>();
        /*clickData = new PointerEventData(EventSystem.current);*/
    }
    void Update() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            selected = true;
        }
        if (selected)
        {
            currentHandlePosition = transform.position;
            lineRenderer.SetPosition(handles[gameObject.name], currentHandlePosition);
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            selected=false;
        }

    }

    /*public void OnEndDrag(PointerEventData eventData)
    {
        selected = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
       selected = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
    }*/
}
