using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;
using UnityEngine.UIElements;

public class GamePlay : MonoBehaviour
{
    public PlayfabManager playfab;
    public AnimationManager animationManager;

    public GameObject startMenuGamePanel;
    public GameObject mainGamePanel;
    public GameObject highScorePanel;
    public GameObject textPanel;
    public GameObject soundPanel;
    public GameObject onGameFinishedPanel;
    public GameObject extraLifeButton;
    public GameLogic logic;
    public GameObject buttonPanel;
    public GameObject EnterNumberPanel;


    public AudioSource heartBeatSound;
    public AudioSource backGroundSound;

 
    public TextMeshProUGUI textClue;
    public TextMeshProUGUI textHiddenWord;
    public TextMeshProUGUI textScoreCard;
    public TextMeshProUGUI textTimerCard;
    public TextMeshProUGUI quoteText;
    public TextMeshProUGUI writerText;
    public TextMeshProUGUI enterValidNumerText;
    public TextMeshProUGUI registerNumberButtonText;
    public TextMeshProUGUI registerNumberPanelMessage;


    private bool isHeartBeatOn = false;
    public int chanceLeft;
    public int scoreCard;
    public float timerCard;
    private string chosenWord;
    private string hiddenWord;
    private string[] wordList;
    private static string[] clueList;
    public string[] quoteList;
    public string[] quoteWriterList;
    private readonly int[] randomArray = new int[564];
    private readonly int[] randomArrayForQuotes = new int[29];

    private string clue;
    private int randomArrayIndex = 0;
    private int randomArrayIndexForQuotes = 0;
    private float curVolBackground;
    private float curVolEffect;
    public bool backToMenuButton;

    private bool quoteRunning;

    private void Awake()
    {
        curVolBackground = 1f;
        curVolEffect = 1f;

        quoteRunning = false;

        //Debug.Log("Game play initiated");
        var textFile = Resources.Load<TextAsset>("answer1").ToString();
        wordList = textFile.Split('|');
        var textFile2 = Resources.Load<TextAsset>("question1").ToString();
        clueList = textFile2.Split('|');
        var textFile3 = Resources.Load<TextAsset>("quotes1").ToString();
        quoteList = textFile3.Split('|');
        var textFile4 = Resources.Load<TextAsset>("quotewriters1").ToString();
        quoteWriterList = textFile4.Split('|');

        highScorePanel.SetActive(false);
        textPanel.SetActive(true);
        backGroundSound.Play();
        startMenuGamePanel.SetActive(true);
        mainGamePanel.SetActive(false);

        //Debug.Log(wordList.Length);
        //Debug.Log(clueList.Length);
    }

    private void Start()
    {
        EnterNumberPanel.SetActive(false);
        for (int i = 0; i < quoteList.Length; i++)
        {
            randomArrayForQuotes[i] = i;
            //Debug.Log(randomArrayForQuotes[i]);
        }

        for (int i = 0; i < quoteList.Length; i++)
        {
            int tmp = randomArrayForQuotes[i];
            int swtch = Random.Range(0, quoteList.Length);
            randomArrayForQuotes[i] = randomArrayForQuotes[swtch];
            randomArrayForQuotes[swtch] = tmp;
        }

        

    }

    public void StartButton()
    {
        
        //Debug.Log(quoteList.Length);
        for (int i = 0; i < 5; i++)
        {
            logic.healthBar[i].SetActive(true);
        }

        for (int p = 65; p < 91; p++)
        {
            logic.keyboardButtons[p - 65].SetActive(true);
        }
        logic.curScore = 0;
        logic.timeBonus = 0;
        logic.keyboardPanel.SetActive(true);
        logic.gameLostDisplay.SetActive(false);
        
        //rulesText.SetActive(false);
        StartCoroutine(TransitionDelay());
        mainGamePanel.SetActive(true);
        //Debug.Log(wordList.Length + " = word length");
        backToMenuButton = false;
        randomArrayIndex = 0;
        chanceLeft = 0;
        scoreCard = 0;
        timerCard = 35;
        for (int i = 0; i < wordList.Length; i++)
        {
            randomArray[i] = i;
        }

        for (int i = 0; i < 8; i++)
        {
            int tmp = randomArray[i];
            int swtch = Random.Range(0, 8);
            randomArray[i] = randomArray[swtch];
            randomArray[swtch] = tmp;
        }

        for (int i = 8; i < randomArray.Length; i++)
        {
            int tmp = randomArray[i];
            int swtch = Random.Range(14, randomArray.Length);
            randomArray[i] = randomArray[swtch];
            randomArray[swtch] = tmp;
        }

        clue = "";
        chosenWord = wordList[randomArray[randomArrayIndex]];
        clue = clueList[randomArray[randomArrayIndex]];
        //Debug.Log(randomArray[randomArrayIndex]);
        StartCoroutine(DelayText(clue, textClue));

        logic.answerWhenFail = chosenWord;
        randomArrayIndex++;

        hiddenWord = "";

        for (int i = 0; i < chosenWord.Length; i++)
        {
            if (chosenWord[i].ToString() == " ")
            {
                hiddenWord += " ";
            }
            else
            {
                hiddenWord += "-";
            }
        }
        textHiddenWord.color = Color.red;
        textHiddenWord.text = hiddenWord;
        //Debug.Log(hiddenWord);
        textScoreCard.text = (scoreCard*10).ToString();
    }

