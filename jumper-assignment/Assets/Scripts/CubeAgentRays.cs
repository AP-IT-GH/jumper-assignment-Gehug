using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CubeAgentRays : Agent
{

    public Transform Target;
    public GameObject TargetObj;
    public Transform GreenZone;
    public override void OnEpisodeBegin()
    {
        // reset de positie en orientatie als de agent gevallen is
        if (this.transform.localPosition.y < 0)
        {

            this.transform.localPosition = new Vector3(0, 0.5f, 0); this.transform.localRotation = Quaternion.identity;
        }

        // verplaats de target naar een nieuwe willekeurige locatie
        TargetObj.active = true;
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        target_caught = false;
        green_zone_caught = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {    // Agent positie    
        sensor.AddObservation(this.transform.localPosition);
        
        sensor.AddObservation(this.transform.localRotation.y);

        sensor.AddObservation(Vector3.Distance(this.transform.localPosition, GreenZone.localPosition)); // distance to greenzone

    }

    public float speedMultiplier = 0.5f;
    public float rotationMultiplier = 5;
    private bool target_caught = false;
    private bool green_zone_caught = false;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2;
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);
        transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[1], 0.0f);
        // Beloningen
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        // target bereikt   
        if (distanceToTarget < 1.42f & !target_caught)
        {
            SetReward(0.5f);
            target_caught = true;
            TargetObj.active = false;



        }

        if (target_caught & green_zone_caught) // Als target groene zone raakt (als target is gepakt)
        {
            SetReward(0.5f);
            EndEpisode();
        }
        
        // Van het platform gevallen?    
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }


    private void OnCollisionEnter(Collision collision)
    {



        if (collision.gameObject.CompareTag("greenZone") & target_caught) {

            green_zone_caught = true;

        }

    }




}
