using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour
{
    private float c;
    void Start()
    {
        
    }

    void Update()
    {
        c = Mathf.Sin(Time.time / 10) * 0.5f + 0.5f;
        if (!npcMove.stopAnimation)
        {
            GetComponent<MeshRenderer>().material.color = new Color(c, c, c);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(c, c, c));
            if (null == GetComponent<wall>())
            {
                return;
            }
            for (int i = 0; i < GetComponent<wall>().AddVirtualChildren.Count; i++)
            {
                if (null != GetComponent<wall>().AddVirtualChildren[i].GetComponent<MeshRenderer>())
                {
                    GetComponent<wall>().AddVirtualChildren[i].GetComponent<MeshRenderer>().material.color = new Color(c, c, c);
                    GetComponent<wall>().AddVirtualChildren[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(c, c, c));
                }
            }
        }
    }
}
