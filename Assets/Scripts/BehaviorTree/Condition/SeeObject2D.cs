using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SeeObject2D : Conditional
{
    public SharedGameObject target;
    //public SharedString name;
    //public SharedString tag;
    public SharedFloat distance;
    public SharedFloat yRadius;
    private int direction;
    private List<GameObject> seenObject = new List<GameObject>();
    
    public override TaskStatus OnUpdate()
    {
        direction = (int)this.transform.localScale.x;

        if (target.Value!=null &&distance.Value!=0&&yRadius.Value!=0)
        {

            if (Mathf.Abs(target.Value.transform.position.x - this.transform.position.x) < distance.Value&& Mathf.Abs(target.Value.transform.position.y - this.transform.position.y) <yRadius.Value&& (target.Value.transform.position.x - this.transform.position.x) * direction > 0)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }       
        }
        else
        {
            return TaskStatus.Failure;
        }
        //if (name.Value!=null)
        //{
        //    seenObject.Add(GameObject.Find(name.Value));
        //    foreach (GameObject item in seenObject)
        //    {
        //        if ((item.transform.position.x-this.transform.position.x)*direction<distance.Value)
        //        {
        //            return TaskStatus.Success;
        //        }
        //    }
        //}
        //if (tag.Value!=null)
        //{
        //    seenObject.AddRange(GameObject.FindGameObjectsWithTag(tag.Value));
        //    foreach (GameObject item in seenObject)
        //    {
        //        if ((item.transform.position.x - this.transform.position.x) * direction < distance.Value)
        //        {
        //            return TaskStatus.Success;
        //        }
        //    }
        //}

    }

}
