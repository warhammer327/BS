using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GamePlay gamePlay;
    public PlayfabManager playfabManager;

    public GameObject startManuPanel;
    public GameObject audioPanel;
    public GameObject textPanel;
    public GameObject buttonPanel;
    public GameObject highscorePanel;
    public GameObject levelWonPanel;
    public GameObject OnRecord;
    public GameObject buttonFadePanel;
    public GameObject cluePanel;
    public GameObject keyboardPanel;
    public GameObject hiddenWordPanel;
    public GameObject statusPanel;

    public GameObject loginPanel;
    public TextMeshProUGUI loadingPanelText;

    public GameObject profilePanel;

    private float duration = 0.5f;
    public bool menuAnimSwitch;

    private void Start()
    {
        menuAnimSwitch = false;
        //aboutPanel.SetActive(false);
        buttonFadePanel.SetActive(false);
        LeanTween.moveLocalX(textPanel, 303, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(buttonPanel, 0, duration).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesButton, 900, 1).setEaseInOutCubic();
//        LeanTween.moveLocalX(gamePlay.rulesText, 930, 1).setEaseInOutCubic();
    }
    public void BringLeaderboard()
    {
        if (playfabManager.isInternet)
        {
            buttonFadePanel.SetActive(true);
            //FadeHighScoreButtonText(); 
            LeanTween.moveLocalY(highscorePanel, 0, duration).setEaseInOutCubic();
            LeanTween.moveLocalY(textPanel, 1200, duration).setEaseInOutCubic();
            //LeanTween.moveLocalX(gamePlay.rulesButton, 1200, 1).setEaseInOutCubic();
            //LeanTween.moveLocalX(gamePlay.rulesText, 1230, 1).setEaseInOutCubic();
        }
    }

    public void SlideUpHighScore()
    {
        buttonFadePanel.SetActive(false);
        LeanTween.moveLocalY(highscorePanel, 1200, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(textPanel, 0, duration).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesButton, 900, 1).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesText, 930, 1).setEaseInOutCubic();

    }
    public void BringAudioPanel()
    {
        //gamePlay.rulesButton.SetActive(false);
        buttonFadePanel.SetActive(true);
        //FadeAudioButtonButtonText();
        LeanTween.moveLocalY(audioPanel, 0, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(textPanel, 1200, duration).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesButton, 1200, 1).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesText, 1230, 1).setEaseInOutCubic();
    }

    public void SlideDownAudioPanel()
    {

        buttonFadePanel.SetActive(false);
        LeanTween.moveLocalY(textPanel, 0, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(audioPanel, 1200, duration).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesButton, 900, 1).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesText, 930, 1).setEaseInOutCubic();
    }
    public void BringRulesPanel()
    {
        //gamePlay.rulesButton.SetActive(false);
        //buttonFadePanel.SetActive(true);
        //FadeAudioButtonButtonText();
        LeanTween.moveLocalY(profilePanel, 0, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(textPanel, 1200, duration).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesButton, 1200, 1).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesText, 1230, 1).setEaseInOutCubic();
    }

    public void SlideDownRulesPanel()
    {

        //buttonFadePanel.SetActive(false);
        LeanTween.moveLocalY(textPanel, 0, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(profilePanel, 1200, duration).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesButton, 900, 1).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesText, 930, 1).setEaseInOutCubic();
    }
    public void OnPressStatButton()
    {
        LeanTween.moveLocalX(statusPanel, 0, duration).setEaseInOutCubic();
        LeanTween.moveLocalX(keyboardPanel, 0, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(hiddenWordPanel, 40, duration).setEaseInOutCubic();
        LeanTween.moveLocalX(cluePanel, 0, duration).setEaseInOutCubic();
        LeanTween.moveLocalX(textPanel, 2000, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(buttonPanel, -1085, duration).setEaseInOutCubic();
        //LeanTween.moveLocalY(rulesPanel, 1200, duration).setEaseInOutCubic();
    }

    public void OnGameLost()
    {
        LeanTween.moveLocalX(startManuPanel, 0, duration).setEaseInOutCubic();
    }

    public void OnLevelWon()
    {
        LeanTween.moveLocalY(levelWonPanel, 0, duration).setEaseInOutCubic();
    }

    public void SlideUpLevelWonPanel()
    {
        LeanTween.moveLocalY(levelWonPanel, 2000, duration).setEaseInOutCubic();
    }

    public void SlideDownOnRecord()
    {
        LeanTween.moveLocalY(OnRecord, 0, duration).setEaseInOutCubic();
    }

    

    

    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1.3f);
        //aboutPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        LeanTween.moveLocalX(statusPanel, 2000, duration).setEaseInOutCubic();
        LeanTween.moveLocalX(keyboardPanel, 2000, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(hiddenWordPanel, -1085, duration).setEaseInOutCubic();
        LeanTween.moveLocalX(cluePanel, -2000, duration).setEaseInOutCubic();
        LeanTween.moveLocalX(textPanel, 303, duration).setEaseInOutCubic();
        LeanTween.moveLocalY(buttonPanel, 0, duration).setEaseInOutCubic();
        gamePlay.startMenuGamePanel.SetActive(true);
    }
    
    public void FadeLoadingText()
    {
        loadingPanelText.text = "Logging in";
        StartCoroutine(FadeLoginText(loadingPanelText));
    }

    public void KeepAwayRulesIcon()
    {
        //LeanTween.moveLocalX(gamePlay.rulesButton, 1200, 1).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesText, 1230, 1).setEaseInOutCubic();
    }

    public void BingInRulesIcon()
    {
        //LeanTween.moveLocalX(gamePlay.rulesButton, 900, 1).setEaseInOutCubic();
        //LeanTween.moveLocalX(gamePlay.rulesText, 930, 1).setEaseInOutCubic();
    }
    IEnumerator FadeLoginText(TextMeshProUGUI str)
    {
        int i = 0;
        while (loginPanel.activeSelf == true)
        {
            str.faceColor = new Color32(255, 255, 255, (byte)i);
            yield return null;
            i += 5;
            if (i == 255)
            {
                while (i > 0)
                {
                    str.faceColor = new Color32(255, 255, 255, (byte)i);
                    yield return null;
                    i -= 5;
                }
            }
        }
    }
    
}
