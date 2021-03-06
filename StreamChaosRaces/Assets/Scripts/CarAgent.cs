using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    [SerializeField]
    private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;

    private KartGame.KartSystems.ArcadeKart carController;

    private void Awake()
    {
        carController = GetComponent<KartGame.KartSystems.ArcadeKart>();
    }

    private void Start()
    {
    }

    public override void OnEpisodeBegin()
    {
        trackCheckpoints.ResetCheckpoints(carController.transform);
        transform.position = spawnPosition.position + new Vector3(Random.Range(-2f, +2f), 0, Random.Range(-2f, +2f));
        transform.forward = spawnPosition.forward;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(carController.carSpeed);

        //Primera observacion direccion checkpoint
        Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        //Debug.Log(directionDot);
        sensor.AddObservation(directionDot);

        //Segunda observacion direccion checkpoint
        /*var direction = (trackCheckpoints.GetNextCheckpoint(transform).transform.position - carController.transform.position).normalized;
        sensor.AddObservation(Vector3.Dot(carController.GetComponent<Rigidbody>().velocity.normalized, direction));*/
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = 0f;
        float turnAmount = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0:
                forwardAmount = 0f;
                break;
            case 1:
                forwardAmount = +1f;
                break;
            case 2:
                forwardAmount = -1f;
                break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0:
                turnAmount = 0f;
                break;
            case 1:
                turnAmount = +1f;
                break;
            case 2:
                turnAmount = -1f;
                break;
        }

        //carController.SetInputs(forwardAmount, turnAmount);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            forwardAction = 2;
        }

        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            forwardAction = 2;
        }

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.5f);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.1f);
        }
    }

    public void RewardCheckpoint()
    {
        Debug.Log("reward checkpoint");
        AddReward(1f);
    }

    public void NegativeRewardCheckpoint()
    {
        Debug.Log("negative reward checkpoint");
        AddReward(-1f);
    }
}
