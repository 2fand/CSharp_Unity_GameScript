using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class get : MonoBehaviour
{
    public AudioClip getSound;
    public Image screen;
    public AudioClip screenSound;
    IEnumerator play()
    {
        you.canMove = false;
        
        //audio
        if (null == GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        //play screen
        screen.enabled = true;
        GetComponent<AudioSource>().PlayOneShot(screenSound);
        yield return new WaitForSeconds(2);
        screen.enabled = false;
        GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(0.01f);
        GetComponent<AudioSource>().PlayOneShot(getSound);
        //show
        GetComponent<Image>().enabled = true;
        transform.GetChild(0).GetComponent<Text>().enabled = true;
        //hide
        yield return new WaitForSeconds(1);
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).GetComponent<Text>().enabled = false;
        //get effect
        you.canMove = true;
        enabled = false;
        yield return null;
    }

    void Start()
    {
        //²¥·ÅÉÁÆÁ¶¯»­ -> ...
        StartCoroutine(play());
    }

    void Update()
    {
        
    }
}
