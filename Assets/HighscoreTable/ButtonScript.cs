using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    public TMP_InputField userScore;
    public TMP_InputField userName;
    public GameObject highScoreTable;
    public GameObject okButton;
    public GameObject openLeaderBoardButton;

    public void OpenLB()
    {
        userScore.gameObject.SetActive(false);
        userName.gameObject.SetActive(false);
        okButton.gameObject.SetActive(false);
        highScoreTable.gameObject.SetActive(true);
    }


    public void OpenLeaderBoard()
    {
        openLeaderBoardButton.gameObject.SetActive(false);
        highScoreTable.gameObject.SetActive(true);
    }

    public void openLBButton()
    {
        openLeaderBoardButton.gameObject.SetActive(true);
    }

}
