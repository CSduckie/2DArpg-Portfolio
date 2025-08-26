using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private SkillConfig skillConfig;
    
    [SerializeField]
    private Rigidbody2D rb;
    
    private float weaponDamage;
    private float stunValue;
    private Vector2 repelDir;

    private int attackDir;
    
    
    private void Start()
    {
        weaponDamage = skillConfig.releaseData.attackData.hitData.value;
        repelDir = skillConfig.releaseData.attackData.hitData.RepelVelocity;
        stunValue = skillConfig.releaseData.attackData.hitData.stunValue;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other && other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().isDead)
                return;
            
            
            //TODO:后续需要添加damage的修改值
            //other.GetComponent<CharacterStats>().TakeDamage(weaponDamage,stunValue,attackDir,repelDir);
            
            //完成目标后自行销毁
            Destroy(gameObject);
        }
    }

    public void InitBullet(int _shootDir)
    {
        attackDir = _shootDir;
        rb.linearVelocity = new Vector2(attackDir * bulletSpeed, 0);
    }

    //当被玩家完美弹反的时候产生的行为
    public void PerfectDefendByPlayer()
    {
        
    }
}
