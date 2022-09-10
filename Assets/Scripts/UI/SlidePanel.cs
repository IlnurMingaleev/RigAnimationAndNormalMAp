using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePanel : MonoBehaviour
{
    private float panelHeight;
    [SerializeField] private RectTransform panelRectTRansform;
    private RectTransform panel;
    [SerializeField] private RectTransform buttonTabs;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform lastPoint;
    //[SerializeField] private LineRenderer lineRenderer;
    private float buttonTabHeight;
    // Start is called before the first frame update
    void Start()
    {
        panelHeight = panelRectTRansform.rect.height;
        buttonTabHeight = buttonTabs.rect.height;
        panel = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateLineRendererPositions() 
    {
        //lineRenderer.SetPosition(0, startPoint.position);
        //lineRenderer.SetPosition(4, lastPoint.position);
    }
    public void OpenPanel() 
    {
        panel.anchoredPosition = new Vector2(0, -(panelHeight + buttonTabHeight));
        UpdateLineRendererPositions();
        //gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        panel.anchoredPosition = new Vector2(0, 0);
        UpdateLineRendererPositions();
        //gameObject.SetActive(false);
    }
}
