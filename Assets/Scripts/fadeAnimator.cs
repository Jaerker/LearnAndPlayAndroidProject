
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeAnimator : MonoBehaviour
{
    public Animator animation;

    private int lvlIndex;


    public void LoadAnimation(int lvl)
    {
        lvlIndex = lvl;
        animation.SetTrigger("Fade");
    }

    public void LoadGame()
    {
        if (lvlIndex == -1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(lvlIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
