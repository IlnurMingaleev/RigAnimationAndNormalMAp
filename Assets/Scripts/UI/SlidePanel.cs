using UnityEngine;

public class SlidePanel : MonoBehaviour
{
    private float panelHeight;
    private RectTransform panel;
    private float buttonTabHeight;

    [SerializeField] private RectTransform panelRectTRansform;
    [SerializeField] private RectTransform buttonTabs;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform lastPoint;
    [SerializeField] private PointPosition pointPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        panelHeight = panelRectTRansform.rect.height;
        buttonTabHeight = buttonTabs.rect.height;
        panel = GetComponent<RectTransform>();
    }

    
    public void OpenPanel() 
    {
        panel.anchoredPosition = new Vector2(0, -(panelHeight + buttonTabHeight));
        pointPosition.UpdateLineRendererPositions();
      
    }
    public void ClosePanel()
    {
        panel.anchoredPosition = new Vector2(0, 0);
       
        
    }
}
