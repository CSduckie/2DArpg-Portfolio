using System.Collections;
using UnityEngine;

public class BossController : EnemyController
{
    public BossStats bossStats { get; private set; }
    
    public bool willStun;
    [Header("Boss无人机技能")] 
    public GameObject bossDronePrefab;
    public int bossDroneSpawnNum;
    public float bossDroneSpwanDuration;
    public Transform[] bossDroneHookPos;
    public Vector2[] droneAttackingPosOffset;
    //无人机数量，用于追踪是否所有无人机回到boss处
    [HideInInspector] public int droneNum;
    public bool canCallDrone;
    [Range(0,1)]public float callDroneChance;
    public float dronSkillCD;
    
    
    [Header("Boss飞行参数")] 
    public Transform heightPoint;
    public float bossSpeed;
    
    [Header("Boss机械爪攻击技能")]
    public GameObject bossClaws;
    public Transform[] bossClawSpawnPos;
    public BossArmController[]  bossArms;
    public bool spwanCompleted;
    public Transform bossArmAttackPos;
    public bool canUseClaw;
    [Range(0,1)]public float useClawChance;
    public float clawSkillCD;


    [Header("Boss强杀技能参数")] 
    public float enemySpwanDuration;
    public int currentForceKillChargeStage;
    public GameObject[] sparkVFX;
    public bool forceKillTriggered;
    public Transform bossForceKillChargingTrans;
    public Transform EnemySpwanTransL;
    public Transform EnemySpwanTransR;
    public GameObject assassinPrefab;
    
    
    [Header("Boss炸弹技能参数")] 
    public GameObject bombPrefab;
    [SerializeField] private Transform bombSpwanTrans;
    public float bombSpwanDuration;
    public bool canBomb;
    [Range(0,1)]public float useBombChance;
    public float bombSkillCD;
    
    [Header("Boss砸击技能")] 
    [Range(0,1)] public float smashChance;
    public float smashCD;
    public bool canSmash;
    public GameObject smashHitBox;
    
	[Header("激光技能")]
    [Range(0,1)] public float laserSkillChance;
	public LaserControl laser;
    public float rotateSpeed;
    [SerializeField] private GameObject effectPrefab; // 特效预制体进来
    public float laserSkillCD;
    public bool canLaser;
    
    protected override void Start()
    {
        base.Start();
        transform.position = heightPoint.transform.position;
        bossStats = GetComponent<BossStats>();
        //TODO:添加过场后，删除这个代码
        ChangeState(BossStates.Idle);
    }
    
    //状态机
    public void ChangeState(BossStates bossStates)
    {
        switch(bossStates)
        {
            case BossStates.Idle:
                stateMachine.ChangeState<BossIdleState>();
                break;
            case BossStates.DroneAttack:
                stateMachine.ChangeState<BossDroneAttackState>();
                break;
            case BossStates.SmashAttack:
                stateMachine.ChangeState<BossSmashAttackState>();
                break;
            case BossStates.ArmSmashAttack:
                stateMachine.ChangeState<BossArmAttackState>();
                break;
            case BossStates.ForceKillSkill:
                stateMachine.ChangeState<BossForceKillSkillState>();
                break;
            case BossStates.BombAttackSkill:
                stateMachine.ChangeState<BossBombAttackState>();
                break;
            case BossStates.Stun:
                stateMachine.ChangeState<BossStunState>();
                break;
            case BossStates.Die:
                stateMachine.ChangeState<BossDieState>();
                break;
            case BossStates.LaserAOESkill:
                stateMachine.ChangeState<BossLaserAOESkillState>();
                break;
        }
    }

    #region 无人机生成技能
    public void SpwanDrone()
    {
        droneNum = 4;
        StartCoroutine(spwanDronesCoroutine(bossDroneSpawnNum,bossDroneSpwanDuration));
    }

    private IEnumerator spwanDronesCoroutine(int _numToSpwan, float spwanDuration)
    {
        Vector2 playerCurrentPos = GameManager.instance.player.transform.position;
        for (int i = 0; i < _numToSpwan; i++)
        {
            var newDrone = Instantiate(bossDronePrefab, transform.position, Quaternion.identity);
            BossDroneController droneScript = newDrone.GetComponent<BossDroneController>();
            droneScript.Init(droneAttackingPosOffset[i] + new Vector2(playerCurrentPos.x,playerCurrentPos.y),playerCurrentPos,1,this,bossDroneHookPos[i].position);
            yield return new WaitForSeconds(spwanDuration);
        }
    }
    #endregion
    
