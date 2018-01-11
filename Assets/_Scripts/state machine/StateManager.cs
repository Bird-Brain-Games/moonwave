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
        //Debug.Log("Changing to " + a_State + " from " + currentState.m_Name);

        currentState.StateExit();
        currentState = states[a_State];
        currentState.StateEnter();

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
        AttachState(key, s);
        defaultState = states[key];
        defaultStateString = key;
    }

    public void ResetToDefaultState()
    {
        ChangeState(defaultStateString);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.StateUpdate();
    }

    // Update called each physics update
    void FixedUpdate()
    {
        currentState.StateFixedUpdate();
    }

    // Update called after other updates
    void LateUpdate()
    {
        currentState.StateLateUpdate();
        currentState.ChangeStateUpdate();
    }

    void OnCollisionEnter(Collision collision) {currentState.StateOnCollisionEnter(collision);}
    void OnCollisionStay(Collision collision) {currentState.StateOnCollisionStay(collision);}
    void OnCollisionExit(Collision collision) {currentState.StateOnCollisionExit(collision);}
    void OnTriggerEnter(Collider other) {currentState.StateOnTriggerEnter(other);}
    void OnTriggerStay(Collider other) {currentState.StateOnTriggerStay(other);}
    void OnTriggerExit(Collider other) {currentState.StateOnTriggerExit(other);}
}
