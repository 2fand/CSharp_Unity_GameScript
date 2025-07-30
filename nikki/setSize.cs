using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setSize : MonoBehaviour
{
    public Image image;
    public float h;
    public float w; 
    void Start()
    {
        
        //image.gameObject.SetActive(false);
        //image.gameObject.SetActive(true);
    }

    void Update()
    {
        image.rectTransform.sizeDelta = new Vector2(h, w);
    }
}
