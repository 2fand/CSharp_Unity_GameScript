using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addTransfrom : MonoBehaviour
{
    public Vector3 addPos;
    public bool posLocation;
    public Vector3 addRotation;
    public Vector3 addScale;
    public Transform thisAddTrans;
    public bool secondAdd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (null != thisAddTrans)
        {
            if (posLocation)
            {
                transform.localPosition += thisAddTrans.position * (secondAdd ? Time.deltaTime : 1);
            }
            else
            {
                transform.position += thisAddTrans.position * (secondAdd ? Time.deltaTime : 1);
            }
            transform.Rotate(thisAddTrans.rotation.eulerAngles * (secondAdd ? Time.deltaTime : 1));
            transform.localScale += thisAddTrans.localScale * (secondAdd ? Time.deltaTime : 1);
        }
        else
        {
            if (posLocation)
            {
                transform.localPosition += addPos * (secondAdd ? Time.deltaTime : 1);
            }
            else
            {
                transform.position += addPos * (secondAdd ? Time.deltaTime : 1);
            }
            transform.Rotate(addRotation * (secondAdd ? Time.deltaTime : 1));
            transform.localScale += addScale * (secondAdd ? Time.deltaTime : 1);
        }
    }
}
