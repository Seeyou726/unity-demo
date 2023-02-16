using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeAttacked : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("MainCamera"))
        {
            Debug.Log(1111 + "    " + collision.gameObject.name);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
