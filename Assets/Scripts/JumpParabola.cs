using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class JumpParabola : MonoBehaviour
{
    [SerializeField] private RectTransform point1;
    [SerializeField] private RectTransform jumpHeightHandle;
    [SerializeField] private RectTransform jumpDurationHandle;
    [SerializeField] private RectTransform heightSlider;
    [SerializeField] private RectTransform gravityHandle;
    [SerializeField] private AnnularSlider annularSlider;
    [SerializeField] private RectTransform dot;
    [SerializeField] private Camera cam;
    private float gravityValue;
    private UILineRenderer jumpLineRenderer;
   [SerializeField] private UILineRenderer gravityLineRenderer;
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
        
        jumpLineRenderer = GetComponent<UILineRenderer>();
        SetLineRendererPositions();
        //lineRendererPositions = new List<Vector3>();
    }

    private void Start()
    {
        lineRendererPositions = new List<Vector3>();

        lineRendererPositions.Add(Vector3.zero);
        OnDurationValueChanged();
        //FindXEnd();
        SetLineRendererPositions();
        UpdateGravityLineRendererPositions();
        isGravityValueChanged = false;
        
        

    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetMouseButtonDown(0)) 
         {
             selected = true;
         }
         if (Input.GetMouseButtonUp(0)) 
         { 
             selected = false;

         }
         if (selected) 
         {

         }*/
        /**/
    }

    public void OnDurationValueChanged() 
    {
        float x1 = point1.position.x;
        float x2 = jumpDurationHandle.position.x;
        float xValue = Mathf.Min(x1,x2) + Mathf.Abs(x2  - x1)/2.0f;
        heightSlider.position = new Vector2(xValue, heightSlider.position.y);
        //FindXEnd();
        //xEnd = Mathf.Lerp(point1.position.x, point2.position.x, positionScale);
    }

    public void CalculateA() 
    {
        Vector3 p1 = point1.position;
        Vector3 p2 = jumpHeightHandle.position;
        Vector3 p3 = jumpDurationHandle.position;

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
            B = (-2.0f) * A * jumpHeightHandle.position.x;
        }
        
    }
    public void CalculateC()
    {
        if (A != 0)
        {
            C = A * jumpHeightHandle.position.x * jumpHeightHandle.position.x + jumpHeightHandle.position.y;
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
        //Debug.Log(lineRendererPositions);
        lineRendererPositions.Clear();
        //xEnd = Mathf.Lerp(1.0f, 10.0f, gravityValue);
        for (float t = 0; t <= 1; t += 0.05f) 
        {
            float x = Mathf.Lerp(point1.position.x, jumpHeightHandle.position.x, t);
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
            float x = Mathf.Lerp(jumpHeightHandle.position.x, xEnd, t);
            float y = A * (x - jumpHeightHandle.position.x) * (x - jumpHeightHandle.position.x) + jumpHeightHandle.position.y;
            //Debug.Log(lineRendererPositions);
            //Debug.Log(gameObject);
            lineRendererPositions.Add(new Vector3(x, y, gameObject.transform.position.z));
        }
         //FindLineRendererPositions();
         isGravityValueChanged = false;

    }

    public void FindXEnd() 
    {
        xEnd = Mathf.Sqrt((jumpDurationHandle.position.y  - jumpHeightHandle.position.y)/A) + jumpHeightHandle.position.x;
    }

    public void SetLineRendererPositions() 
    {
        CalculateCoefficients();
        //xEnd = point2.position.x;
        CalculateLineRendererPositions();
        dot.position = lineRendererPositions[lineRendererPositions.Count - 1];
        Vector2[] arrayOfPositions = GetLineRendererPositionsFromList();
        jumpLineRenderer.Points = arrayOfPositions;
    
    }
    public void UpdateGravityLineRendererPositions() 
    {
        Vector2 grevityHandlePos = gravityHandle.position;
        Vector2 jumpHandlePos = jumpHeightHandle.position;
        Vector2[] gravityLineRendererPositions = new Vector2[] { 
            cam.WorldToScreenPoint(jumpHandlePos),
            cam.WorldToScreenPoint(grevityHandlePos)
        };
        gravityLineRenderer.Points = gravityLineRendererPositions;
    }

    public void OnGravityValueChanged() 
    {
        gravityValue = annularSlider.Value;

        UpdateJumpLineRendererPositions();
        //positionScale = Mathf.Abs(xEnd - point1.position.x) / Mathf.Abs(point2.position.x - point1.position.x);

    }
    public Vector2[] GetLineRendererPositionsFromList() 
    {
        Vector2[] arrayLineRendererPositions = new Vector2[lineRendererPositions.Count];
        for (int i = 0; i < arrayLineRendererPositions.Length; i++) 
        {
            arrayLineRendererPositions[i] = cam.WorldToScreenPoint(lineRendererPositions[i]);
        }  
        return arrayLineRendererPositions;
    }
    public void UpdateJumpLineRendererPositions() 
    {
        OnDurationValueChanged();
        SetLineRendererPositions();
        UpdateGravityLineRendererPositions();
    }
}
