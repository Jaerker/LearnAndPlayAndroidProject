
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public Canvas IntroCanvas;
    public Canvas MenuCanvas;
    public Canvas CreditsCanvas;



    // Start is called before the first frame update
    void Start()
    {
        if (GlobalHandler.gameRunning == false)
        {
          
            IntroCanvas.enabled = true;
            MenuCanvas.enabled = false;
            CreditsCanvas.enabled = false;
            GlobalHandler.gameRunning = true;
        }
        else
        {
            IntroCanvas.enabled = false;
            MenuCanvas.enabled = true;
            CreditsCanvas.enabled = false;
        }

    }

    public void PlaySound(AudioClip ac)
    {
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
    }
    public void b_IntroButton()
    {
        IntroCanvas.enabled =  false;
        MenuCanvas.enabled = true;
    }



    public void b_Credits()
    {
        if(CreditsCanvas.enabled == false)
        {
            IntroCanvas.enabled = false;
            MenuCanvas.enabled = false;
            CreditsCanvas.enabled = true;
        }
        else
        {
            IntroCanvas.enabled = false;
            MenuCanvas.enabled = true;
            CreditsCanvas.enabled = false;
        }
    }



}
