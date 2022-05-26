using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnCarCorrectCheckpoint;
    public event EventHandler OnCarWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;

    [SerializeField]
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    public int nextCheckpointSingleIndex;

    private void Awake()
    {
        //Debug.Log("Awake");
        Transform checkpointsTransform = transform.Find("CheckPoints");

        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Car"))
        {
            carTransformList.Add(i.transform);
        }
        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {
        nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            // Correct checkpoint
            //Debug.Log("Correct");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnCarCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
            carTransform.GetComponent<CarAgent>().RewardCheckpoint();
        }
        else
        {
            // Wrong checkpoint
            Debug.Log("Wrong");
            OnCarWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            carTransform.GetComponent<CarAgent>().NegativeRewardCheckpoint();
        }
    }

    public CheckpointSingle GetNextCheckpoint(Transform carTransform)
    {
        nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];

        return checkpointSingleList[nextCheckpointSingleIndex];
    }

    public void ResetCheckpoints(Transform carTransform)
    {
        nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex = 0) % checkpointSingleList.Count;
    }
}
