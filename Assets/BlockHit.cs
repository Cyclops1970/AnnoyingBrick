using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour {

    public AudioClip playerDieSound;
    public GameObject playerDieParticle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.invincible != true)
        {
            AudioSource.PlayClipAtPoint(playerDieSound, Camera.main.transform.localPosition);
            Instantiate(playerDieParticle, collision.transform.localPosition, Quaternion.identity);
            StartCoroutine(GameOverPanel());
            Handheld.Vibrate();
            GameManager.gameStarted = false; // game ended

            Destroy(collision.gameObject); // destroy player

        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.invincible != true)
        {
            StartCoroutine(GameOverPanel());
            AudioSource.PlayClipAtPoint(playerDieSound, Camera.main.transform.localPosition);
            Instantiate(playerDieParticle, collision.transform.localPosition, Quaternion.identity);
            Handheld.Vibrate();
            GameManager.gameStarted = false; // game ended

            Destroy(collision.gameObject); // destroy player

        }
    }

    IEnumerator GameOverPanel()
    {
        yield return new WaitForSeconds(1);

        //I should be able to reference this somehow without this find stuff
        GameObject g = GameObject.Find("GameManager");
        GameObject gop = g.GetComponent<GameManager>().gameOverPanel;

        gop.SetActive(true);    //show the game over panel

        yield return null;
    }
}
