using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.name.Equals("bot") || other.name.Equals("top"))
        {
            Vector3 temp = transform.position;
            temp.y = other.transform.position.y * -0.85f;
            transform.SetPositionAndRotation(temp, transform.rotation);
        }

        if (other.name.Equals("right") || other.name.Equals("left"))
        {
            Vector3 temp = transform.position;
            temp.x = other.transform.position.x * -0.85f;
            transform.SetPositionAndRotation(temp, transform.rotation);
        }
    }
}
