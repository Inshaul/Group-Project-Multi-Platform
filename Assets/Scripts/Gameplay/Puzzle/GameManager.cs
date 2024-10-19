using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Alien> aliens;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI; 
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    private float startingTime = 15f;
    private float timeRemaining;
    private HashSet<Alien> currentAliens = new HashSet<Alien>();
    private int score;
    private bool playing = false;

    public void StartGame() {
        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        gameUI.SetActive(true);
        for (int i = 0; i < aliens.Count; i++) {
            aliens[i].Hide();
            aliens[i].SetIndex(i);
        }
        currentAliens.Clear();
        timeRemaining = startingTime;
        score = 0;
        scoreText.text = "0";
        playing = true;
    }

    public void GameOver() {
        outOfTimeText.SetActive(true);
        foreach (Alien alien in aliens) {
            alien.StopGame();
        }
        playing = false;
        playButton.SetActive(true);
    }

    void Update() {
        if (playing) {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0) {
                timeRemaining = 0;
                GameOver();
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            if (currentAliens.Count <= (score / 10)) {
                int index = Random.Range(0, aliens.Count);
                if (!currentAliens.Contains(aliens[index])) {
                    currentAliens.Add(aliens[index]);
                    aliens[index].Activate();
                }
            }
        }
    }

    public void AddScore(int alienIndex) {
        score += 1;
        scoreText.text = $"{score}";
        timeRemaining += 1;
        currentAliens.Remove(aliens[alienIndex]);
    }

    public void Missed(int alienIndex) {
        timeRemaining -= 2;
        currentAliens.Remove(aliens[alienIndex]);
    }
}
