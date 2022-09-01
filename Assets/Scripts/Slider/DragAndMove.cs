using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragAndMove : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private PointerEventData clickData;
    List<RaycastResult> clickResults;

    private List<GameObject> clickedElements;
    bool dragging = false;
    private GameObject draggedObject;
    private Vector3 mousePosition;
    private Vector3 previousMousePosition;
    // Start is called before the first frame update
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
        clickedElements = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
