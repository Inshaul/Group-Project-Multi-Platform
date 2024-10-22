using Gameplay.Interactions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WhacAnAlien : Interactable
{
    public PlayerInput PlayerInput;
    public List<AlienMove> Aliens;
    public GameObject Door;
    public TextMeshProUGUI CountDownText, KeyPromptText;
    [Range(0, 20)]
    public float CountDown;
    public float timer;
    public bool Started;
    // Start is called before the first frame update
    void Start()
    {
        Started = false;
        timer = CountDown;
    }

    // Update is called once per frame
    void Update() {
        if(Started){
            timer -= Time.deltaTime;
            if (timer < 0) {
                //timeover
                foreach (var i in Aliens) {
                    i.shouldMoveUp = true;
                }

                timer = CountDown;
                Started = false;
                Door.SetActive(true);
                KeyPromptText.enabled = false;
            }
            else {
                //counting
                Door.SetActive(false);
                CountDownText.text = "Reset Time: " + ((int)timer).ToString();
            }
        }
    }
    public override void Interact() {
        if(!Started){
            KeyPromptText.enabled = true;
            foreach (var i in Aliens) {
                i.shouldMoveUp = true;
            }
        }
        PlayerInput.SwitchCurrentActionMap("UI");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
   
}
