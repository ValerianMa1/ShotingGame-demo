using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedBehaviour : StateMachineBehaviour
{



    public static Dictionary<string , System.Action> buttonFunctionTable;



    void Awake() 
    {
        buttonFunctionTable = new Dictionary<string, System.Action>();    
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       UIInput.instance.DisableAllUIInput();
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       buttonFunctionTable[animator.gameObject.name].Invoke();
    }
}
