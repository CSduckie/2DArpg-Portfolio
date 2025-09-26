using UnityEngine;

public class HammerGuySprite : EnemySprite
{
    private HammerGuyController hammerGuyController;
    protected void Awake()
    {
        hammerGuyController = enemy as HammerGuyController;
    }
    
    //动画事件
    public void EnableAttackBox()
    {
        hammerGuyController.hammerGuyWeapon.SetActive(true);
    }

    public void DisableAttackBox()
    {
        hammerGuyController.hammerGuyWeapon.SetActive(false);
    }
}
