using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BordersPlacer : MonoBehaviour
{
    public BoxCollider2D leftWall;
    public BoxCollider2D rightWall;
    public BoxCollider2D topWall;
    public BoxCollider2D bottomWall;
    Vector2 screenBounds;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        PlaceColliders();
    }

    void PlaceColliders()
    {
        float boxHeight = -0.5f;
        float boxWidth = -0.5f;

        leftWall.transform.position = new Vector3(-screenBounds.x + boxWidth, 0f, 0f); ;
        rightWall.transform.position = new Vector3(screenBounds.x - boxWidth, 0f, 0f); ;
        topWall.transform.position = new Vector3(0f, screenBounds.y - boxHeight, 0f); ;
        bottomWall.transform.position = new Vector3(0f, -screenBounds.y + boxHeight, 0f); ;
    }
}
