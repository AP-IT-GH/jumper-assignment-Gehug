using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Google.Protobuf.WellKnownTypes;


[RequireComponent(typeof(Rigidbody))]
public class JumpAgent : Agent
{

    public GameObject obstacle;
    private float obstacleSpeed = 0;

    Rigidbody rb;
    bool onGround = true; 



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {    // Agent positie    
        sensor.AddObservation(this.transform.localPosition.y);

        sensor.AddObservation(obstacleSpeed);
    }

    public float jumpForce = 30f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2;

        bool jump = actionBuffers.DiscreteActions[0] >= 1;

        if (jump & onGround) // if jump button is pressed and is on the ground
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }

    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;
        

        DiscreteActionsOut[0] = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        Input.GetKey(KeyCode.Space);
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onGround = true;
        }


        
    }






}
