using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    public Vector3 movementVector;

    public float period = 2f;

    [SerializeField][Range(0,1)] float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon){ //Stop the NAN error
            return;
        }
        float cycle = Time.time / period; //Continue growing

        const float tau = Mathf.PI * 2; // constant value of 6.283
        float rawSinWave = Mathf.Sin(cycle * tau); // going from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2; //recalculated to go from 0 to 1

        Vector3 Offset = movementVector * movementFactor;
        transform.position = startingPosition + Offset;
        // transform.rotation.SetFromToRotation(startingPosition,transform.position);
    }
}
