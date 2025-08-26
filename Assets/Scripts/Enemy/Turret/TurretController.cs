using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class TurretController : EnemyController
{
    [Header("玩家检测与激光")]
    public float checkDistance;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsWall;
    
    [SerializeField]
    public TurretLaserController laserController;

    [Header("攻击锁定相关参数以及射速")] 
    public float lostTargetTime = 3f;
    public float attackDuration;
    
    public bool canAttack { get; private set; } = false;

    [Header("子弹和枪口")] 
    public Transform muzzleTrans;
    public GameObject bulletPrefab;
    
    protected override void Start()
    {
        base.Start();
        laserController = GetComponentInChildren<TurretLaserController>();
        
        ChangeState(TurretState.Idle);
    }
    
    //状态机
    public void ChangeState(TurretState turretState)
    {
        switch(turretState)
        {
            case TurretState.Idle:
                stateMachine.ChangeState<TurretIdleState>();
                break;
            case TurretState.Active:
                stateMachine.ChangeState<TurretActiveState>();
                break;
            case TurretState.Battle:
                stateMachine.ChangeState<TurretBattleState>();
                break;
            case TurretState.Shoot:
                stateMachine.ChangeState<TurretShootState>();
                break;
            case TurretState.Close:
                stateMachine.ChangeState<TurretCloseState>();
                break;
            case TurretState.Die:
                stateMachine.ChangeState<TurretDieState>();
                break;
        }
    }
    
    //Check for player
    public bool isPlayerFound()
    {
        //炮台比较特殊，因此直接使用transform里的localScale就行
        Vector2 faceDir = new Vector2(transform.localScale.x,0);
        
        RaycastHit2D playerHit = Physics2D.Raycast(laserController.transform.position, faceDir, checkDistance, whatIsPlayer);
        if (playerHit.collider != null)
        {
            //检查是否被墙挡住
            Vector2 playerPos = playerHit.collider.transform.position;
            RaycastHit2D wallHit = Physics2D.Linecast(laserController.transform.position, playerPos, whatIsWall);

            // 如果没有撞到墙  视线通畅
            if (wallHit.collider == null)
            {
                return true;
            }
        }
        return false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(laserController.transform.position ,new Vector2(laserController.transform.position.x + checkDistance * transform.localScale.x, laserController.transform.position.y) );
    }

    
    #region 攻击冷却携程
    //开始启用攻击冷却
    public void StartAttackCool()
    {
        StartCoroutine(AttackCooling());
    }
    private IEnumerator AttackCooling()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackDuration); 
        canAttack = true;
    }
    #endregion
    
    //生成子弹
    public void ShootBullet()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = muzzleTrans.position;
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        var dir = transform.localScale.x > 0 ? 1 : -1;
        bulletScript.InitBullet(dir);
    }


    #region 炮台的受伤和死亡
    public void GetHurt()
    {
        //此处滞空是因为机枪我不希望有受伤动画
    }
    
    public void Die()
    {
        ChangeState(TurretState.Die);
        isDead = true;
        enemyUI.SetActive(false);
    }
    #endregion
    
}
