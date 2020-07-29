using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public Card[] cards;

    public Button[] buttons;

    public CardDisplay[] cardsOnScreen;
    //gc is GameCanvas
    public TextMeshProUGUI fails;
    private int failedTimes;

    public TextMeshProUGUI gc_t_points;
    private int points;
    public TextMeshProUGUI TimerText;
    private float timer;
    private float rightAnswer;
    private bool isRunningTimer = true;
    private int newRound = 1;


    //sc is ScoreCanvas
    public TextMeshProUGUI sc_t_points;
    public TextMeshProUGUI sc_t_scoreGreeting;


    public Canvas GameCanvas;
    public Canvas ScoreCanvas;



    // Start is called before the first frame update
    void Start()
    {
        timer = 10.0f;
        GameCanvas.enabled = true;
        ScoreCanvas.enabled = false;
        ChangeCards();

    }


    //Gör så att det blir en delay efter man tryckt på kortet eller när man förlorat, lägger till tid om man får rätt
    IEnumerator PressingButtonAction(float time, CardDisplay cd)
    {
        isRunningTimer = false;
        
        yield return new WaitForSeconds(time);

        if (failedTimes >= 3)
       
        {
            FlipCanvases();
        }
        ChangeCards();

        foreach (Button b in buttons)
        {
            b.enabled = true;
        }
        isRunningTimer = true;
        yield break;

    }
    //Om du går över tiden
    IEnumerator LostByTime(float time)
    {
        foreach (Button b in buttons)
        {
            b.enabled = false;
        }
        yield return new WaitForSeconds(time);

        FlipCanvases();
        
        foreach (Button b in buttons)
        {
            b.enabled = true;
        }

    }
    public void PressCard(CardDisplay cd)
    {
        newRound++;

        foreach( Button b in buttons)
        {
            b.enabled = false;
        }
        int checker = 0;

        foreach(var card in cardsOnScreen)
        {
            if (card.card.animalTypeID == cd.card.animalTypeID) checker++;
        }
        if(checker == 1)
        {
            Debug.Log("You found the right choice");
            points++;
            cd.ps.Play();
            rightAnswer = 2f / (float.Parse(newRound.ToString()) * 0.15f);
            if (rightAnswer >= 1.5f)
            {
                rightAnswer = 1.5f;
            }
            if (rightAnswer + timer >= 10f)
            {
                timer = 10f;
            }
            else
            {
                
                timer += rightAnswer;
            }
            Debug.Log(rightAnswer.ToString());

        }
        else
        {
            Debug.Log("You got it wrong");
            fails.text += "X";
            failedTimes++;
        }


        StartCoroutine(PressingButtonAction(1f, cd));



    }

    //Ändrar vilken Canvas som ska vara synlig
    public void FlipCanvases()
    {
        if (GameCanvas.enabled)
        {
            sc_t_scoreGreeting.text = "Såhär långt kom du!";

            sc_t_points.text = "Poäng: " + points.ToString();
            failedTimes = 0;
            points = 0;
            GameCanvas.enabled = false;
            ScoreCanvas.enabled = true;
            isRunningTimer = false;
            timer = 10f;
            newRound = 1;
        }
        else
        {
            GameCanvas.enabled = true;
            ScoreCanvas.enabled = false;
            isRunningTimer = true;

        }
        fails.text = "";
        failedTimes = 0;

    }

    //Sjukt självklart vad denna gör egentligen
    public void QuitApp()
    {
        SceneManager.LoadScene(0);
    }



    //Detta ändrar alla korten till en ny omgång
    void ChangeCards()
    {

        int chosenCard = Convert.ToInt32(UnityEngine.Random.Range(0f, 3f));
        int evenCards = Convert.ToInt32(UnityEngine.Random.Range(0f, 38f));
        for(int i=0; i < cardsOnScreen.Length; i++)
        {
            if(i== chosenCard)
            {
                do
                {
                    cardsOnScreen[i].card = cards[Convert.ToInt32(UnityEngine.Random.Range(0f, 38f))];
                }
                while (cardsOnScreen[i].card.animalTypeID == cards[evenCards].animalTypeID);
            }
            else
            {
                do
                {
                    cardsOnScreen[i].card = cards[Convert.ToInt32(UnityEngine.Random.Range(0f, 38f))];
                }
                while (cardsOnScreen[i].card.animalTypeID != cards[evenCards].animalTypeID);
            }
            cardsOnScreen[i].Refresh();
        }
    }

    private void FixedUpdate()
    {
        gc_t_points.text = "Poäng: " + points.ToString();
        if (isRunningTimer)
        {
            timer -= Time.deltaTime;
            TimerText.text = "Tid: " + timer.ToString("f1");
        }
        if(timer <= 0f && isRunningTimer)
        {
            isRunningTimer = false;
            StartCoroutine(LostByTime(1f));   
        }
    }





}
