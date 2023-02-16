using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    public Vector2 uvAnimationRate = Vector2.zero;
    protected Vector2 uvOffset = Vector2.zero;
    private SpriteRenderer sp;

    private void OnEnable()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    public void MoveBackGround(float dir)
    {
        uvOffset += uvAnimationRate * Time.deltaTime;

        if (dir > 0)
        {
            uvAnimationRate = new Vector2(Mathf.Abs(uvAnimationRate.x), Mathf.Abs(uvAnimationRate.y));
        }
        if (dir < 0)
        {
            uvAnimationRate = new Vector2(-Mathf.Abs(uvAnimationRate.x), Mathf.Abs(uvAnimationRate.y));
        }

        if (sp.material != null)
        {
            sp.material.mainTextureOffset = uvOffset;
        }
    }

    //public float scrollSpeed = 0.5f;
    //private float offset;
    //private Material mat;

    //private void Start()
    //{
    //    mat = GetComponent<Renderer>().material;
    //}

    //private void Update()
    //{
    //    offset += (Time.deltaTime * scrollSpeed) / 10f;
    //    mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    //}
}
