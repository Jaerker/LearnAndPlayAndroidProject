using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public Card[] cards;

    public Button[] buttons;

    //Korten på skärmen
    public CardDisplay[] cardsOnScreen;

    //Kort som kommer flippas på ett helt varv
    private CardDisplay correctCard;

    //rotationshastighet på kortet
    public float rotationSpeed;
    private bool rotateCard = false;
    private bool delayCheckOnCard = false;
    Quaternion firstStance;

    //gc är gameCanvas
    public TextMeshProUGUI fails;
    private int failedTimes;

    public TextMeshProUGUI gc_t_points;
    private int points;
    public TextMeshProUGUI TimerText;
    private float timer;
    private float rightAnswer;
    private bool isRunningTimer = false;
    private int newRound = 1;


    //sc är scoreCanvas
    public TextMeshProUGUI sc_t_points;
    public TextMeshProUGUI sc_t_scoreGreeting;


    public Canvas gameCanvas;
    public Canvas scoreCanvas;
    public Canvas countdownCanvas;

    //Kopplat till ljud
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip regularButtonSound;
    public AudioClip puzzleSolvedSound;


    //variabler till countdownCanvas
    public TextMeshProUGUI countdownText;
    private int countdown = -1;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartNewGame(1f));

    }

    //Ljud metoden, enkel funktion som tar in ett audioclip bara
    public void PlaySound(AudioClip ac)
    {
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
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
        delayCheckOnCard = false;
        foreach ( Button b in buttons)
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
            
            //Debug.Log("You found the right choice");
            points++;
            if(points % 10 == 0)
            {
                PlaySound(puzzleSolvedSound);
            }
            else
            {
                PlaySound(correctSound);
            }
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
            //Sätter rätt kort som ska snurras på när man får ett rätt svar
            correctCard = cd;
            //Detta aktiverar vad som ska hända i Fixed Update nedan
            rotateCard = true;
            

            //Debug.Log(rightAnswer.ToString());

        }
        else
        {
            PlaySound(wrongSound);
            //Debug.Log("You got it wrong");
            fails.text += "X";
            failedTimes++;
        }


        StartCoroutine(PressingButtonAction(0.5f, cd));



    }

    //Ändrar vilken Canvas som ska vara synlig
    public void FlipCanvases()
    {
        if (gameCanvas.enabled)
        {
            sc_t_scoreGreeting.text = "Såhär långt kom du!";

            sc_t_points.text = "Poäng: " + points.ToString();
            failedTimes = 0;
            points = 0;
            gameCanvas.enabled = false;
            scoreCanvas.enabled = true;
            isRunningTimer = false;
            timer = 10f;
            newRound = 1;
        }
        else
        {
            gameCanvas.enabled = false;
            scoreCanvas.enabled = false;
            countdownCanvas.enabled = true;
            StartCoroutine(StartNewGame(1f));

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

    void FixedUpdate()
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



        if (rotateCard)
        {
            correctCard.transform.Rotate(new Vector3(0f, 10f, 0f), rotationSpeed);
            
            
            if(correctCard.transform.rotation.y >= -1f && correctCard.transform.rotation.y <= 1f)
            {
                if (delayCheckOnCard)
                {
                    
                    correctCard.transform.rotation = firstStance;
                    rotateCard = false;
                    
                }
            }
            StartCoroutine(ShortDelayOnCard());
        }

    }

    /*
     * Kommer köras tills countdown värdet är nere på 0, då kommer spelet köras igång. 
     */
    IEnumerator StartNewGame(float time)
    {

        gameCanvas.enabled = false;
        scoreCanvas.enabled = false;
        countdownCanvas.enabled = true;

        //Countdown värdet är alltid -1 när den lämnat hela processen, därav så ser vi till att countdown canvas blir tillgänglig här.
        if (countdown == -1)
        {

            countdown = 3;
            
        }
        countdownText.text = countdown.ToString();
        Debug.Log(countdown);
        yield return new WaitForSeconds(time);
        
        countdown -= 1;
        if (countdown != 0)
        {
            StartCoroutine(StartNewGame(1f));
        }
        else if (countdown == 0)
        {
            //Här startar hela spelet om
            
            //Ser till att rätt Canvas är igång och att Timern sätts igång.
            gameCanvas.enabled = true;
            scoreCanvas.enabled = false;
            countdownCanvas.enabled = false;
            isRunningTimer = true;
            timer = 10.0f;
            countdown = -1;
            ChangeCards();
            firstStance = cardsOnScreen[0].transform.rotation;
            
            


            
            
        }
        
        yield break;

    }

    IEnumerator ShortDelayOnCard()
    {
        yield return new WaitForSeconds(0.5f);
        
        delayCheckOnCard = true;
    }






}
