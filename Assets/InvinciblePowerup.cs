using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvinciblePowerup : MonoBehaviour {

    GameObject player;
    float powerupRunTime = 9;
    float warningTime = 2;
    public AudioClip invincibleSound;
    public AudioClip invincibleEndSound;

    [HideInInspector]
    ParticleSystem powerupPS, playerPS;
    Color psColour;

    TextMeshProUGUI infoText;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        powerupPS = this.GetComponentInChildren<ParticleSystem>();
        psColour = this.GetComponent<SpriteRenderer>().color; //store colour for use in update method.
        playerPS = player.GetComponentInChildren<ParticleSystem>();

        //setup info text reference.
        infoText = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Info;

        //Ensure not instantiated touching another collider
        StartCoroutine(PositionPowerup());
    }

    IEnumerator PositionPowerup()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, this.GetComponent<CircleCollider2D>().radius); 
        while (hitColliders.Length>2)
        {
            print(hitColliders.Length);
            foreach(Collider2D c in hitColliders)
            {
                if (c.gameObject.tag == "Block")
                {
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 0.1f, GameManager.zPos);
                }
            }
            hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, this.GetComponent<CircleCollider2D>().radius); 
            yield return null;
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player") && (GameManager.powerup != true))
        {
            AudioSource.PlayClipAtPoint(invincibleSound, Camera.main.transform.localPosition);

            GameManager.invincible = true;
            GameManager.powerup = true;

            StartCoroutine(PlayerInvincible());

            //disable, hide and destroy
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponentInChildren<ParticleSystem>().Stop();
            this.GetComponent<Collider2D>().enabled = false;
            Destroy(this, powerupRunTime*2);

        }
    }

    IEnumerator PlayerInvincible()
    {
        if (GameManager.rb != null)
        {
            //infoText.text = "Invincible " + powerupRunTime.ToString("F0");

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

            while(Time.time < startTime+powerupRunTime) // loop for entire powerup time.
            {
                if((powerupRunTime-warningTime > Time.time - startTime)&&(GameManager.rb!=null))
                {
                    //before warning time
                    infoText.color = Color.white;
                    yield return null;
                }
                else
                {
                    infoText.color = Color.red;

                    //warning time
                    if ((counter % 2 == 0))
                    {
                        player.GetComponent<SpriteRenderer>().color = Color.red;
                        playerPSMain.startColor = Color.red;
                        infoText.color = Color.red;
                    }
                    else
                    {
                        player.GetComponent<SpriteRenderer>().color = Color.white;
                        playerPSMain.startColor = Color.white;
                        infoText.color = Color.white;
                    }
                    counter++;
                    yield return null;
                }
                infoText.text = "Invincible: " + (powerupRunTime - (Time.time - startTime)).ToString("F1");
            }

            AudioSource.PlayClipAtPoint(invincibleEndSound, Camera.main.transform.localPosition);
            if (GameManager.rb != null)
            {
                player.GetComponent<SpriteRenderer>().color = Color.white;
                playerPSMain.startColor = Color.white;
                player.GetComponentInChildren<ParticleSystem>().Stop();
            }

            infoText.text = "";
            GameManager.invincible = false;
            GameManager.powerup = false;
            yield return null;
            
        }
        
        yield return null;
    }

    private void Update()
    {
        //control the colour 
        var powerupPSMain = powerupPS.main;
        if (GameManager.powerup)
        {
            powerupPSMain.startColor = Color.white;
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            powerupPSMain.startColor = psColour;
            this.GetComponent<SpriteRenderer>().color = psColour;
        }
    }
}
