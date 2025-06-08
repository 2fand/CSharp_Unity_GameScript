using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleRun : MonoBehaviour
{
    public float dieTime = 2f;
    public Vector3 direction = Vector3.forward * 0.05f;

    private IEnumerator toDie()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(dieTime / 10);
            gameObject.GetComponent<MeshRenderer>().material.color -= new Color(0, 0, 0, 0.1f);
        }
        gameObject.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(toDie());
    }
    void Update()
    {
        transform.localPosition += direction;
    }
}
