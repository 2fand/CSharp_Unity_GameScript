using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fullscreen : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        GetComponent<Image>().rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}
