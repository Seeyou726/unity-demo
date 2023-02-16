using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Chase : Action
{
    public SharedGameObject target;
    public SharedFloat moveSpeed;
    public SharedFloat arriveDistance;

    public override TaskStatus OnUpdate()
    {
        if (Mathf.Abs(target.Value.transform.position.x - this.transform.position.x)>=arriveDistance.Value)
        {
            if (target.Value.transform.position.x - this.transform.position.x >= 0)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed.Value, 0);
                return TaskStatus.Running;
            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed.Value * -1, 0);
                return TaskStatus.Running;
            }
        }
        else
        {
            return TaskStatus.Success;
        }

    }
}
