using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{


    [Header(" ====== PLAYER INPUT ===== ")]
    [SerializeField] PlayerInput playerInput;

    
    [Header(" ====== CANVAS ===== ")]
    [SerializeField] Canvas HUDCanvas;
    [SerializeField] Canvas menusCanvas;
    [SerializeField] Canvas waveUICanvas;

    [Header(" ====== BUTTON ===== ")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button mainMenuButtton;


    void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnPause += Unpause;


        ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name,OnResumeButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(optionButton.gameObject.name,OnOptionButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButtton.gameObject.name,OnMainMenuButtonClick);
    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnPause -= Unpause;
    }

    void Pause()
    {
        Time.timeScale = 0;
        HUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        waveUICanvas.enabled = false;
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.instance.SelectUI(resumeButton);
    }

    void Unpause()
    {
        resumeButton.Select();
        resumeButton.animator.SetTrigger("Pressed");
    }


    public void OnResumeButtonClick()
    {
        Time.timeScale = 1;
        HUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        waveUICanvas.enabled = true;
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }



    public void OnOptionButtonClick()
    {
        UIInput.instance.SelectUI(optionButton);
        playerInput.EnablePauseMenuInput();
    }



    public void OnMainMenuButtonClick()
    {
        menusCanvas.enabled =false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

}