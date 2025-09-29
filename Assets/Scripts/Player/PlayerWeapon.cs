using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private SkillConfig skillConfig;
    private PlayerController player;

    private float weaponDamage;
    private float stunValue;
    private Vector2 repelDir;
    private List<GameObject> hitVFX =  new List<GameObject>();
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
        foreach (var vfx in skillConfig.releaseData.attackData.hitData.SpawnObj)
        {
            GameObject vfxObject = vfx.prefab;
            hitVFX.Add(vfxObject); 
        }
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
        
        //生成一个轻微的随机角度
        float randomZ = UnityEngine.Random.Range(-20f, 20f);
        Quaternion rot = Quaternion.Euler(0f, 0f, randomZ);
        int attackDir = player.isFacingRight?1:-1;
        if(attackDir == 1)
            rot =  Quaternion.Euler(0f, 180f, randomZ);
        else
            rot =  Quaternion.Euler(0f, 0f, randomZ);

        
        var vfxSpwanPos = enemy.GetComponentInChildren<EnemySprite>().transform.position;
        //通知特效器生成所有武器特效
        foreach (var vfx in hitVFX)
        {
            VFXManager.Instance.SpawnVFX(vfx,vfxSpwanPos,rot.eulerAngles,1.2f);
        }
        //通知特效生成器生成敌人被打的特效
        VFXManager.Instance.SpawnVFX(enemy.GetComponent<EnemyController>().hitVFX,vfxSpwanPos,rot.eulerAngles,1.2f);
        
        CharacterStats _attackerStats = GetComponentInParent<CharacterStats>();
            
            
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
