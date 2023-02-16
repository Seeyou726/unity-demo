using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AttackedCondition:Conditional
{
    private AttackedManager attackedManager;

    public override void OnStart()
    {
        attackedManager = GetComponent<AttackedManager>();
    }
    public override TaskStatus OnUpdate()
    {
        Debug.Log(attackedManager.beAttacked);
        if (attackedManager.beAttacked)
        {
            if (!attackedManager.currentAnimEnd)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
        else
        {
            return TaskStatus.Failure;
        } 

    }
}
