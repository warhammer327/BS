using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.WSA;
using System.Net;
using UnityEngine.Advertisements;
using UnityEngine.UIElements;

public class GameLogic : MonoBehaviour
{

    public AudioSource gameComplete;
    public AudioSource wrongPressSound;

    public GamePlay gamePlay;
    public AdsManager adsManager;
    public PlayfabManager playfabManager;
    public AnimationManager animationManager;
    public AudioSource buttonSound;
    public GameObject gameWonDisplay;
    public GameObject gameLostDisplay;
    public GameObject[] healthBar;
    public GameObject watchAdButton;
    public TextMeshProUGUI textOnWinning;

    public TextMeshProUGUI textAnswerCard;
    public GameObject[] keyboardButtons;
    public GameObject keyboardPanel;
    public TextMeshProUGUI SessionScore;
    public int curScore;
    public int timeBonus;

    public bool showWatchAdButton;
    public string answerWhenFail;

    private void Awake()
    {
        watchAdButton.SetActive(false);
        showWatchAdButton = false;
    }


    // Start is called before the first frame update
    public void Algo(string pressedButton, ref string chosenWord, ref string hiddenWord)
    {
        if (chosenWord.Contains(pressedButton) && gamePlay.timerCard > 0)
        {
            byte[] val = Encoding.ASCII.GetBytes(pressedButton);
            keyboardButtons[val[0] - 65].SetActive(false);
            int i = chosenWord.IndexOf(pressedButton);
            while (i != -1)
            {
                hiddenWord = hiddenWord.Substring(0, i) + pressedButton + hiddenWord.Substring(i + 1);
                chosenWord = chosenWord.Substring(0, i) + "-" + chosenWord.Substring(i + 1);
                i = chosenWord.IndexOf(pressedButton);
            }

            gamePlay.textHiddenWord.text = hiddenWord;
            if (!hiddenWord.Contains("-"))
            {
                gameComplete.Play();


                textOnWinning.color = Color.red;
                StartCoroutine(DelayText(hiddenWord, gamePlay.textHiddenWord));
                keyboardPanel.SetActive(false);
                Invoke("OnSuccessDisplay", 1.5f);
                for (int p = 65; p < 91; p++)
                {
                    keyboardButtons[p - 65].SetActive(true);
                }
                for (int k = 0; k < 5; k++)
                {
                    healthBar[k].SetActive(true);
                }
            }
        }
        else
        {
            wrongPressSound.Play();
            gamePlay.chanceLeft++;
            //Debug.Log(gamePlay.chanceLeft);
            if (gamePlay.chanceLeft >= 5)
            {
                LevelFailed();
            }
            else
            {
                int index = gamePlay.chanceLeft - 1;
                healthBar[index].SetActive(false);
            }
        }
    }

    public void LevelFailed()
    {
        gamePlay.heartBeatSound.Stop();
        playfabManager.SendToLeaderboard((curScore * 10) + timeBonus);
        if (!gamePlay.backToMenuButton)
        {
            SessionScore.text = "You scored " + ((curScore * 10) + timeBonus).ToString();
            if (showWatchAdButton)
            {
                watchAdButton.SetActive(true);
                showWatchAdButton = false;
            }
            else
            {
                watchAdButton.SetActive(false);
            }
            textAnswerCard.text = answerWhenFail;
            gameLostDisplay.SetActive(true);
            for (int p = 65; p < 91; p++)
            {
                keyboardButtons[p - 65].SetActive(true);
            }
        }
    }
    IEnumerator DelayText(string str, TextMeshProUGUI textUIElement)
    {

        textUIElement.text = "";
        yield return new WaitForSeconds(0.1f);
        textUIElement.text = str;
        yield return new WaitForSeconds(0.1f);
        textUIElement.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        textUIElement.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        textUIElement.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        textUIElement.color = Color.green;
        yield return new WaitForSeconds(0.1f);

    }
    IEnumerator ColorChangeWinningMessage(string str, TextMeshProUGUI textOnWinning)
    {
        textOnWinning.text = "";
        foreach (var letter in str)
        {
            textOnWinning.text += letter;
            yield return new WaitForSeconds(0.05f);
            textOnWinning.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            textOnWinning.color = Color.green;
            yield return new WaitForSeconds(0.05f);
            textOnWinning.color = Color.blue;
            yield return new WaitForSeconds(0.05f);
            textOnWinning.color = Color.yellow;
        }
    }


    void OnSuccessDisplay()
    {
        gameWonDisplay.SetActive(true);
        animationManager.OnLevelWon();
        StartCoroutine(ColorChangeWinningMessage("Passed!", textOnWinning));

    }

    public void showAd()
    {
        
        StartCoroutine(ShowAdThenStart());
        gamePlay.scoreCard++;
        foreach (var item in healthBar)
        {
            item.SetActive(true);
        }
        //some function
        gamePlay.scoreCard -= 1;
    }

    IEnumerator ShowAdThenStart()
    {
        adsManager.PlayRewardedAd();
        yield return null;
        gamePlay.ChangeVars();

    }
}
