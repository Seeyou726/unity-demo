using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackDamageData
{
    public List<string> attackName = new List<string> { "asuna_attack_1", "asuna_attack_2", "asuna_attack_3" };
    public List<float> attackValue = new List<float> { 10, 20, 30 };
    public Dictionary<string, float> attackDamage = new Dictionary<string, float>();
    public AttackDamageData()
    {
        for (int i = 0; i < attackName.Count; i++)
        {
            attackDamage.Add(attackName[i],attackValue[i]);
        }
        
    }
}
