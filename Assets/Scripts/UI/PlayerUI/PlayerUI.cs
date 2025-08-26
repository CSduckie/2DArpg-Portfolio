using System;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [Header("生命值相关")]
    public Image healthImage;
    public Image healthDelayImage;
    public PlayerStats playerStats;

    [Header("失衡值相关")] 
    public Image coolingImage1;
    public Image coolingImage2;
    public Image coolingDelayImage;

    [Header("能量值相关")] 
    public EnergySlotManager energySlotManager;
    
    private void Start()
    {
        playerStats.onHealthChanged += UpdateHealthUI;
        playerStats.onCoolingChanged += UpdateCoolingUI;
        playerStats.onEnergyChanged += UpdateEnergyUI;
    }
    
    private void UpdateHealthUI()
    {
        healthImage.fillAmount = playerStats.GetCurrentHealth()/playerStats.GetMaxHP();
    }
    private void UpdateCoolingUI()
    {
        coolingImage1.fillAmount = playerStats.GetCurrentCooling() / playerStats.GetMaxExoCooling();
        coolingImage2.fillAmount = playerStats.GetCurrentCooling() / playerStats.GetMaxExoCooling();
    }
    
    private void UpdateEnergyUI()
    {
        foreach (var energySlot in energySlotManager.allSlots)
        {
            energySlot.engergyFillIMG.enabled = false;
        }
        
        for (int i = 0; i < playerStats.GetCurrentEnergy() -1; i++)
        {
            energySlotManager.allSlots[i].engergyFillIMG.enabled = true;
        }
    }
    
    private void Update()
    {
        //实现血条的线性渐变效果
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        playerStats.onHealthChanged -= UpdateHealthUI;
    }
}
