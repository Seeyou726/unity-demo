using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Walk : Action  
{
    public SharedFloat moveSpeed;
    public SharedFloat moveTime;
    public SharedInt direction;
    private float timer;

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer<=moveTime.Value)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed.Value*direction.Value, 0);
            return TaskStatus.Running;
        }
        else
        {
            timer = 0;
            return TaskStatus.Success;
        }
    }

}
