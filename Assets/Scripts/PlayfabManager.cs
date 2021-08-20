using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine.Advertisements;
using UnityEditor;
using System.Collections;
using System.Linq;
using UnityEditor.UIElements;
using System.Text.RegularExpressions;

public class PlayfabManager : MonoBehaviour
{
    public AnimationManager animationManager;
    public ToggleGroup togglePayment;
    public ToggleGroup togglePaymentUpdate;
    public GamePlay gamePlay;
    public GameObject loggingPanel;
    public GameObject takeInUsername;
    public GameObject advanceToMenuOffline;

    public TextMeshProUGUI loadingLeaderBoardText;
    public TMP_InputField usernameSubmit;
    public TMP_InputField numberSubmit;
    public TMP_InputField updateNumberSubmit;
    public TextMeshProUGUI enterValidNumberText;
    public TextMeshProUGUI loginText;

    public TextMeshProUGUI firstPos;
    public TextMeshProUGUI firstPosName;
    public TextMeshProUGUI firstPosPoint;

    public TextMeshProUGUI secondPos;
    public TextMeshProUGUI secondPosName;
    public TextMeshProUGUI secondPosPoint;

    public TextMeshProUGUI thirdPos;
    public TextMeshProUGUI thirdPosName;
    public TextMeshProUGUI thirdPosPoint;

    public TextMeshProUGUI fourthPos;
    public TextMeshProUGUI fourthPosName;
    public TextMeshProUGUI fourthPosPoint;

    public TextMeshProUGUI fifthPos;
    public TextMeshProUGUI fifthPosName;
    public TextMeshProUGUI fifthPosPoint;

    public string[] namePlayer = { "waiting", "waiting", "waiting", "waiting", "waiting" };


    public int[] pointPlayer = { -1, -1, -1, -1, -1 };
    public int[] tablePos = { -1, -1, -1, -1, -1 };

    public string[] playfabIDs;

    public bool isInternet;

    private string playfabID;
    public string res;
    public string playerName;

    private void Awake()
    {
        if (CheckForInternetConnection())
        {
            isInternet = true;

        }
        else
        {
            isInternet = false;
        }
        advanceToMenuOffline.SetActive(false);
        loggingPanel.SetActive(true);

    }

    // Start is called before the first frame update
    public void Start()
    {
        animationManager.FadeLoadingText();
        //Debug.Log(isInternet);
        Login();
        //loggingPanel.SetActive(false);
    }

    // Update is called once per frame


    void Login()
    {

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        //loggingPanel.SetActive(false);
        //        Debug.Log("Successful account creation");
        //GetLeaderboardAroundPlayer();

        playfabID = result.PlayFabId;

        playerName = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
        }

