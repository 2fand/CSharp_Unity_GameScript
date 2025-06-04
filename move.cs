using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public string upButton = "w";
    public string leftButton = "a";
    public string downButton = "s";
    public string rightButton = "d";
    public int moveSpeed = 10;
    public int spinSpeed = 5;
    
    void Start()
    {

    }

    
    void Update()
    {
        if (Input.GetButton(upButton))
        {
            transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
        if (Input.GetButton(leftButton))
        {
            transform.Rotate(0, -spinSpeed, 0);
        }
        if (Input.GetButton(downButton))
        {
            transform.Translate(0, 0, -moveSpeed * Time.deltaTime);
        }
        if (Input.GetButton(rightButton))
        {
            transform.Rotate(0, spinSpeed, 0);
        }
    }
}
