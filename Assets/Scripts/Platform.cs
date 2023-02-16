using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private GameObject leftBorder;
    private GameObject rightBorder;
    private GameObject platform;

    // Start is called before the first frame update
    private void Awake()
    {
        leftBorder = transform.Find("LeftBorder").gameObject;
        rightBorder = transform.Find("RightBorder").gameObject;
        platform = transform.Find("PlatformCollider").gameObject;

        if (this.GetComponent<SpriteRenderer>())
        {
            platform.GetComponent<BoxCollider2D>().size = this.GetComponent<SpriteRenderer>().size;
            platform.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);

            leftBorder.GetComponent<BoxCollider2D>().size = new Vector2(0.2f, GetComponent<SpriteRenderer>().size.y);
            leftBorder.transform.position = new Vector3(transform.position.x - GetComponent<SpriteRenderer>().size.x / 2-0.1f, transform.position.y, 0);

            rightBorder.GetComponent<BoxCollider2D>().size = new Vector2(0.2f, GetComponent<SpriteRenderer>().size.y);
            rightBorder.transform.position = new Vector3(transform.position.x + GetComponent<SpriteRenderer>().size.x / 2+0.1f, transform.position.y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
