using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOnOff : MonoBehaviour {

    AudioListener audioListener;
    public Button button;
    public Sprite on;
    public Sprite off;
    Image buttonImage;

    private void Start()
    {
        //get the image component of the sounds button
        buttonImage = button.GetComponent<Image>();

        if (PlayerPrefs.GetInt("sounds")==0)
        {
            AudioListener.pause = false;
            buttonImage.sprite = on;
            buttonImage.color = Color.green;
        }
        else
        {
            AudioListener.pause = true;
            buttonImage.sprite = off;
            buttonImage.color = Color.red;
        }
    }

    public void SoundToggle()
    {
        print("Sound Toggle");

        //turn sounds off
        if (PlayerPrefs.GetInt("sounds")==0)
        {
            AudioListener.pause = true;
            buttonImage.sprite = off;
            PlayerPrefs.SetInt("sounds", 1);
            buttonImage.color = Color.red;
        }
        else // turn sounds on
        {
            AudioListener.pause = false;
            buttonImage.sprite = on;
            PlayerPrefs.SetInt("sounds", 0);
            buttonImage.color = Color.green;
        }
    }
}
