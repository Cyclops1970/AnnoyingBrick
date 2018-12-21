using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour {

    public AudioClip playerDie;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Block")||(collision.gameObject.tag=="LevelBlock")||(collision.gameObject.tag=="Powerup"))
        {
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag=="Player") //play has dropped off the screen
        {
            AudioSource.PlayClipAtPoint(playerDie, Camera.main.transform.localPosition);
            //I should be able to reference this somehow without this find stuff
            GameObject g = GameObject.Find("GameManager");
            GameObject gop = g.GetComponent<GameManager>().gameOverPanel;
            Handheld.Vibrate();
            gop.SetActive(true);    //shoe the game over panel
            GameManager.gameStarted = false;
            Destroy(collision.gameObject);
        }
    }
    
}
