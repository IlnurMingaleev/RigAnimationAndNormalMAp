using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        OnDirectionChanged();
    }


    public void OnDirectionChanged() 
    {
        
    }
}
