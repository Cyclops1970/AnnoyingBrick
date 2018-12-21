using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePowerup : MonoBehaviour {

    GameObject player;
    float powerupRunTime = 9;
    float warningTime = 2;
    public AudioClip reverseSound;
    public AudioClip reverseEndSound;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        ParticleSystem ps = this.GetComponentInChildren<ParticleSystem>();
        var main = ps.main;
        main.startColor = Color.blue;
        this.GetComponent<SpriteRenderer>().color = Color.blue;

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
        if ((collision.tag == "Player") && ((GameManager.invincible != true) && (GameManager.reverse != true)))
        {
            AudioSource.PlayClipAtPoint(reverseSound, Camera.main.transform.localPosition);

            GameManager.reverse = true;

            StartCoroutine(PlayerReverse());

            //disable, hide and destroy
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponentInChildren<ParticleSystem>().Stop();
            this.GetComponent<Collider2D>().enabled = false;
            Destroy(this, powerupRunTime * 2);

        }
    }

    IEnumerator PlayerReverse()
    {
        if (GameManager.rb != null)
        {
            player.GetComponentInChildren<ParticleSystem>().Play();
            ParticleSystem ps = player.GetComponentInChildren<ParticleSystem>();
            var main = ps.main;
            main.startColor = Color.blue;

            player.GetComponent<SpriteRenderer>().color = Color.blue;
            //wait for time period minue the warning time
            yield return new WaitForSeconds(powerupRunTime - warningTime);

            //warning time
            float startTime = Time.time;
            int counter = 0;
            AudioSource.PlayClipAtPoint(reverseEndSound, Camera.main.transform.localPosition);
            while ((Time.time - startTime < warningTime) && (GameManager.rb != null))
            {
                if ((counter % 2 == 0))
                {
                    player.GetComponent<SpriteRenderer>().color = Color.red;
                    main.startColor = Color.red;
                }
                else
                {
                    player.GetComponent<SpriteRenderer>().color = Color.white;
                    main.startColor = Color.white;
                }
                counter++;
                yield return null;
            }

            if (GameManager.rb != null)
            {
                player.GetComponent<SpriteRenderer>().color = Color.white;
                main.startColor = Color.white;
                player.GetComponentInChildren<ParticleSystem>().Stop();
            }

            GameManager.reverse = false;
            yield return null;
        }
        yield return null;
    }
}
