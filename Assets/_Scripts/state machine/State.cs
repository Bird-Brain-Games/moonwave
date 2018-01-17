using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public virtual string m_Name {get; set;}
    // Keeping track of the next state to change to
    bool changingState;
    string nextState;

    StateManager stateManager;
    // Use this for initialization
    void Awake()
    {
        stateManager = GetComponent<StateManager>();
    }

#region Virtual States
    
    public virtual void StateUpdate() {}
    public virtual void StateFixedUpdate() {}
    public virtual void StateLateUpdate() {}

    public virtual void StateOnCollisionEnter(Collision collision) {}
    public virtual void StateOnCollisionStay(Collision collision) {}
    public virtual void StateOnCollisionExit(Collision collision) {}

    public virtual void StateOnTriggerEnter(Collider other) {}
    public virtual void StateOnTriggerStay(Collider other) {}
    public virtual void StateOnTriggerExit(Collider other) {}

    public virtual void StateEnter() {}
    public virtual void StateExit() {}

    public virtual StateManager GetManager()
    {
        return stateManager;
    }
#endregion

    public void ChangeStateUpdate()
    {
        if (changingState)
        {
            stateManager.ChangeState(nextState);
            changingState = false;
        }
        
    }
    
    public void ChangeState(string a_State)
    {
        nextState = a_State;
        changingState = true;
    }

}