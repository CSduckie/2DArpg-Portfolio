using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
public class ShielderFightControl : MonoBehaviour
{
    public GameObject airwalls;
    private ShielderController shielder;
    
    public CinemachineCamera shielderFightCamera;
    public CinemachineCamera normalFollowCamera;
    
    private void Start()
    {
        shielder = GetComponentInChildren<ShielderController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            airwalls.SetActive(true);
            shielderFightCamera.Priority = 20;
            normalFollowCamera.Priority = 10;
            ReadyForShielderFight();
        }
    }

    private void ReadyForShielderFight()
    {
        StartCoroutine(ActiveShielderAfterSec());
    }

    private IEnumerator ActiveShielderAfterSec()
    {
        yield return new WaitForSeconds(2f);
        shielder.StartShielderFight();
    }

    public void ShielderFightWon()
    {
        shielderFightCamera.Priority = 10;
        normalFollowCamera.Priority = 20;
        airwalls.SetActive(false);
    }
}
