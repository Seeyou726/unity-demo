using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadHitBottom : MonoBehaviour
{
    private GameObject Player;
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        Player = this.transform.parent.gameObject;
        boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.offset = new Vector2(Player.GetComponent<BoxCollider2D>().offset.x,boxCollider.offset.y);
        boxCollider.size = new Vector2(Player.GetComponent<BoxCollider2D>().size.x, boxCollider.size.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            Player.SendMessage("HeadHitBottom");
    }
}
