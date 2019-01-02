using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        transform.LookAt(target);
    }
    
    void Update()
    {

        if (Vector3.Distance(target.position, transform.position) >= 5 && 
            Vector3.Distance(target.position, transform.position) <= 12)
        {
            transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * 3);
        }
        else
        {
            if (Vector3.Distance(target.position, transform.position) <= 5)
            {
                transform.LookAt(target);
                transform.Translate(Vector3.back);
            }
            if (Vector3.Distance(target.position, transform.position) >= 12)
            {
                transform.LookAt(target);
                transform.Translate(Vector3.forward);
            }
        }

    }
}
