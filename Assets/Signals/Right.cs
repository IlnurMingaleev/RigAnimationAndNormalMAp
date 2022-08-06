using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Right : MonoBehaviour
{
    Image img;
    [SerializeField] private Sprite right;
    [SerializeField] private Sprite left;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    public void OnRightSiganalRecieved()
    {
        img.sprite = right;
        gameObject.SetActive(true);
    }
    public void OnLeftSiganalRecieved()
    {
        img.sprite = left;
    }
}