    IEnumerator TransitionDelay()
    {
        yield return new WaitForSeconds(1f);
        startMenuGamePanel.SetActive(false);
    }
    void FixedUpdate()
    {

        backGroundSound.volume = curVolBackground;
        logic.wrongPressSound.volume = curVolEffect;
        logic.buttonSound.volume = curVolEffect;
        logic.gameComplete.volume = curVolEffect;



        if (mainGamePanel.activeSelf == true)
        {
            if (timerCard <= 0 && hiddenWord.Contains("-"))
            {
                logic.LevelFailed();
                heartBeatSound.Stop();
            }
            else if (!isHeartBeatOn && timerCard < 10 && !backToMenuButton)
            {
                //Debug.Log("Heart Playing");
                heartBeatSound.Play();
                isHeartBeatOn = true;
            }
            else
            {
                if (!hiddenWord.Contains("-"))
                {
                    textTimerCard.text = "0";
                }
                else
                {
                    textTimerCard.text = timerCard.ToString("0.0");
                    timerCard -= Time.deltaTime;
                }
            }
        }
        
        if (startMenuGamePanel.activeSelf == true && quoteRunning==false)
        {
            quoteRunning = true;
            writerText.text = quoteWriterList[randomArrayForQuotes[randomArrayIndexForQuotes]];
            StartCoroutine(DelayTextForQuote(quoteList[randomArrayForQuotes[randomArrayIndexForQuotes]], quoteText)); 
            randomArrayIndexForQuotes++;
            randomArrayIndexForQuotes %= quoteList.Length - 1;

        }

        if(timerCard>=20)
        {
            textTimerCard.color = Color.green;
        }
        else if(timerCard<20 && timerCard>=10)
        {
            textTimerCard.color = Color.yellow;
        }
        else if(timerCard<10)
        {
            textTimerCard.color = Color.red;
        }

    }

    public void ChangeVars()
    {
        animationManager.SlideUpLevelWonPanel();

        for (int i = 0; i < 5; i++)
        {
            logic.healthBar[i].SetActive(true);
        }
        for (int p = 65; p < 91; p++)
        {
            logic.keyboardButtons[p - 65].SetActive(true);
        }
        backToMenuButton = false;
        clue = "";
        logic.keyboardPanel.SetActive(true);
        logic.curScore += 1;
        logic.timeBonus += (int)timerCard / 10;
        //Debug.Log(logic.curScore + logic.timeBonus);
        try
        {
            chosenWord = wordList[randomArray[randomArrayIndex]];
            clue = clueList[randomArray[randomArrayIndex]];
            logic.answerWhenFail = wordList[randomArray[randomArrayIndex]];
        }
        catch
        {
            onGameFinishedPanel.SetActive(true);
            //Debug.Log("try catch");
        }

        StartCoroutine(DelayText(clue, textClue));


        timerCard = 35;
        randomArrayIndex++;
        chanceLeft = 0;
        hiddenWord = "";

        for (int i = 0; i < chosenWord.Length; i++)
        {
            if (chosenWord[i].ToString() == " ")
            {
                hiddenWord += " ";
            }
            else
            {
                hiddenWord += "-";
            }
        }

        textHiddenWord.color = Color.red;
        textHiddenWord.text = hiddenWord;
        //logic.gameWonDisplay.SetActive(false);
        logic.gameLostDisplay.SetActive(false);
        //Debug.Log(hiddenWord);
        scoreCard++;
        if (scoreCard != 0 && scoreCard % 5 == 0)
        {
            logic.showWatchAdButton = true;
        }
        textScoreCard.text = (scoreCard*10).ToString();
    }

