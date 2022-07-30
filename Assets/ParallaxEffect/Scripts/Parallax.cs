using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplayer;
    private Transform camera;
    private Vector3 lastPositionCamera;
    private float widthOfTexture;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        lastPositionCamera = camera.position;
        widthOfTexture = GetWidthOfSprite();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaPosition = camera.position - lastPositionCamera;
        transform.position += deltaPosition * parallaxMultiplayer;
        lastPositionCamera = camera.position;
        if (Mathf.Abs(camera.position.x - transform.position.x) >= widthOfTexture) 
        {
            float offset = (camera.position.x - transform.position.x) % widthOfTexture;
            transform.position = new Vector3(camera.position.x + offset, transform.position.y, transform.position.z);
        }
    }


    private float GetWidthOfSprite() 
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture2D = sprite.texture;
        float textureWidthInUnits = texture2D.width / sprite.pixelsPerUnit;
        return textureWidthInUnits;
    }
}
