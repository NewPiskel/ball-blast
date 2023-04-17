using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScreen : MonoBehaviour
{
    [SerializeField] BoxCollider2D leftWallCollider;
    [SerializeField] BoxCollider2D rightWallCollider;
    // Start is called before the first frame update
    void Awake ()
    {
        float screenWidth = Game.Instance.screenWidth;

        leftWallCollider.transform.position = new Vector3(-screenWidth - leftWallCollider.size.x / 2f, 0f, 0f);
        rightWallCollider.transform.position = new Vector3(screenWidth + rightWallCollider.size.x / 2f, 0f, 0f);

        //Destroy(this);
    }

   
}
