using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class meshAnimation : MonoBehaviour
{
    private Mesh[] last_meshes;
    public Mesh[] meshes;
    int p = 0;
    public float waitTime = 0.1f;
    bool isEnd = true;
    public animationMode animationMode = animationMode.loop;
    IEnumerator showPicture()
    {
        isEnd = false;
        if (null != meshes && meshes.Length != 0)
        {
            if (null != GetComponent<MeshFilter>())
            {
                GetComponent<MeshFilter>().mesh = meshes[p];
            }
            else
            {
                GetComponent<SkinnedMeshRenderer>().sharedMesh = meshes[p];
            }
            animationRun.run(ref p, ref meshes, animationMode);
            yield return new WaitForSeconds(waitTime);
        }
        isEnd = true;
    }

    void Start()
    {
        StartCoroutine(showPicture());
        last_meshes = meshes;
    }

    void Update()
    {
        if (!npcMove.stopAnimation)
        {
            if (last_meshes != meshes)
            {
                last_meshes = meshes;
                p = 0;
            }
            if (isEnd && p >= 0 && p < meshes.Length)
            {
                StartCoroutine(showPicture());
            }
        }
    }
}
