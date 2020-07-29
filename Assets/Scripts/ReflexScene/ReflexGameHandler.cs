using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReflexGameHandler : MonoBehaviour
{
    public Card[] cards;
    public Button[] buttons;
    public Button[] playAndQuit;
    public TextMeshProUGUI[] buttonText;
    public CardDisplay cd;

    //Kopplat till timern
    public TextMeshProUGUI timerText;
    private float timer = 10;
    private bool hasTimeRunOut = true;
    
    //Kopplat till poängen
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    private int score;
    private float rightAnswer;
    //Spel Canvas
    public Canvas gameCanvas;
    public Canvas scoreCanvas;
    public Canvas countdownCanvas;

    //Nedräknings canvas
    public TextMeshProUGUI countdownText;
    private int countdown = -1;


    public float rotationSpeed;
    private bool flippingTheCard = false;
    private bool animalImageDelay = false;
    Quaternion firstStance;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(StartNewGame(1f));
    }

    // Update is called once per frame
    void  FixedUpdate()
    {
        if (flippingTheCard)
        {
            cd.transform.Rotate(new Vector3(0f, 10f, 0f), rotationSpeed);
            //Debug.Log("Card angle Y: " + cd.transform.rotation.y.ToString());
            if (cd.transform.rotation.y >= 0.68f && cd.transform.rotation.y <= 0.70f || cd.transform.rotation.y >= -0.68f && cd.transform.rotation.y <= -0.70f)
            {
                if (!animalImageDelay)
                {
                    
                    StartCoroutine(AnimalImageDelay(0.5f));
                    Debug.Log(!cd.animalSprite.enabled);
                    rotationSpeed = rotationSpeed * -1;
                }
            }
            
            if(cd.transform.rotation == firstStance)
            {
                rotationSpeed = rotationSpeed * -1;
                flippingTheCard = false;
                foreach (Button b in buttons)
                {
                    b.enabled = true;
                }
            }

            

        }
        scoreText.text = "Poäng: " + score.ToString();
        if (!hasTimeRunOut)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("f1");
        }
        if (timer <= 0f && !hasTimeRunOut)
        {
            hasTimeRunOut = true;
            StartCoroutine(Lost(1f));
        }
    }

    //Här har tiden gått ut och man går från Game canvas till Score canvas
    IEnumerator Lost(float time)
    {
        foreach(var b in buttons)
        {
            b.enabled = false;
        }
        yield return new WaitForSeconds(time);
        
        gameCanvas.enabled = false;
        scoreCanvas.enabled = true;
        finalScoreText.text = "Poäng: " + score.ToString();
        foreach (var b in buttons)
        {
            b.enabled = true;
        }

      
    }

    // Här startas nedräkningen för ett nytt game
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

        yield return new WaitForSeconds(time);

        countdown -= 1;
        if(countdown != 0)
        {
            StartCoroutine(StartNewGame(time));
        }
        if(countdown == 0)
        {
            //Här startar hela spelet om
            gameCanvas.enabled = true;
            scoreCanvas.enabled = false;
            countdownCanvas.enabled = false;
            StartCoroutine(AnimalImageDelay(0.1f));
            
            
            countdown = -1;
            hasTimeRunOut = false;
        }
        yield break;

    }

    IEnumerator SmallDelay(float time)
    {
        foreach(var i in playAndQuit)
        {
            i.enabled = false;
        }
        yield return new WaitForSeconds(time);
        foreach (var i in playAndQuit)
        {
            i.enabled = true;
        }
    }
    public void PlayAgainButton()
    {
        score = 0;
        timer = 10f;

        StartCoroutine(SmallDelay(1f));
        StartCoroutine(StartNewGame(1f));
    }



    public void QuitGame(string scene)
    {
        StartCoroutine(SmallDelay(1f));
        SceneManager.LoadScene(scene);
    }
    //startar en snurr åt ena hållet, när den kommit till mittpunkt så ändras bild och snurrar tillbaka samma håll.
    IEnumerator AnimalImageDelay(float time)
    {
        cd.card = cards[Convert.ToInt32(UnityEngine.Random.Range(0f, 39f))];
        cd.Refresh();
        int randomNumber = Convert.ToInt32(UnityEngine.Random.Range(0f, 3f));
        //Debug.Log(randomNumber.ToString());
        foreach(var bt in buttonText)
        {
            bt.text = "null";
        }
        for(int i = 0; i < buttons.Length; i++)
        {
            if (i == randomNumber)
            {
                buttonText[i].text = cd.card.name;
            }
            else
            {
                int check = 0;
                do
                {
                    check = 0;
                    buttonText[i].text = cards[Convert.ToInt32(UnityEngine.Random.Range(0f, 39f))].name;
                    foreach(var bt in buttonText)
                    {
                        if(buttonText[i].text == bt.text || buttonText[i].text == cd.card.name)
                        {
                            check++;
                        }
                    }
                    Debug.Log(check.ToString());
                }
                while (check > 1);
            }
            


        }
        
        animalImageDelay = true;
        yield return new WaitForSeconds(time);
        
        animalImageDelay = false;

        yield break;
    }
    public void FlipTheCard(TextMeshProUGUI b)
    {
        if(b.text == cd.card.name)
        {
            //Debug.Log("You found the right choice");
            score++;
            
            cd.ps.Play();
            rightAnswer = 1f;
            if (rightAnswer + timer >= 10f)
            {
                timer = 10f;
            }
            else
            {

                timer += rightAnswer;
            }
        }
        else
        {
            //spela ljud här    
        }
        foreach (var bu in buttons)
        {
            bu.enabled = false;
        }
        firstStance = cd.transform.rotation;
        flippingTheCard = true;
    }

    


}
