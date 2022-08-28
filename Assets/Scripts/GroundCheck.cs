using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float castDistance = 0.01f;
    [SerializeField] private LayerMask platformLayerMask;

    public bool CheckIsOnGround() 
    {
        

        Color color;
        Collider2D playerCollider = gameObject.GetComponent<Collider2D>();
        RaycastHit2D raycastHit;
        bool isGrounded = true;
        if (playerCollider != null) 
        {
            float startX1 = playerCollider.bounds.center.x - playerCollider.bounds.extents.x;
            float startY1 = playerCollider.bounds.center.y - playerCollider.bounds.extents.y;
            float endX1 = startX1;
            float endY1 = startY1 - 0.1f;

            float startX2 = endX1;
            float startY2 = endY1;
            float endX2 = startX1 + playerCollider.bounds.size.x;
            float endY2 = endY1;

            float startX3 = startX1 + playerCollider.bounds.size.x;
            float startY3 = startY1;
            float endX3 = endX2;
            float endY3 = endY2;

            float y = playerCollider.bounds.size.y;
            float x = playerCollider.bounds.center.x;
            raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, castDistance, platformLayerMask);
            if (raycastHit.collider != null)
            {
                color = Color.green;
            }
            else
            {
                color = Color.red;
            }
            Debug.DrawLine(new Vector3(startX1, startY1), new Vector3(endX1, endY1), color);
            Debug.DrawLine(new Vector3(startX2, startY2), new Vector3(endX2, endY2), color);
            Debug.DrawLine(new Vector3(startX3, startY3), new Vector3(endX3, endY3), color);
            //Debug.Log(raycastHit.collider);
            
            isGrounded = raycastHit.collider != null;
        }
        return isGrounded;

    }

    
}
