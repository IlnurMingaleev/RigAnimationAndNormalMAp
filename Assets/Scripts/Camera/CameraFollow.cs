using UnityEngine;
using UnityEngine.UI;


public class CameraFollow : MonoBehaviour
{
    [Header("Camera Parametrs")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTimeX;
    [SerializeField] private float smoothTimeY;
    [SerializeField] private float lookAheadOffsetX;

    private Rigidbody2D playerRigidbody;
    private PlayerController controller;
    private Vector3 velocity = Vector3.zero;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        controller = playerTarget.gameObject.GetComponent<PlayerController>();
        playerRigidbody = playerTarget.gameObject.GetComponent<Rigidbody2D>();
        camera = gameObject.GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector3 aheadPoint = playerTarget.position + new Vector3(playerRigidbody.velocity.x, 0, 0);
        Vector3 point = camera.WorldToViewportPoint(aheadPoint);
        Vector3 delta = aheadPoint - camera.ViewportToWorldPoint(new Vector3(lookAheadOffsetX,0f, point.z));
        Vector3 desiredPosition = playerTarget.position + offset + delta;
        float deltaPositionX = Mathf.SmoothDamp(transform.position.x, desiredPosition.x, ref velocity.x, smoothTimeX);
        float deltaPositionY = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref velocity.y, smoothTimeY);
        transform.position = new Vector3(deltaPositionX, deltaPositionY, transform.position.z);
    }

    public void OnSliderValueChanged(GameObject sliderGameObject)
    {
        Slider tempSlider = sliderGameObject.GetComponent<Slider>();
        float value = tempSlider.value;
        switch (sliderGameObject.name)
        {
            case "Damping(X) Slider":
                smoothTimeX = value;
                break;
            case "Damping(Y) Slider":
                smoothTimeY = value;
                break;
            case "Lookahead Slider":
                lookAheadOffsetX = value;
                break;

            case "Zoom Slider":
                camera.orthographicSize = value;
                break;
        }

    }
}

