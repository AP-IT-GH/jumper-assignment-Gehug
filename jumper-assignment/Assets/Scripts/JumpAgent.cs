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
    private float randomSpeed;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Beweeg het blokje vooruit langs de X-as
        obstacle.transform.Translate(Vector3.right * randomSpeed * Time.deltaTime);
    }

    public override void OnEpisodeBegin()
    {
        //reset obstacle naar begin positie
        randomSpeed = Random.Range(3f, 7f);
        obstacle.transform.localPosition = new Vector3(0, 0.5f, 5f); // reset obstacle 
        this.gameObject.transform.localPosition = new Vector3(0f, 0.5f, -4f); // reset agent
    }

    public override void CollectObservations(VectorSensor sensor)
    {    // Agent positie    
        sensor.AddObservation(this.transform.localPosition.y);
/*        sensor.AddObservation(obstacle.transform.localPosition.x);*/
        sensor.AddObservation(randomSpeed);
    }

    public float jumpForce = 30f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2;


        bool jump = actionBuffers.DiscreteActions[0] == 1;
        print(actionBuffers.DiscreteActions[0]);

        if (jump & onGround) // if jump button is pressed and is on the ground
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }


        float distanceToTarget = Vector3.Distance(this.transform.localPosition, obstacle.transform.localPosition);
        if (distanceToTarget < 1.42f ) {


            SetReward(-1);
            EndEpisode();


        }

        if (obstacle.transform.position.y < 0)
        {
            EndEpisode();
            

        }

       





    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;


        DiscreteActionsOut[0] = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        
    }



    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onGround = true;
        }



    }






}
