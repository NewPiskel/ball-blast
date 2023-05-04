using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "boulder")
        {
            FindObjectOfType<SceneLoader>().LoadGameOver();
        }
    }
}
