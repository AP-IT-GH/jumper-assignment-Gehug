using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f; // snelheid waarmee het blokje beweegt

    // Update wordt elke frame aangeroepen
    void Update()
    {
        // Beweeg het blokje vooruit langs de X-as
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
