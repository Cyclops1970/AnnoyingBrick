using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ReverseGravityPowerup : MonoBehaviour {

    GameObject player;
    float powerupRunTime = 9;
    float warningTime = 2;
    public AudioClip reverseGravitySound;
    public AudioClip reverseGravityEndSound;
    ParticleSystem powerupPS, playerPS;
    TextMeshProUGUI infoText;
    Color psColour;
    private void Start()
    {
        psColour = this.GetComponent<SpriteRenderer>().color;

        player = GameObject.FindGameObjectWithTag("Player");
        //setup info text reference.
        infoText = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Info;
        powerupPS = this.GetComponentInChildren<ParticleSystem>();
        playerPS = player.GetComponentInChildren<ParticleSystem>();

        var powerupRB = this.GetComponent<Rigidbody2D>();
        powerupRB.AddForce(new Vector2(Random.Range(-75, 75), Random.Range(-15, 15)));

        //Ensure not instantiated touching another collider
        StartCoroutine(PositionPowerup());

    }

    IEnumerator PositionPowerup()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale / 5, 0); // had it at /5
        while (hitColliders.Length > 1)
        {
            foreach (Collider2D c in hitColliders)
            {
                if (c.gameObject.tag == "Block")
                {
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 0.1f, GameManager.zPos);
                }
            }
            hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale / 5, 0); // had it at /5
            yield return null;
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player") && (GameManager.powerup != true))
        {
            AudioSource.PlayClipAtPoint(reverseGravitySound, Camera.main.transform.localPosition);

            GameManager.reverseGravity = true;
            GameManager.powerup = true;

            StartCoroutine(ReverseGravity());

            //disable, hide and destroy
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponentInChildren<ParticleSystem>().Stop();
            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            Destroy(this, powerupRunTime * 2);

        }
    }

    IEnumerator ReverseGravity()
    {
        if (GameManager.rb != null)
        {
            Vector2 cf = GameManager.rb.mass * GameManager.rb.velocity;
            GameManager.rb.AddForce(-cf, ForceMode2D.Impulse); //Stop the Player

            Vector2 oldGravity = Physics2D.gravity;
            Physics2D.gravity *= -.5f;

            //play the particle system attached to the player
            playerPS.Play();

            //set player particles to match the powerup
            var playerPSMain = playerPS.main;
            var powerupPSMain = powerupPS.main;
            playerPSMain.startColor = powerupPSMain.startColor;
            //set player colour to match the powerup
            player.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;


            //new code

            float startTime = Time.time;
            int counter = 0;

            while (Time.time < startTime + powerupRunTime) // loop for entire powerup time.
            {
                if ((powerupRunTime - warningTime > Time.time - startTime) && (GameManager.rb != null))
                {
                    //before warning time
                    infoText.color = Color.white;
                    yield return null;
                }
                else
                {
                    infoText.color = Color.red;

                    //warning time
                    if ((counter % 2 == 0)&&(player!=null))
                    {
                        player.GetComponent<SpriteRenderer>().color = Color.red;
                        playerPSMain.startColor = Color.red;
                        infoText.color = Color.red;
                    }
                    else if (player!=null)
                    {
                        player.GetComponent<SpriteRenderer>().color = Color.white;
                        playerPSMain.startColor = Color.white;
                        infoText.color = Color.white;
                    }
                    counter++;
                    yield return null;
                }
                infoText.text = "Gravity: " + (powerupRunTime - (Time.time - startTime)).ToString("F1");
            }

            AudioSource.PlayClipAtPoint(reverseGravityEndSound, Camera.main.transform.localPosition);
            if (GameManager.rb != null)
            {
                player.GetComponent<SpriteRenderer>().color = Color.white;
                playerPSMain.startColor = Color.white;
                player.GetComponentInChildren<ParticleSystem>().Stop();
            }

            if (GameManager.rb != null)
            {
                //set movement back to normal
                cf = GameManager.rb.mass * GameManager.rb.velocity;
                GameManager.rb.AddForce(-cf, ForceMode2D.Impulse); //Stop the Player
                Physics2D.gravity = oldGravity;
            }

            infoText.text = "";
            GameManager.reverseGravity = false;
            GameManager.powerup = false;
        }
        yield return null;
    }
    private void Update()
    {
        var powerupPSMain = powerupPS.main;

        if (GameManager.powerup)
        {
            powerupPSMain.startColor = Color.white;
            this.GetComponent<SpriteRenderer>().color = Color.white;
            this.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
        else
        {
            powerupPSMain.startColor = psColour;
            this.GetComponent<SpriteRenderer>().color = psColour;
            this.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }
}
