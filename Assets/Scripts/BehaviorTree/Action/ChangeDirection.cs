using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ChangeDirection : Action
{
    public SharedGameObject backToTarget;
    public SharedGameObject forwardToTarget;
    public SharedBool isRandom;
    public SharedInt direction;

    public override TaskStatus OnUpdate()
    {
        if (backToTarget.Value != null&&forwardToTarget.Value!=null&&isRandom.Value)
        {
            Debug.Log("ChangeDirection同时只能选一个模式");
            return TaskStatus.Failure;
        }
        else if (backToTarget.Value!=null)
        {
            direction.Value = this.transform.position.x - backToTarget.Value.transform.position.x > 0 ? 1 : -1;
            this.transform.localScale = new Vector3(direction.Value, 1, 1);
            return TaskStatus.Success;
        }
        else if(forwardToTarget.Value!=null)
        {
            direction.Value = this.transform.position.x - forwardToTarget.Value.transform.position.x > 0 ? -1 : 1;
            this.transform.localScale = new Vector3(direction.Value, 1, 1);
            return TaskStatus.Success;
        }
        else if (isRandom.Value)
        {
            direction.Value = Random.Range(0, 2) > 0.5 ? 1 : -1;
            this.transform.localScale = new Vector3(direction.Value, 1, 1);
            return TaskStatus.Success;
        }
        else
        {
            Debug.Log("ChangeDirection需要选择一个模式");
            return TaskStatus.Failure;
        }
    }
}
