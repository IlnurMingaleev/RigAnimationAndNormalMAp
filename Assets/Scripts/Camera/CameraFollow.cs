
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTimeX;
    [SerializeField] private float smoothTimeY;
    private Rigidbody2D playerRigidbody;
    private PlayerController controller;
    [SerializeField] private float lookAheadOffsetX;
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
        //float deltaPositionX = Mathf.Lerp(transform.position.x, desiredPosition.x, smoothTimeX * Time.deltaTime);
        //float deltaPositionY = Mathf.Lerp(transform.position.y, desiredPosition.y, smoothTimeY * Time.deltaTime);
        transform.position = new Vector3(deltaPositionX, deltaPositionY, transform.position.z);
    }
}

