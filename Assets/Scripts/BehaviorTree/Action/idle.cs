using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class idle : Action
{
    public SharedFloat idleTime;
    private float timer;

    public override void OnStart()
    {
        timer = 0;
    }
    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer<=idleTime.Value)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            return TaskStatus.Running;
        }
        else
        {
            timer = 0;
            return TaskStatus.Success;
        }
    }
}
