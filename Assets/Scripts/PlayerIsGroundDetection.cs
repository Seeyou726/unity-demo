using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGroundDetection : MonoBehaviour
{
    private GameObject Player;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        Player = this.transform.parent.gameObject;
        boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.offset = new Vector2(Player.GetComponent<BoxCollider2D>().offset.x, boxCollider.offset.y);
        boxCollider.size = new Vector2(Player.GetComponent<BoxCollider2D>().size.x, boxCollider.size.y);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //        Player.SendMessage("IsGround");
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //        Player.SendMessage("IsNotGround");
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            Player.SendMessage("IsGround");
        //Debug.Log("Trigger Enter    " +collision.name + "    " + Player.GetComponent<PlayerController>().currentState);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Player.SendMessage("IsNotGround");
        }
            
        //Debug.Log("Trigger Exit    " + collision.name + "    " + Player.GetComponent<PlayerController>().currentState);
    }

}
