using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    //Holds all of our items in it.
    [SerializeField]
    State currentState;
    State defaultState;
    string defaultStateString;

    Dictionary<string, State> states;

    void Awake()
    {
        states = new Dictionary<string, State>();
    }

    // Make sure to only change state in the late update of the state
    // So that all the updates can call first [G, C]
    public void ChangeState(string a_State)
    {
        try{
        //Debug.Log("Changing to " + a_State + " from " + currentState.m_Name);

        currentState.StateExit();
        currentState = states[a_State];
        currentState.StateEnter();
        } catch{}
    }

    public void AttachState(string key, State s)
    {
        if (states.ContainsKey(key))    return; // If it's already in the list, don't add it [Graham]

        states.Add(key, s);
        s.m_Name = key;
        if (currentState == null)
        {
            currentState = states[key];
        }
    }

    public void AttachDefaultState(string key, State s)
    {
        try{
        AttachState(key, s);
        defaultState = states[key];
        defaultStateString = key;
        } catch {}
    }

    public void ResetToDefaultState()
    {
        try{
            ChangeState(defaultStateString);
        }
        catch {}
    }

    // Update is called once per frame
    void Update()
    {
        try {
            currentState.StateUpdate();
        } catch{}
    }

    // Update called each physics update
    void FixedUpdate()
    {
        try {
            currentState.StateFixedUpdate();
        } catch{}
    }

    // Update called after other updates
    void LateUpdate()
    {
        try {
            currentState.StateLateUpdate();
            currentState.ChangeStateUpdate();
        } catch{}
    }

    void OnCollisionEnter(Collision collision) {try {currentState.StateOnCollisionEnter(collision);} catch{}}
    void OnCollisionStay(Collision collision) {try{currentState.StateOnCollisionStay(collision);} catch{}}
    void OnCollisionExit(Collision collision) {try{currentState.StateOnCollisionExit(collision);} catch{}}
    void OnTriggerEnter(Collider other) {try{currentState.StateOnTriggerEnter(other);} catch{}}
    void OnTriggerStay(Collider other) {try{currentState.StateOnTriggerStay(other);} catch{}}
    void OnTriggerExit(Collider other) {try{currentState.StateOnTriggerExit(other);} catch{}}
}
