using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private float radiusX;  //相机X、Y轴半径
    private float radiusY;
    private float borderLeft;   //地图上下左右边界
    private float borderRight;
    private float borderTop;
    private float borderBottom;

    public Transform player;

    private void Start()
    {
        radiusX = GetComponent<BoxCollider2D>().bounds.extents.x;
        radiusY = GetComponent<BoxCollider2D>().bounds.extents.y;
        borderLeft = GameObject.FindGameObjectWithTag("LeftBorder").GetComponent<Transform>().position.x;
        borderRight = GameObject.FindGameObjectWithTag("RightBorder").GetComponent<Transform>().position.x;
        borderBottom = GameObject.FindGameObjectWithTag("Bottom").GetComponent<Transform>().position.y;

        player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        Vector3 pos = transform.position;

        if (Mathf.Abs(player.position.x - borderLeft) > radiusX)
        {
            if (Mathf.Abs(player.position.x - borderRight) > radiusX)
            {
                pos.x = player.position.x;
            }
            else
            {
                pos.x = borderRight - radiusX;
            }
        }
        else
        {
            pos.x = borderLeft + radiusX;
        }

        if (Mathf.Abs(player.position.y - borderBottom) > radiusY)
        {
            pos.y = player.position.y;
        }
        else
        {
            pos.y = borderBottom + radiusY;
        }

        transform.position = pos;
    }
}
