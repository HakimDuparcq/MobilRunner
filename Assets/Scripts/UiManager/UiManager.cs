using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public Animator HudAnimator;

    public GameObject HUD;
    public GameObject Menu;
    public GameObject Game;
    public GameObject Settings;
    public GameObject ScoreView;

    [Space(10)]
    public Button StartGameButton;
    public Button SettingsButton;
    public Button SettingsReturn;
    public Button ScoreViewNext;
    public Button HomeInGame;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ListenerUI();
        Setup();
    }

    void Setup()
    {
        HUD.SetActive(true);
        Menu.SetActive(true);
        Game.SetActive(false);
        Settings.SetActive(false);
        ScoreView.SetActive(false);
    }

    public void ListenerUI()
    {
        StartGameButton.onClick.AddListener(() => GameManager.instance.StartGame());
        SettingsButton.onClick.AddListener(()=>ShowSettings(true));
        SettingsReturn.onClick.AddListener(()=>ShowSettings(false));
        ScoreViewNext.onClick.AddListener(() => ShowMenuResetGame());
        HomeInGame.onClick.AddListener(() => ShowMenuResetGame());
    }
    
    public void ShowSettings(bool show)
    {
        Settings.SetActive(show);
    }

    public void ShowMenuResetGame()
    {
        ScoreView.SetActive(false);
        Menu.SetActive(true);
        GameManager.instance.ResetGame();
    }

    public IEnumerator MenuToGame()
    {
        HudAnimator.SetTrigger("MenuToGame");
        yield return new WaitForSeconds(0.3f);
        Menu.SetActive(false);
        Game.SetActive(true);

    }

}
