using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 这个脚本用于支持伤害的动态改变，每次调用伤害时，从中获取Getvalue
/// </summary>
[System.Serializable]
public class Stats
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }
    
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }
    
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
