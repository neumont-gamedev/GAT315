using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicCharacter : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        move.x = Input.GetAxis("Horizontal") * 5;
        move.y = Input.GetAxis("Vertical") * 5;

        transform.position += move * Time.deltaTime;
    }
}
