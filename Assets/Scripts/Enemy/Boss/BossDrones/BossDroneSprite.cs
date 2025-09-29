using UnityEngine;

public class BossDroneSprite : EnemySprite
{

    private BossDroneController bossDroneController;

    protected void Awake()
    {
        bossDroneController = enemy as BossDroneController;
    }
    
    
    public void EnableWeapon()
    {
        bossDroneController.EnableWeapon();
    }

    public void DisableWeapon()
    {
        bossDroneController.DisableWeapon();
    }
}
