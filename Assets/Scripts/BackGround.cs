using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteHorizontal;
    [SerializeField] private bool infiniteVertical;

    private Transform cameraTransform;
    private Vector3 lastCameraPos;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPos = cameraTransform.position;

        //if (infiniteHorizontal)
        //{
        //    if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        //    {
        //        float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
        //        transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, cameraTransform.position.y);
        //    }
        //}

        //if (infiniteVertical)
        //{
        //    if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        //    {
        //        float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
        //        transform.position = new Vector3(cameraTransform.position.x , cameraTransform.position.y + offsetPositionY);
        //    }
        //}
    }
}
