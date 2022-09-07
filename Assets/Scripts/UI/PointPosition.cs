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
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<UILineRenderer>();
        UpdateLineRendererPositions();
    }

    // Update is called once per frame
    void Update()
    {
       
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

}
