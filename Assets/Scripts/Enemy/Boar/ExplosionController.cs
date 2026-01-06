using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private SkillConfig skillConfig;
    
    private float weaponDamage;
    public float stunValue { get; private set; }
    private Vector2 repelDir;
    public float impulseValue { get;private set; }
    private void Start()
    {
        weaponDamage = skillConfig.releaseData.attackData.hitData.value;
        repelDir = skillConfig.releaseData.attackData.hitData.RepelVelocity;
        stunValue = skillConfig.releaseData.attackData.hitData.stunValue;
        impulseValue = skillConfig.releaseData.attackData.ScreenImpulseValue;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.isDead)
                return;
            CharacterStats attackerStats = GetComponentInParent<CharacterStats>();
            int attackDir = 0;
            
            other.GetComponent<CharacterStats>().TakeDamage(weaponDamage,stunValue,attackDir,repelDir,attackerStats);
        }
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
