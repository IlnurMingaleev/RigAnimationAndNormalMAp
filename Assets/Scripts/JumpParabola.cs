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
    [SerializeField] private Transform gravityHandle;
    [SerializeField] private Transform jumpHeightHandle;
    [SerializeField] private AnnularSlider annularSlider;
    [SerializeField] private Transform dot;
    private float gravityValue;
    private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer gravityLineRenderer;
    private bool selected;

    private float A;
    private float B;
    private float C;
    private bool isGravityValueChanged;
    private float xEnd;
    private float yOffset;
    private float positionScale;

    //Vector3[] lineRendererPositions;
    List<Vector3> lineRendererPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetLineRendererPositions();
        //lineRendererPositions = new List<Vector3>();
    }

    private void Start()
    {
        
        OnDurationValueChanged();
        //FindXEnd();
        SetLineRendererPositions();
        UpdateGravityLineRendererPositions();
        isGravityValueChanged = false;
        
        

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
            UpdateGravityLineRendererPositions();
        }
    }

    public void OnDurationValueChanged() 
    {
        float x1 = point1.position.x;
        float x2 = point2.position.x;
        float xValue = Mathf.Min(x1,x2) + Mathf.Abs(x2  - x1)/2.0f;
        heightSlider.position = new Vector2(xValue, heightSlider.position.y);
        //FindXEnd();
        //xEnd = Mathf.Lerp(point1.position.x, point2.position.x, positionScale);
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
        //xEnd = Mathf.Lerp(1.0f, 10.0f, gravityValue);
        for (float t = 0; t <= 1; t += 0.05f) 
        {
            float x = Mathf.Lerp(point1.position.x, vertex.position.x, t);
            float y = A * x * x + B * x + C;
            //Debug.Log(lineRendererPositions);
            //Debug.Log(gameObject);
            lineRendererPositions.Add(new Vector3(x, y, gameObject.transform.position.z));
        }
        if(gravityValue - 1.0f > float.Epsilon) 
        {
            A = A * gravityValue;
        }
        
        FindXEnd();


        for (float t = 0; t <= 1; t += 0.01f)
        {
            float x = Mathf.Lerp(vertex.position.x, xEnd, t);
            float y = A * (x - vertex.position.x) * (x - vertex.position.x) + vertex.position.y;
            //Debug.Log(lineRendererPositions);
            //Debug.Log(gameObject);
            lineRendererPositions.Add(new Vector3(x, y, gameObject.transform.position.z));
        }
         //FindLineRendererPositions();
         isGravityValueChanged = false;

    }

    public void FindLineRendererPositions() 
    {
        
        if (isGravityValueChanged)
        {
            A = A * gravityValue;
            FindXEnd();
        }
       
        

    }
  /*  public void MoveXEndPosition() 
    {
        float part = 
        xEnd = Mathf.Lerp(point1.position.x, point2.position.x, part);
    }*/
    public void FindXEnd() 
    {
        xEnd = Mathf.Sqrt((point2.position.y  - vertex.position.y)/A) + vertex.position.x;
    }

    public void SetLineRendererPositions() 
    {
        CalculateCoefficients();
        //xEnd = point2.position.x;
        CalculateLineRendererPositions();
        dot.position = lineRendererPositions[lineRendererPositions.Count - 1];
        Vector3[] arrayOfPositions = lineRendererPositions.ToArray();
        lineRenderer.positionCount = arrayOfPositions.Length;
        lineRenderer.SetPositions(arrayOfPositions);
    
    }
    public void UpdateGravityLineRendererPositions() 
    {
        Vector3 grevityHandlePos = gravityHandle.position;
        Vector3 jumpHandlePos = jumpHeightHandle.position;
        Vector3[] gravityLineRendererPositions = new Vector3[] { jumpHandlePos, grevityHandlePos };
        gravityLineRenderer.positionCount = gravityLineRendererPositions.Length;
        gravityLineRenderer.SetPositions(gravityLineRendererPositions);
    }

    public void OnGravityValueChanged() 
    {
        gravityValue = annularSlider.Value;
        

        //positionScale = Mathf.Abs(xEnd - point1.position.x) / Mathf.Abs(point2.position.x - point1.position.x);

    }
}
