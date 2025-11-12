using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class ShielderSprite : EnemySprite
{
    private ShielderController shielderController;
    //屏幕抖动
    [Header("摄像机震动")]
    public CinemachineImpulseSource impulseSource;
    
    [Header("受伤闪烁")]
    public float flashTime;
    public GameObject lights;
    protected void Awake()
    {
        shielderController = enemy as ShielderController;
        impulseSource = FindFirstObjectByType<CinemachineImpulseSource>();
    }
    
    #region 受伤白色蒙版
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
    #endregion


    #region 动画事件
    public void EnableAttackBox(int _index)
    {
        shielderController.shielderWeapons[_index].SetActive(true);
        impulseSource.GenerateImpulse(shielderController.shielderWeapons[_index].GetComponent<EnemyWeapon>().impulseValue);
    }

    public void DisableAttackBox(int _index)
    {
        shielderController.shielderWeapons[_index].SetActive(false);
    }

    public void AddRecoil()
    {
        shielderController.rb.AddForce( new Vector2( -2 * (shielderController.isFacingRight ? 1 : -1), 0), ForceMode2D.Impulse);
    }

    #endregion
}