    #region 机械爪技能
    public void SpwanClaws()
    {
        spwanCompleted = false;
        //检测当前是否创建过爪子了
        if (bossArms[0] == null && bossArms[1] == null)
        {
            //创建一左，一右两个爪子
            //将左爪子初始时Flip一次
            var leftClaw = Instantiate(bossClaws,bossClawSpawnPos[0].position, Quaternion.identity);
            BossArmController leftClawScript = leftClaw.GetComponent<BossArmController>();
            leftClawScript.Flip();
            leftClawScript.Init(new Vector2(-6,-1.6f),this);
            bossArms[0] =  leftClawScript;
            var RightClaw =  Instantiate(bossClaws,bossClawSpawnPos[1].position, Quaternion.identity);
            BossArmController rightClawScript = RightClaw.GetComponent<BossArmController>();
            rightClawScript.Init(new Vector2(6,-1.6f),this);
            bossArms[1] =  rightClawScript;
        }
        else
        {
            //如果已经存在一组了，那就设置他们的正确翻转，然后SetActive
            if (bossArms[0].isFacingRight)
            {
                bossArms[0].gameObject.SetActive(true);
                bossArms[0].Flip();
                bossArms[0].MoveToHookPos();
                bossArms[0].ChangeState(BossArmState.Idle);
            }

            if (!bossArms[1].isFacingRight)
            {
                bossArms[1].gameObject.SetActive(true);
                bossArms[1].Flip();
                bossArms[1].MoveToHookPos();
                bossArms[1].ChangeState(BossArmState.Idle);
            }
        }
        spwanCompleted = true;
    }
    #endregion
    
    #region 强杀技能和逻辑

    public void SpwanEnemies()
    {
        var leftEnemy = Instantiate(assassinPrefab,EnemySpwanTransL.position,Quaternion.identity);
        var rightEnemy = Instantiate(assassinPrefab,EnemySpwanTransR.position,Quaternion.identity);
        rightEnemy.GetComponent<EnemyController>().Flip();
        leftEnemy.SetActive(true);
        rightEnemy.SetActive(true);
    }
    
    
    public void enableRandomSparks()
    {
        int index = Random.Range(0, sparkVFX.Length);
        if (index == 0 || index == 2)
        {
            float yAxis = Random.Range(1.5f, 9f);
            VFXManager.Instance.SpawnVFX(sparkVFX[index],new Vector2(sparkVFX[index].transform.position.x,yAxis),sparkVFX[index].transform.rotation.eulerAngles,new Vector3(1,1,1),3f);
        }
        else
        {
            float xAxis = Random.Range(-6f, 11.5f);
            VFXManager.Instance.SpawnVFX(sparkVFX[index],new Vector2(xAxis,sparkVFX[index].transform.position.y),sparkVFX[index].transform.rotation.eulerAngles,new Vector3(1,1,1),3f);
        }
    }
    
    #endregion
    
    #region 炸弹技能

    public void SpwanBombs()
    {
        Instantiate(bombPrefab,bombSpwanTrans.position,Quaternion.identity);
    }
    #endregion

    #region 激光技能

    public void StartSpwanEffectCoroutine()
    {
        StartCoroutine(SpawnEffects());
    }
    
    private IEnumerator SpawnEffects()
    {
        float x = -8.5f;
        float y = -0.5f;

        while (x <= 15.5f + 0.001f) // 加一点点容错
        {
            Instantiate(effectPrefab, new Vector3(x, y, 0f), Quaternion.identity);
            x += 2.5f;

            if (x <= 15.5f)
                yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion
    

    #region 技能冷却

    public void CoolSmashSkill()
    {
        StartCoroutine(coolSmashCoroutine());
    }

    private IEnumerator coolSmashCoroutine()
    {
        yield return new WaitForSeconds(smashCD);
        canSmash = true;
    }
    public void CoolDroneAttackSkill()
    {
        StartCoroutine(CoolDroneAttackCorutine());
    }

    private IEnumerator CoolDroneAttackCorutine()
    {
        yield return new WaitForSeconds(dronSkillCD);
        canCallDrone = true;
    }
    
    public void CoolClawSkill()
    {
        StartCoroutine(CoolClawSkillCorutine());
    }

    private IEnumerator CoolClawSkillCorutine()
    {
        yield return new WaitForSeconds(clawSkillCD);
        canUseClaw = true;
    }
    
    
    public void CoolBombSkill()
    {
        StartCoroutine(coolBombCoroutine());
    }

    private IEnumerator coolBombCoroutine()
    {
        yield return new WaitForSeconds(bombSkillCD);
        canBomb = true;
    }

    public void CoolLaserSkill()
    {
        StartCoroutine(coolLaserCoroutine());
    }

    private IEnumerator coolLaserCoroutine()
    {
        yield return new WaitForSeconds(laserSkillCD);
        canLaser = true;
    }
    #endregion

    #region 状态更改

    public void Die()
    {
        ChangeState(BossStates.Die);
        return;
    }

    public void GetStun()
    {
        willStun = true;
    }

    #endregion
}
