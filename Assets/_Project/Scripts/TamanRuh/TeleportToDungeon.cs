using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToDungeon : MonoBehaviour
{
    public Scene mainGameScene;
    public bool isInside;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.N) && isInside)
        {
            SceneManager.LoadScene(0); 
        }
    }
 

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            isInside = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            isInside = false; 
        }
    }

    // private void OnTriggerStay2D(Collider2D coll)
    // {
        
    //     if(coll.gameObject.tag == "Player")
    //     {
    //         SceneManager.LoadScene(0); 
    //     }
    
        
    // }
}

