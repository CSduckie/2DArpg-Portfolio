using System;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private SkillConfig skillConfig;
    private PlayerController player;

    private float weaponDamage;
    private float stunValue;
    private Vector2 repelDir;
    private float freezeTime;
    private float impulseValue;
    private Coroutine currentCoroutine;
    private void Start()
    {
        player = GameManager.instance.player;
        weaponDamage = skillConfig.releaseData.attackData.hitData.value;
        repelDir = skillConfig.releaseData.attackData.hitData.RepelVelocity;
        stunValue = skillConfig.releaseData.attackData.hitData.stunValue;
        freezeTime = skillConfig.releaseData.attackData.FreezeFrameTime;
        impulseValue = skillConfig.releaseData.attackData.ScreenImpulseValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other && other.CompareTag("Enemy"))
        {
            if(other.GetComponent<EnemyController>().isDead)
                return;
            
            HitEnemy(other.gameObject);
            
        }
    }

    public void HitEnemy(GameObject enemy)
    {
        //启用打击感效果
        StartFreezeFrame(freezeTime);
        //摄像机震动,由自身进行调用的效果
        player.impulseSource.GenerateImpulse(impulseValue);
            
        CharacterStats _attackerStats = GetComponentInParent<CharacterStats>();
            
        int attackDir = player.isFacingRight?1:-1;
            
        //TODO:后续需要添加damage的修改值
        enemy.GetComponent<CharacterStats>().TakeDamage(weaponDamage ,stunValue, attackDir,repelDir,_attackerStats);
    }
    
    #region 打击感配置
    //停止当前所有携程
    public void StopAllCoro()
    {
        if (currentCoroutine != null)
        {
            StopAllCoroutines();
            player.sprite.anim.speed = 1;
        }
    }
    
    //卡肉效果
    private void StartFreezeFrame(float time)
    {
        if(time > 0)
        {
            currentCoroutine = StartCoroutine(DoFreezeFrame(time));
        }
    }
    
    private IEnumerator DoFreezeFrame(float time)
    {
        player.sprite.anim.speed = 0;
        yield return new WaitForSeconds(time);
        player.sprite.anim.speed = 1;
        currentCoroutine = null;
    }

    
    #endregion
}
