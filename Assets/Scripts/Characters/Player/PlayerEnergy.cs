using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{

    [SerializeField] EnergyBar energyBar;
    [SerializeField] float overdriveInterval = 0.1f;
    public const int MAX = 100;
    //常量的声明：1、全部使用大写字母  2.有多个单词用下划线分割开  3、常量声明必须在声明时进行初始化
    public const int PECENT = 1;
    int energy;
    bool available = true;


    WaitForSeconds waitForOverdriveInterval;


    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }


    void OnEnable() 
    {
        PlayerOverdrive.on += PlayerOverdriveOn;    
        PlayerOverdrive.off += PlayerOverdriveOff;    
    }




    void OnDisable() 
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;    
        PlayerOverdrive.off -= PlayerOverdriveOff;    
    }



    void Start() 
    {
        energyBar.Initialize(energy,MAX);
        Obtain(MAX);
    }


    //积累能量函数
    public void Obtain(int value)
    {
        if(energy == MAX || available == false || gameObject.activeSelf == false) return;
        // energy += value;
        energy = Mathf.Clamp(energy + value,0,MAX);
        energyBar.UpdateStates(energy,MAX);
    }
    //消耗能量函数
    public void Use(int value)
    {
        energy -=value;
        energyBar.UpdateStates(energy,MAX);
        if(energy == 0 && !available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }
    //判断所需能量够不够
    public bool IsEnough(int value) => energy < value;



    void PlayerOverdriveOn()
    {
        StartCoroutine(nameof(KeepUsingCoroutine));
        available = false;
    }



    void PlayerOverdriveOff()
    {
        StopCoroutine(nameof(KeepUsingCoroutine));
        available = true;
    }

    IEnumerator KeepUsingCoroutine()
    {
        while(gameObject.activeSelf && energy > 0)
        {
            yield return waitForOverdriveInterval;
            Use(PECENT);
        }
    }

}
