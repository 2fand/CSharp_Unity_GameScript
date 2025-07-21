using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScreen : MonoBehaviour
{
    public float h = 1;
    public float height
    {
        get
        {
            return h;
        }
        set
        {
            h = value;
        }
    }
    public float w = 1;
    public float width
    {
        get
        {
            return w;
        }
        set
        {
            w = value;
        }
    }
    public float x = 0.5f;
    public float y = 0.5f;
    void Start()
    {
        
    }

    void Update()
    {
        GetComponent<Image>().rectTransform.position = new Vector2(Screen.width * x, Screen.height * y);
        GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Screen.width * w, Screen.height * h);
    }
}
