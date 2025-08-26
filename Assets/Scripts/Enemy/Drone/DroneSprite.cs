using UnityEngine;

public class DroneSprite : EnemySprite
{
    private DroneController droneController;

    protected void Awake()
    {
        droneController = enemy as DroneController;
    }
    
    
    //动画事件
    public void EnableAttackBox()
    {
        droneController.droneWeapon.SetActive(true);
    }

    public void DisableAttackBox()
    {
        droneController.droneWeapon.SetActive(false);
    }
}
