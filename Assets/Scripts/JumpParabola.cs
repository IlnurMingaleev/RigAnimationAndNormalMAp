using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpParabola : MonoBehaviour
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform vertex;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform heightSlider;
    private LineRenderer lineRenderer;
    private bool selected;

    private float A;
    private float B;
    private float C;

    //Vector3[] lineRendererPositions;
    List<Vector3> lineRendererPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetLineRendererPositions();
        //lineRendererPositions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            selected = true;
        }
        if (Input.GetMouseButtonUp(0)) 
        { 
            selected = false;
        
        }
        if (selected) 
        {
            OnDurationValueChanged();
            SetLineRendererPositions();
        }
    }

    public void OnDurationValueChanged() 
    {
        float x1 = point1.position.x;
        float x2 = point2.position.x;
        float xValue = Mathf.Min(x1,x2) + Mathf.Abs(x2  - x1)/2.0f;
        heightSlider.position = new Vector2(xValue, heightSlider.position.y); 
    }

    public void CalculateA() 
    {
        Vector3 p1 = point1.position;
        Vector3 p2 = vertex.position;
        Vector3 p3 = point2.position;

        float x1 = p1.x;
        float y1 = p1.y;
        float x2 = p2.x;
        float y2 = p2.y;
        float x3 = p3.x;
        float y3 = p3.y;

        A = (y3 - ((x3 * (y2 - y1) + x2 * y1 - x1 * y2) /(x2-x1))) / (x3 * (x3 - x1 - x2) + x1 * x2);
    }
    public void CalculateB() 
    {
        if (A != 0) 
        {
            B = (-2.0f) * A * vertex.position.x;
        }
        
    }
    public void CalculateC()
    {
        if (A != 0)
        {
            C = A * vertex.position.x * vertex.position.x + vertex.position.y;
        }

    }

    public void CalculateCoefficients() 
    {
        CalculateA();
        CalculateB();
        CalculateC();
    }

    public void CalculateLineRendererPositions() 
    {
        lineRendererPositions.Clear();
        for (float t = 0; t <= 1; t += 0.01f) 
        {
            float x = Mathf.Lerp(point1.position.x, point2.position.x, t);
            float y = A * x * x + B * x + C;
            //Debug.Log(lineRendererPositions);
            //Debug.Log(gameObject);
            lineRendererPositions.Add(new Vector3(x, y, gameObject.transform.position.z));
        }
        
    }

    public void SetLineRendererPositions() 
    {
        CalculateCoefficients();
        CalculateLineRendererPositions();
        Vector3[] arrayOfPositions = lineRendererPositions.ToArray();
        lineRenderer.positionCount = arrayOfPositions.Length;
        lineRenderer.SetPositions(arrayOfPositions);
    
    }
    public void UpdatePointsPosition() 
    {
        
    }
}
