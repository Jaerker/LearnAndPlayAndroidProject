using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlphabetGameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public void b_TemporaryButton()
    {
        SceneManager.LoadScene("IntroScreen");
    }
}
