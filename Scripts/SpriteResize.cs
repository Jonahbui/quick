/*  Author: Jonah Bui
	Purpose: Resize a sprite to fit screen.
	Date: January 14, 2020
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResize : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Camera worldCamera;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        //Get width and height of sprite
        float width = sprite.bounds.size.x;
        float height = sprite.bounds.size.y;

        //Get width and height of the camera
        float cameraHeight = worldCamera.orthographicSize * 2.0f;
        float cameraWidth = cameraHeight * ((float)Screen.width / (float)Screen.height);
        
        //Use proportions to scale the sprites
        transform.localScale = new Vector3(cameraWidth / width, cameraHeight / height, transform.localScale.z);
    }
}
