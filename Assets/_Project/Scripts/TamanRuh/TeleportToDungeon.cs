using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToDungeon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
            if(coll.gameObject.tag == "Player")
        {
            // && Input.GetKeyDown(KeyCode.N)
            Debug.Log("Mantap cuuii");
            // SceneManager.LoadScene(0); 
        }
    }
}

