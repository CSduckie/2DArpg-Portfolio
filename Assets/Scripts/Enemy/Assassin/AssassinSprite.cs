using UnityEngine;

public class AssassinSprite : EnemySprite
{
    private AssassinController assassinController;

    protected void Awake()
    {
        assassinController = enemy as AssassinController;
    }
    
    
    //动画事件
    public void EnableAttackBox()
    {
        assassinController.assassinWeapon.SetActive(true);
    }

    public void DisableAttackBox()
    {
        assassinController.assassinWeapon.SetActive(false);
    }
}
