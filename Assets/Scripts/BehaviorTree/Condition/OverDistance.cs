using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class OverDistance : Conditional
{
    public SharedGameObject gameObject_1;
    public SharedGameObject gameObject_2;
    public SharedFloat distance;
    public SharedFloat yRadius;

    public override TaskStatus OnUpdate()
    {
        if (gameObject_1==null||gameObject_2==null)
        {
            Debug.Log("没有设置物体");
            return TaskStatus.Success;
        }
        else if (Mathf.Abs(gameObject_1.Value.transform.position.x - gameObject_2.Value.transform.position.x) <= distance.Value &&
            Mathf.Abs(gameObject_1.Value.transform.position.y - gameObject_2.Value.transform.position.y) <= yRadius.Value)
        {
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
