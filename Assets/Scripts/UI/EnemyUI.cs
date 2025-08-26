using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour
{
    [Header("生命值相关")]
    public Image healthImage;
    public Image healthDelayImage;
    public CharacterStats enemyCharacterStats;

    [Header("失衡值相关")] 
    public Image coolingImage;

    protected virtual void Start()
    {
        enemyCharacterStats.onHealthChanged += UpdateHealthUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected virtual void UpdateHealthUI()
    {
        healthImage.fillAmount = enemyCharacterStats.GetCurrentHealth()/enemyCharacterStats.GetMaxHP();
    }
    
}
