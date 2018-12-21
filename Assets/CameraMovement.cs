using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    float moveLevel = 0;
    enum Direction {up, down, left, right};
    Direction playerDirection = Direction.up;
    float highestCameraPoint;

    private void FixedUpdate()
    {
        if (GameManager.gameStarted==false) 
        {
            highestCameraPoint = moveLevel;
        }
        else
        {
            //bad code
            player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                if (playerDirection == Direction.up)
                {
                    if ((player.transform.localPosition.y > moveLevel) && (player.transform.localPosition.y > highestCameraPoint))
                    {
                        Camera.main.transform.localPosition = new Vector2(Camera.main.transform.localPosition.x, player.transform.localPosition.y);
                        highestCameraPoint = Camera.main.transform.localPosition.y;
                    }
                }
            }
        }
    }
    
}
