using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public float actualScore;
    public float bestScore;



    [Space(10)]
    public TextMeshProUGUI scoreDisplayMenu;
    public TextMeshProUGUI scoreDisplayInGame;
    public TextMeshProUGUI scoreDisplayScoreView;

    [Space(10)]
    public float score1Star;
    public float score2Star;
    public float score3Star;
    [Space(5)]
    public Sprite emptyStar;
    public Sprite fullStar;
    [Space(5)]
    public Image star1;
    public Image star2;
    public Image star3;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Setup();
    }

    void Update()
    {
        UpdateScore();
    }

    private void Setup()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
    }
    private void UpdateScore()
    {
        if (GameManager.instance.gameState == GameState.InGame)
        {
            actualScore += MapController.instance.speedMap * Time.deltaTime;
            scoreDisplayInGame.text = ((int)(actualScore)).ToString();
        }
        else if (GameManager.instance.gameState == GameState.EndGame)
        {
            scoreDisplayScoreView.text = ((int)(actualScore)).ToString();
            SetupStars();

            if (actualScore > bestScore)
            {
                bestScore = actualScore;
                PlayerPrefs.SetInt("BestScore", (int)bestScore);
            }
            scoreDisplayMenu.text = ((int)(bestScore)).ToString();
        }
    }

    private void SetupStars()
    {
        if (actualScore>score3Star)
        {
            star1.sprite = fullStar;
            star2.sprite = fullStar;
            star3.sprite = fullStar;
        }
        else if (actualScore > score2Star)
        {
            star1.sprite = fullStar;
            star2.sprite = fullStar;
            star3.sprite = emptyStar;
        }
        else if (actualScore > score1Star)
        {
            star1.sprite = fullStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
        }
        else
        {
            star1.sprite = emptyStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
        }

    }
}
