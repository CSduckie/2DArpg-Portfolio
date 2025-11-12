using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
public class EliteFightControl : MonoBehaviour
{
    public GameObject airwalls;
    public CinemachineCamera eliteFightCamera;
    public CinemachineCamera normalFollowCamera;

    protected bool isEliteFightWon = false;
    protected bool eliteFightTriggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !eliteFightTriggered)
        {
            airwalls.SetActive(true);
            eliteFightCamera.Priority = 20;
            normalFollowCamera.Priority = 10;
            ReadyForEliteFight();
            eliteFightTriggered = true;
        }
    }

    protected virtual void ReadyForEliteFight()
    {
        Debug.Log("ReadyForEliteFight");
    }
    
}
