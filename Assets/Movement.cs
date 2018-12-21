using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour, IPointerDownHandler 
{

    Vector2 movePlayerRight = new Vector2(3.25f, 23);
    Vector2 movePlayerLeft = new Vector2(-3.25f, 23);

    // Use this for initialization
    void Start()
    {
        //used for the OnPointerDown detection
        Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            GameObject.FindGameObjectWithTag("MainCamera").gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if ((GameManager.gameStarted == true) && (GameManager.rb.simulated == false))
        {
            GameManager.rb.simulated = true;
            GameObject g = GameObject.Find("GameManager");
            GameObject ppi = g.GetComponent<GameManager>().playPanelInstructions;
            ppi.SetActive(false);
        }

        if (GameManager.rb != null)
        {
            Vector2 cf = GameManager.rb.mass * GameManager.rb.velocity;

            GameManager.rb.AddForce(-cf, ForceMode2D.Impulse); //Stop the Player

            if (eventData.pointerCurrentRaycast.gameObject.tag == "RightTouch")
            {
                if(GameManager.reverse==false)
                {
                    GameManager.rb.AddForce(movePlayerRight, ForceMode2D.Impulse);
                }
                else
                {
                    GameManager.rb.AddForce(movePlayerLeft, ForceMode2D.Impulse);
                }
                

                GameManager.rb.AddTorque(1);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.tag == "LeftTouch")
            {
                if(GameManager.reverse==false)
                {
                    GameManager.rb.AddForce(movePlayerLeft, ForceMode2D.Impulse);
                }
                else
                {
                    GameManager.rb.AddForce(movePlayerRight, ForceMode2D.Impulse);
                }
                

                GameManager.rb.AddTorque(-1);
            }

            GameObject g = GameObject.Find("GameManager");
            AudioClip ms = g.GetComponent<GameManager>().moveSound;
            AudioSource.PlayClipAtPoint(ms, Camera.main.transform.localPosition);
        }
    }
    private void Update() 
    {
        //DELETE ME just for pc testing
        if (GameManager.rb != null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector2 cf = GameManager.rb.mass * GameManager.rb.velocity;

                GameManager.rb.AddForce(-cf, ForceMode2D.Impulse); //Stop the Player

                if (GameManager.reverse == false)
                {
                    GameManager.rb.AddForce(movePlayerLeft, ForceMode2D.Impulse);
                }
                else
                {
                    GameManager.rb.AddForce(movePlayerRight, ForceMode2D.Impulse);
                }
                

                GameManager.rb.AddTorque(-1);
                GameObject g = GameObject.Find("GameManager");
                AudioClip ms = g.GetComponent<GameManager>().moveSound;
                AudioSource.PlayClipAtPoint(ms, Camera.main.transform.localPosition);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Vector2 cf = GameManager.rb.mass * GameManager.rb.velocity;

                GameManager.rb.AddForce(-cf, ForceMode2D.Impulse); //Stop the Player
                if(GameManager.reverse==false)
                {
                    GameManager.rb.AddForce(movePlayerRight, ForceMode2D.Impulse);
                }
                else
                {
                    GameManager.rb.AddForce(movePlayerLeft, ForceMode2D.Impulse);
                }

                GameManager.rb.AddTorque(1);
                GameObject g = GameObject.Find("GameManager");
                AudioClip ms = g.GetComponent<GameManager>().moveSound;
                AudioSource.PlayClipAtPoint(ms, Camera.main.transform.localPosition);
            }
        }
    }
}
