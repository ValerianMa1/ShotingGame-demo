using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    InputSystemUIInputModule UIInputModule;
    [SerializeField] PlayerInput playerInput;


    protected override void Awake()
    {

        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }




    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }



    public void DisableAllUIInput()
    {
        playerInput.DisableAllInput();
        UIInputModule.enabled = false;
    }
}
