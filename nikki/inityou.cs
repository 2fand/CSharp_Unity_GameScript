using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inityou : MonoBehaviour
{
    public AudioClip defaultWalkSound;
    public AudioClip effectEqiupSound;
    public AudioClip effectCancelEqiupSound;
    public Material[] screens;
    public GameObject[] effects;
    public AudioClip[] effectWalkSounds;
    void Awake()
    {
        you.defaultWalkSound = defaultWalkSound ?? you.defaultWalkSound;
        you.effectEqiupSound = effectEqiupSound ?? you.effectEqiupSound;
        you.effectCancelEqiupSound = effectCancelEqiupSound ?? you.effectCancelEqiupSound;
        you.screens = screens ?? you.screens;
        you.effects = effects ?? you.effects;
        you.effectWalkSounds = effectWalkSounds ?? you.effectWalkSounds;
    }

    void Update()
    {
        
    }
}
