using UnityEngine;
using System.Collections;

public class ShielderFightControl : EliteFightControl
{
    public ShielderController shielder;
    
    protected override void ReadyForEliteFight()
    {
        StartCoroutine(ActiveShielderAfterSec());
    }
    
    public void ShielderFightWon()
    {
        eliteFightCamera.Priority = 10;
        normalFollowCamera.Priority = 20;
        airwalls.SetActive(false);
        isEliteFightWon = true;
    }
    
    private IEnumerator ActiveShielderAfterSec()
    {
        yield return new WaitForSeconds(2f);
        shielder.StartShielderFight();
    }
}
