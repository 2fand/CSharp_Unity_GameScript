using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    public bool changeX = true;
    public bool changeY = false;
    public bool changeZ = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (null != GetComponent<npcMove>() && GetComponent<npcMove>().canTurn)
        {
            GetComponent<npcMove>().canTurn = false;
        }
        if (null != Camera.main)
        {
            transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles - new Vector3(changeX ? 90 : 0, changeY ? 90 : 0, changeZ ? 90 : 0));
            for (int i = 0; i < transform.childCount; i++) { 
                transform.GetChild(i).transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles - new Vector3(changeX ? 90 : 0, changeY ? 90 : 0, changeZ ? 90 : 0)); ;
            }
        }
    }
}
