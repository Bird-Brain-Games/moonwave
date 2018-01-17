using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killBox : MonoBehaviour
{
    // Logging    l_ is used to indicate a variable is a logging variable
    public int l_deaths;
    
    void Start()
    {
        l_deaths = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        // When player leaves kill box, call player's knockout funciton
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<KnockOut>().PlayerKnockedOut();
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            other.gameObject.GetComponent<Bullet>().BulletOutOfBounds();
        }

    }

}
