using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public GameObject fullBlock;
    public GameObject smallBlock;
    public GameObject leftTouchArea;
    public GameObject rightTouchArea;
    public GameObject leftBorder;
    public GameObject rightBorder;
    public GameObject topBorder;
    public GameObject bottomBorder;
    public GameObject playerPrefab;
    public GameObject playPanel;
    public GameObject playPanelInstructions;
    public GameObject gameOverPanel;
    public GameObject homePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public AudioClip moveSound;
    public AudioClip scoreSound;
    public TextMeshProUGUI Info;

    [Header("Power Ups")]
    public GameObject powerupInvincible;
    public GameObject powerupReverse;
    public GameObject powerupReverseGravity;
    public GameObject powerupSpeed;

    [HideInInspector]
    public static bool invincible;
    public static bool reverse;
    public static bool reverseGravity;
    public static bool speed;
    public static bool powerup;

    //public GameObject movementManager;
    [HideInInspector]
    public int score, highScore;

    [HideInInspector]
    public float camX, camY;
    [HideInInspector]
    public static bool gameStarted;
    public static GameObject player;
    public static Rigidbody2D rb;

    float numOfScreenLevels;
    float xScale, xScaleDivisor, yScale;
    float minX, maxX;
    float xOffset = 0.25f;
    float yOffset;
    public static float zPos = 15;
    public Toggle toggleRandom, toggleAffectors;
    [HideInInspector]
    public static float timeSinceLastAffector = 0;
    float timeBetweenAffectors = 5f; //15
    Color currentColour;

    // Use this for initialization
    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore");

        //Screen units
        camY = Camera.main.orthographicSize * 2; //Full Height
        camX = (camY * Screen.width / Screen.height); // covers different aspect aspect ratios

        xScaleDivisor = 2.5f;
        xScale = camX / xScaleDivisor; //2.75 how wide to make the blank block
        yScale = camY / 25; // 25 how tall to make the blank block

        //Where to instantiate new rows
        yOffset = Camera.main.transform.localPosition.y + camY / 2 + yScale;

        //How far to the left I allow the blank block to be
        minX = (float)(-camX / 2 + ((fullBlock.transform.localScale.x / 2) * xScale) + xOffset);
        maxX = (float)(camX / 2 - ((fullBlock.transform.localScale.x / 2) * xScale) - xOffset);

        gameStarted = false;

        SetBorders();
        SetTouchObjects();
         
        Home();
    }

    public void Home()
    {
        homePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        playPanel.SetActive(false);

        leftTouchArea.SetActive(false);
        rightTouchArea.SetActive(false);

        gameStarted = false;
    }

    public void PlayWrapper()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        if(!toggleRandom.isOn)
        {
            Random.InitState(27);
        }

        score = 0;
        Info.text = "";

        //Affectors (Powerups)
        invincible = false;
        reverse = false;
        speed = false;
        Time.timeScale = 1;
        reverseGravity = false;
        Physics2D.gravity = new Vector2(0, -9.81f);
        powerup = false;

        //setup initial level block width and colour
        xScaleDivisor = 2.5f;
        xScale = camX / xScaleDivisor;
        currentColour = new Color(.74f, 1, 0.4f); // Color.grey; //dependent on colour order in new block

        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        yOffset = Camera.main.transform.localPosition.y + (camY / 2 + yScale);

        gameStarted = false;

        homePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        
        player=Instantiate(playerPrefab, new Vector3(0, 0, zPos), Quaternion.identity);// Quaternion.AngleAxis(90, Vector2.));
        player.GetComponentInChildren<ParticleSystem>().Stop();
        rb = player.GetComponent<Rigidbody2D>();
        rb.simulated = false; // makes you wait till pressed ... changed in movement.cs

        playPanel.SetActive(true);
        playPanelInstructions.SetActive(true);
        timeSinceLastAffector = 0;
        NewBlock();
        leftTouchArea.SetActive(true);
        rightTouchArea.SetActive(true);
        gameStarted = true;

        //Set intial scores, as only updated when new level created
        highScore = PlayerPrefs.GetInt("highScore");
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();

        yield return null;
    }

    void GameOver()
    {
        homePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        playPanel.SetActive(false);
        leftTouchArea.SetActive(false);
        rightTouchArea.SetActive(false);
        gameStarted = false;
    }

    void SetTouchObjects()
    {
        leftTouchArea.transform.localScale = new Vector2(camX / 2, camY);
        leftTouchArea.transform.localPosition = new Vector3(-camX / 4, 0, zPos); //half the size of screen..so half of that over to make it fit.
        
        rightTouchArea.transform.localScale = new Vector2(camX / 2, camY);
        rightTouchArea.transform.localPosition = new Vector3(camX / 4, 0, zPos);
        
    }

    void SetBorders()
    {
        leftBorder.transform.localScale = new Vector2(camX / 2, camY * 2);
        leftBorder.transform.localPosition = new Vector3(-camX / 2 - (leftBorder.transform.localScale.x / 2), 0, zPos);

        rightBorder.transform.localScale = new Vector2(camX / 2, camY * 2);
        rightBorder.transform.localPosition = new Vector3(camX / 2 + (rightBorder.transform.localScale.x / 2), 0, zPos);

        topBorder.transform.localScale = new Vector2(camX * 2, -camY / 2);
        topBorder.transform.localPosition = new Vector3(0, camY / 2 - (topBorder.transform.localScale.y / 2), zPos);

        bottomBorder.transform.localScale = new Vector2(camX * 2, -camY / 2);
        bottomBorder.transform.localPosition = new Vector3(0, -camY / 2 + (bottomBorder.transform.localScale.y / 2),zPos);
    }

    void NewBlock()
    {
        if (gameStarted == true)
        {
            score++;
            AudioSource.PlayClipAtPoint(scoreSound, Camera.main.transform.localPosition);
        }

        if (toggleAffectors.isOn)
        {
            Affectors();
        }
            
        GameObject fb = Instantiate(fullBlock, new Vector3(0, 0, zPos), Quaternion.identity);
        fb.transform.rotation = Quaternion.Euler(0, 0, Random.Range(10,-10));
        GameObject sb1 = Instantiate(smallBlock, new Vector3(0, 0, zPos), Quaternion.identity);
        sb1.transform.rotation = Quaternion.Euler(0, 0, Random.Range(180, -180));
        GameObject sb2 = Instantiate(smallBlock, new Vector3(0, 0, zPos), Quaternion.identity);
        sb2.transform.rotation = Quaternion.Euler(0, 0, Random.Range(180, -180));
        //Set Colours
        sb1.GetComponent<SpriteRenderer>().color = currentColour;
        sb2.GetComponent<SpriteRenderer>().color = currentColour;
        SpriteRenderer[] sr = fb.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer s in sr)
        {
            s.color = currentColour;
        }
        

        //change the width the of levelblock according to progress
        if ((score % 10 == 0)&&(gameStarted))
        {
            xScaleDivisor += 0.1f;
            xScale = camX / xScaleDivisor; //how wide to make the blank block .. reduces over time

            //Change colours
            int colourIndex = score % 7; // seven results, relate to 7 colours (ordered appropriately according to mod result)
            switch (colourIndex)
            {

                case 3:
                    currentColour = Color.cyan;
                    break;
                case 6:
                    currentColour = Color.yellow;
                    break;
                case 2:
                    currentColour = Color.magenta;
                    break;
                case 5:
                    currentColour = Color.blue;
                    break;
                case 1:
                    currentColour = new Color(0.75f, 0.25f, 0.5f); // brown
                    break;
                case 4:
                    currentColour = new Color(1,0.5f,0); //orange
                    break;
                case 0:
                    currentColour = new Color(.74f, 1, 0.4f); //greeny;
                    break;
                default:
                    break;
            }
        }
            

        //setup fullblock
        fb.transform.localScale = new Vector2(xScale, yScale);// cb.transform.localScale.z);
        fb.transform.localPosition = new Vector3(Random.Range(minX, maxX), yOffset, zPos); //puts current block just offscreen
        
        //setup smallblocks
        sb1.transform.localPosition = new Vector3(Random.Range(minX, maxX), 
                                    Random.Range(yOffset + sb1.transform.localScale.y*3, yOffset+sb1.transform.localScale.y*5), zPos);
        sb2.transform.localPosition = new Vector3(Random.Range(minX, maxX), 
                                    Random.Range(yOffset + camY/2 - sb2.transform.localScale.y*3, yOffset + camY /2-sb2.transform.localScale.y*5), zPos);


    }

    void Affectors()
    {
        if(timeSinceLastAffector > timeBetweenAffectors) // time for a new affector
        {
            int a = Random.Range(0, 100);
            a = 7;
            if(Mathf.Clamp(a, 0, 25)==a)
            {
                //Invincible
                GameObject powerup = Instantiate(powerupInvincible, new Vector3(Random.Range(minX, maxX),
                                Random.Range(yOffset + powerupInvincible.transform.localScale.y * 3 / 6, yOffset + powerupInvincible.transform.localScale.y * 5 / 6), zPos), Quaternion.identity);
            }
            else if(Mathf.Clamp(a,26,50)==a) 
            {
                // Reverse
                GameObject powerup = Instantiate(powerupReverse, new Vector3(Random.Range(minX, maxX),
                                Random.Range(yOffset + powerupReverse.transform.localScale.y * 3 / 6, yOffset + powerupReverse.transform.localScale.y * 5 / 6), zPos), Quaternion.identity);
            }
            else if(Mathf.Clamp(a,51,75)==a)
            {
                // ReverseGravity
                GameObject powerup = Instantiate(powerupReverseGravity, new Vector3(Random.Range(minX, maxX),
                                Random.Range(yOffset + powerupReverseGravity.transform.localScale.y * 3 / 6, yOffset + powerupReverseGravity.transform.localScale.y * 5 / 6), zPos), Quaternion.identity);
            }
            else if(Mathf.Clamp(a,76,100)==a)
            {
                //Speed up!
                GameObject powerup = Instantiate(powerupSpeed, new Vector3(Random.Range(minX, maxX),
                               Random.Range(yOffset + powerupSpeed.transform.localScale.y * 3 / 6, yOffset + powerupSpeed.transform.localScale.y * 5 / 6), zPos), Quaternion.identity);
            }

            timeSinceLastAffector = 0;
        }
    }

    private void Update()
    {
        timeSinceLastAffector += Time.deltaTime;

        //always keep yoffset just out of camera view
        yOffset = Camera.main.transform.localPosition.y + (camY / 2 + yScale);

        //get all the blocks and check if one of them is in the middle of sceen,
        //if so, then generate another block
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("LevelBlock");
        foreach (GameObject b in blocks)
        {
            if ((b.transform.localPosition.y < Camera.main.transform.localPosition.y) && (blocks.Length < 2)) //3 levelblocks per level (left, right and center)
            {

                NewBlock();

                if (scoreText != null)
                {
                    highScore = PlayerPrefs.GetInt("highScore");
                    scoreText.text = score.ToString();
                    highScoreText.text = highScore.ToString();
                }
            }
        }
        
    }
}
