using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour {

    public GameObject gamePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        gamePanel.SetActive(false);

        //Destroy all the blocks
        List<GameObject> blocks = new List<GameObject>();
        blocks.AddRange(GameObject.FindGameObjectsWithTag("Block"));
        blocks.AddRange(GameObject.FindGameObjectsWithTag("LevelBlock"));
        blocks.AddRange(GameObject.FindGameObjectsWithTag("Powerup"));

        foreach (GameObject block in blocks)
        {
            Destroy(block);
        }

        //update scores
        //I should be able to reference this somehow without this find stuff
        GameObject g = GameObject.Find("GameManager");
        int s = g.GetComponent<GameManager>().score;
        if (s > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", s);
        }

        scoreText.text = ""+s;
        highScoreText.text = ""+(PlayerPrefs.GetInt("highScore"));
    }

}

