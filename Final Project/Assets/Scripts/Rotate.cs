using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Rotates the powerup, just for effects
        transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
    }
}
