using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
   
   [SerializeField] bool goNextLevel;
   [SerializeField] string levelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (goNextLevel)
            {
                SceneControler.instance.NextLevel();
            }
            else
            {
                SceneControler.instance.LoadScene(levelName);
            }
            

        }
    }
}
