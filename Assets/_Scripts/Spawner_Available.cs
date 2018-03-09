using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Available : MonoBehaviour
{
    int numberColliders;
    // Use this for initialization
    void Start()
    {
        numberColliders = 0;
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetNumColliders() { return numberColliders; }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            numberColliders++;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player left");
            numberColliders--;
        }
    }
}
