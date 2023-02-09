using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions,InputActions.IPauseMenuActions
{
    [Header("components")]
    InputActions inputActions;
    

    //移动两事件
    public event UnityAction<Vector2> onMove = delegate{};
    public event UnityAction onStopMove = delegate{};
    //开火两事件
    public event UnityAction onFire = delegate{};
    public event UnityAction onStopFire = delegate{};
    public event UnityAction onDodge = delegate{};
    public event UnityAction onOverdrive = delegate{};
    public event UnityAction onPause = delegate{};
    public event UnityAction onUnPause = delegate{};

    //初始化
    void OnEnable()
    {
        inputActions = new InputActions();
        //登记Gameplay动作表得回调函数，传入参数是IGameplayActions接口，这里本身就继承了直接用this
        inputActions.Gameplay.SetCallbacks(this);   
        inputActions.PauseMenu.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisableAllInput();
    }
    
    void SwitchActionMap(InputActionMap actionMap,bool isUIinput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if(isUIinput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //禁用和启用动作表，有时候过场动画会用上，玩家死亡弹ui窗口也用得上
    public void DisableAllInput() => inputActions.Disable();

    public void EnableGameplayInput() =>SwitchActionMap(inputActions.Gameplay,false);
    
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu,true);

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;


    public void OnMove(InputAction.CallbackContext context)
    {
        /*
        Disabled      是动作表关闭阶段
        Waiting         是动作表启用但是没有相应输入信号传入阶段
        Started         是按下按键的那一帧，相当于input.getkeydown()函数
        Performed       是输入动作已经执行，包含按下按键和按住按键两个阶段，相当于input.getkey()
        Canceled        是输入信号停止的时候松开按键的那一帧，相当于input.getkeyup函数        
        */
        if(context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onFire.Invoke();
        }
        if(context.canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onPause.Invoke();
        }
    }


    public void OnUnpause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onUnPause.Invoke();
        }
    }
}
