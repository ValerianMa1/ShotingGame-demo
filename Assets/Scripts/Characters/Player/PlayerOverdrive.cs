using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//通过委托减少类与类之间的耦合

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate{};
    public static UnityAction off = delegate{};



    [SerializeField] GameObject triggerVFX;
    [SerializeField] GameObject engineVFXNormal;
    [SerializeField] GameObject engineVFXOverdrive;
    [SerializeField] AudioData onSFX;
    [SerializeField] AudioData offSFX;



    void Awake()
    {
        on += On;
        off += Off;
    }



    void OnDestroy() 
    {
        on -= On;
        off -= Off;
    }





    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }


    void Off()
    {
        
        triggerVFX.SetActive(false);
        engineVFXNormal.SetActive(true);
        engineVFXOverdrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
