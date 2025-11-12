using UnityEngine;
using System.Collections;
public class BossSprite : EnemySprite
{

    public float flashTime;
    public GameObject lights;
    
    public void StartFlash()
    {
        StartCoroutine(damageFlash());
    }

    private IEnumerator damageFlash()
    {
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            lights.SetActive(false);
            elapsedTime += Time.deltaTime;
            
            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / flashTime);
            material.SetFloat("_FlashAmount", currentFlashAmount);
            yield return null;
        }
        lights.SetActive(true);
    }
    
}
    