        if (playerName == null)
        {
            takeInUsername.SetActive(true);
        }
        else
        {
            loggingPanel.SetActive(false);
        }

    }

    void OnError(PlayFabError error)
    {
        //Debug.Log("Error creation account");
        //Debug.Log(error.GenerateErrorReport());
        string errorReport = error.GenerateErrorReport();
        if (errorReport.Contains("Name not available"))
        {
            //Debug.Log("It happened");
            loginText.text = "Try different username";
        }
        else
        {
            loggingPanel.SetActive(false);
        }

        if (gamePlay.EnterNumberPanel.activeSelf == true)
        {
            gamePlay.enterValidNumerText.text = "Error occured, try again later";
        }
    }

    public void SendToLeaderboard(int score)
    {

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>{
               new StatisticUpdate {
                   StatisticName = "LeaderBoard",
                   Value = score
               }
           }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        //Debug.Log("SEnt to leaderboard");
    }


    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        StartCoroutine(DelayResultTextOnRegistrationSuccess("Congrats for registering", loginText));

    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {

    }

    public void GetLeaderboardAroundPlayer()
    {
        firstPosName.text = "Loading...";
        firstPosPoint.text = pointPlayer[0].ToString();
        secondPosName.text = "Loading...";
        secondPosPoint.text = pointPlayer[0].ToString();
        thirdPosName.text = "Loading...";
        thirdPosPoint.text = pointPlayer[0].ToString();
        fourthPosName.text = "Loading...";
        fourthPosPoint.text = pointPlayer[0].ToString();
        fifthPosName.text = "Loading...";
        fifthPosPoint.text = pointPlayer[0].ToString();
        res = "Try a few moments later for your result";
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "LeaderBoard",
            MaxResultsCount = 5,
            PlayFabId = playfabID
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);

    }

    private void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        if (isInternet)
        {
            int index = 0;
            int curPlayerIndex = 0;
            foreach (var item in result.Leaderboard)
            {
                if (item.PlayFabId == playfabID)
                {
                    curPlayerIndex = index;
                }
                tablePos[index] = item.Position + 1;
                namePlayer[index] = item.DisplayName;
                pointPlayer[index++] = item.StatValue;
            }
            index = 0;
            firstPos.text = tablePos[index].ToString();
            firstPosName.text = namePlayer[index];
            firstPosPoint.text = pointPlayer[index++].ToString();

            secondPos.text = tablePos[index].ToString();
            secondPosName.text = namePlayer[index];
            secondPosPoint.text = pointPlayer[index++].ToString();

            thirdPos.text = tablePos[index].ToString();
            thirdPosName.text = namePlayer[index];
            thirdPosPoint.text = pointPlayer[index++].ToString();

            fourthPos.text = tablePos[index].ToString();
            fourthPosName.text = namePlayer[index];
            fourthPosPoint.text = pointPlayer[index++].ToString();

            fifthPos.text = tablePos[index].ToString();
            fifthPosName.text = namePlayer[index];
            fifthPosPoint.text = pointPlayer[index++].ToString();

            if (curPlayerIndex == 0)
            {
                firstPos.color = Color.yellow;
                firstPosName.color = Color.yellow;
                firstPosPoint.color = Color.yellow;
            }
            else if (curPlayerIndex == 1)
            {

                secondPos.color = Color.yellow;
                secondPosName.color = Color.yellow;
                secondPosPoint.color = Color.yellow;
            }
            else if (curPlayerIndex == 2)
            {

                thirdPos.color = Color.yellow;
                thirdPosName.color = Color.yellow;
                thirdPosPoint.color = Color.yellow;
            }
            else if (curPlayerIndex == 3)
            {

                fifthPos.color = Color.yellow;
                fifthPosName.color = Color.yellow;
                fifthPosPoint.color = Color.yellow;
            }
            else if (curPlayerIndex == 4)
            {

                firstPos.color = Color.yellow;
                firstPosName.color = Color.yellow;
                firstPosPoint.color = Color.yellow;
            }
        }
        else
        {
            //Debug.Log("Offline");
            SSTools.ShowMessage("Not connected to server", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }
    public void GetNumber()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }
    private void OnDataReceived(GetUserDataResult result)
    {
        gamePlay.registerNumberButtonText.text = "Click here to update";
        gamePlay.registerNumberPanelMessage.text = "You have already registered. Now you can change number if you wish.";
    }
    public void SaveUsernameAndNumber()
    {
        string number = numberSubmit.text;
        string method;
        var rxGeneral = new Regex(@"01[3456789][0-9]{8}", RegexOptions.Compiled);
        var rxRocketOnly = new Regex(@"01[3456789][0-9]{9}", RegexOptions.Compiled);

        Toggle toggle = togglePayment.ActiveToggles().FirstOrDefault();
        //Debug.Log(toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);
        bool isNumberGood = false;
        try
        {

            if (toggle.name == "" || toggle.name == null)
            {
                return;
            }
        }
        catch
        {
            loginText.text = "Choose between Bkash, Rocket, Nagad";
            return;
        }



        if (rxRocketOnly.IsMatch(number) && toggle.name == "Rocket" && number.Length == 12)
        {
            //Debug.Log("Rocket Good");
            isNumberGood = true;
        }
        else if (rxGeneral.IsMatch(number) && number.Length == 11)
        {
            //Debug.Log("General Good");
            isNumberGood = true;
        }

        if (usernameSubmit.text == "" || usernameSubmit.text == null)
        {
            loginText.text = "Enter a valid user name";
            return;
        }

        method = toggle.name;
        //Debug.Log(number + " | " + method + " | " +usernameSubmit.text);

        if (isNumberGood)
        {
            loginText.text = "Processing";
            var request1 = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                    {"Phone Number", number},
                    {"Method",method}
                }
            };
            togglePayment.SetAllTogglesOff();
            PlayFabClientAPI.UpdateUserData(request1, OnDataSendFirstTime, OnError);
        }
        else
        {
            gamePlay.enterValidNumerText.text = "";
            StartCoroutine(DelayResultTextOnDataSendFailure("Entered number is not valid", loginText));
            return;
        }



    }

    public void UpdateNumber()
    {
        string number = updateNumberSubmit.text;
        var rxGeneral = new Regex(@"01[3456789][0-9]{8}", RegexOptions.Compiled);
        var rxRocketOnly = new Regex(@"01[3456789][0-9]{9}", RegexOptions.Compiled);
        //Debug.Log(number);
        //Debug.Log(number.Length);
        Toggle toggle = togglePaymentUpdate.ActiveToggles().FirstOrDefault();
        //Debug.Log(toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);
        bool isNumberGood = false;
        try
        {

            if (toggle.name == "" || toggle.name == null)
            {
                return;
            }
        }
        catch
        {
            loginText.text = "Choose between Bkash, Rocket, Nagad";
            return;
        }



        if (rxRocketOnly.IsMatch(number) && toggle.name == "Rocket" && number.Length == 12)
        {
            //Debug.Log("Rocket Good");
            isNumberGood = true;
        }
        else if (rxGeneral.IsMatch(number) && number.Length == 11)
        {
            //Debug.Log("General Good");
            isNumberGood = true;
        }

        if (isNumberGood)
        {
            togglePayment.SetAllTogglesOff();
            //gamePlay.enterValidNumerText.text = "Processing";
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                    {"Phone Number", number},
                    {"Method",toggle.name}
                }
            };
            PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
            StartCoroutine(DelayResultTextOnDataSendSuccess("Number has been updated", gamePlay.enterValidNumerText));
        }
        else
        {
            //Debug.Log("2");
            gamePlay.enterValidNumerText.text = "";
            StartCoroutine(DelayResultTextOnDataSendFailure("Entered number is not valid", gamePlay.enterValidNumerText));
            return;
        }
    }

    private void OnDataSend(UpdateUserDataResult result)
    {


    }

    private void OnDataSendFirstTime(UpdateUserDataResult result)
    {
        if (usernameSubmit.text == "" || usernameSubmit.text == null)
        {
            loginText.text = "Enter a valid user name";
            return;
        }
        else
        {
            loginText.text = "Processing";
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = usernameSubmit.text
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        }

    }

    IEnumerator DelayResultTextOnDataSendSuccess(string str, TextMeshProUGUI textUIElement)
    {
        textUIElement.text = "";
        foreach (char letter in str.ToCharArray())
        {
            textUIElement.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1);
        animationManager.BingInRulesIcon();

        gamePlay.EnterNumberPanel.SetActive(false);

    }
    IEnumerator DelayResultTextOnDataSendFailure(string str, TextMeshProUGUI textUIElement)
    {
        textUIElement.text = "";
        foreach (char letter in str.ToCharArray())
        {
            textUIElement.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator DelayResultTextOnRegistrationSuccess(string str, TextMeshProUGUI textUIElement)
    {
        textUIElement.text = "";
        foreach (char letter in str.ToCharArray())
        {
            textUIElement.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1);
        animationManager.BingInRulesIcon();
        loggingPanel.SetActive(false);

    }

    public bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.playfab.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