    IEnumerator DelayText(string str, TextMeshProUGUI textUIElement)
    {

        textUIElement.text = "";
        foreach (char letter in str.ToCharArray())
        {
            textUIElement.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DelayTextForQuote(string str, TextMeshProUGUI textUIElement)
    {

        textUIElement.text = "";
        foreach (char letter in str.ToCharArray())
        {
            textUIElement.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        //Debug.Log("And the loop keeps running");
        yield return new WaitForSeconds(3f);
        quoteRunning = false;
        
    }
    public void GoToStartPanel()
    {
        //rulesText.SetActive(true);
        backToMenuButton = true;
        startMenuGamePanel.SetActive(true);
        mainGamePanel.SetActive(false);
        animationManager.BackToMenu();
    }

    public void TurnOffHighScore()
    {

        //commnented out for testing purpose
        highScorePanel.SetActive(false);
        textPanel.SetActive(true);
    }
    public void ShowHighScore()
    {
        if (playfab.isInternet)
        {

            highScorePanel.SetActive(true);
        }
       
    }
    public void ShowNumberPanel()
    {
        if (playfab.isInternet)
        {
            EnterNumberPanel.SetActive(true);
        }
        else
        {
            SSTools.ShowMessage("Not connected to server", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }
    public void TurnOffNumberPanel()
    {
        EnterNumberPanel.SetActive(false);
    }

    public void ShowSoundPanel()
    {
        highScorePanel.SetActive(false);
        //textPanel.SetActive(false);
        soundPanel.SetActive(true);
        highScorePanel.SetActive(false);
    }
    public void QuitApp()
    {
        Application.Quit();
    }
    public void ChangeVolBackGround(float newVol)
    {
        curVolBackground = newVol;
    }

    public void ChangeVolEffect(float newVol)
    {
        curVolEffect = newVol;
    }
    public void GoToMenuFromSoundPanel()
    {
        textPanel.SetActive(true);
        //soundPanel.SetActive(false);
    }
    public void PressedA()
    {
        logic.Algo("A", ref chosenWord, ref hiddenWord);
    }
    public void PressedB()
    {
        logic.Algo("B", ref chosenWord, ref hiddenWord);
    }
    public void PressedC()
    {
        logic.Algo("C", ref chosenWord, ref hiddenWord);
    }
    public void PressedD()
    {
        logic.Algo("D", ref chosenWord, ref hiddenWord);
    }
    public void PressedE()
    {
        logic.Algo("E", ref chosenWord, ref hiddenWord);
    }
    public void PressedF()
    {
        logic.Algo("F", ref chosenWord, ref hiddenWord);
    }
    public void PressedG()
    {
        logic.Algo("G", ref chosenWord, ref hiddenWord);
    }
    public void PressedH()
    {
        logic.Algo("H", ref chosenWord, ref hiddenWord);
    }
    public void PressedI()
    {
        logic.Algo("I", ref chosenWord, ref hiddenWord);
    }
    public void PressedJ()
    {
        logic.Algo("J", ref chosenWord, ref hiddenWord);

    }
    public void PressedK()
    {
        logic.Algo("K", ref chosenWord, ref hiddenWord);
    }
    public void PressedL()
    {
        logic.Algo("L", ref chosenWord, ref hiddenWord);
    }
    public void PressedM()
    {
        logic.Algo("M", ref chosenWord, ref hiddenWord);
    }
    public void PressedN()
    {
        logic.Algo("N", ref chosenWord, ref hiddenWord);
    }
    public void PressedO()
    {
        logic.Algo("O", ref chosenWord, ref hiddenWord);
    }
    public void PressedP()
    {
        logic.Algo("P", ref chosenWord, ref hiddenWord);
    }
    public void PressedQ()
    {
        logic.Algo("Q", ref chosenWord, ref hiddenWord);
    }
    public void PressedR()
    {
        logic.Algo("R", ref chosenWord, ref hiddenWord);
    }
    public void PressedS()
    {
        logic.Algo("S", ref chosenWord, ref hiddenWord);
    }
    public void PressedT()
    {
        logic.Algo("T", ref chosenWord, ref hiddenWord);
    }
    public void PressedU()
    {
        logic.Algo("U", ref chosenWord, ref hiddenWord);
    }
    public void PressedV()
    {
        logic.Algo("V", ref chosenWord, ref hiddenWord);
    }
    public void PressedW()
    {
        logic.Algo("W", ref chosenWord, ref hiddenWord);
    }
    public void PressedX()
    {
        logic.Algo("X", ref chosenWord, ref hiddenWord);
    }
    public void PressedY()
    {
        logic.Algo("Y", ref chosenWord, ref hiddenWord);
    }
    public void PressedZ()
    {
        logic.Algo("Z", ref chosenWord, ref hiddenWord);
    }
}
