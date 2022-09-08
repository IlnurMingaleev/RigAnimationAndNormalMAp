using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class PointPosition : MonoBehaviour
{
    private UILineRenderer lineRenderer;

    private List<Vector2> listOfPoints;
    [SerializeField] private Camera cam;

    [SerializeField] private RectTransform startPoint;
    [SerializeField] private RectTransform accelerationHandle;
    [SerializeField] private RectTransform speedHandle;
    [SerializeField] private RectTransform decelerationHandle;
    [SerializeField] private RectTransform endPoint;

    [SerializeField] private Canvas canvas;
    private RectTransform canvasRect;
    // Start is called before the first frame update
    void Start()
    {
        canvasRect = canvas.gameObject.GetComponent<RectTransform>();
        lineRenderer = GetComponent<UILineRenderer>();
        UpdateLineRendererPositions();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateLineRendererPositions();  
    }
    public void UpdateLineRendererPositions()
    {


        Vector2[] handlePositions = new Vector2[5] {
            cam.WorldToScreenPoint(startPoint.position),
            cam.WorldToScreenPoint(accelerationHandle.position),
            cam.WorldToScreenPoint(speedHandle.position),
            cam.WorldToScreenPoint(decelerationHandle.position),
            cam.WorldToScreenPoint(endPoint.position),
        };
        //lineRenderer.gameObject.SetActive(false);
        lineRenderer.Points = handlePositions;
        //lineRenderer.gameObject.SetActive(false);
    }

    Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out result);
        return canvas.transform.TransformPoint(result);
    }
    public void OnSliderValueChanged() 
    {
       // UpdateLineRendererPositions();
    
    }

}
