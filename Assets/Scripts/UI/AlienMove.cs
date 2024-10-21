using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMove : MonoBehaviour
{
    public WhacAnAlien game;
    public Pillar Pillar;
    public bool isHide;
    public Transform Top;
    public Transform Bottom;
    public bool shouldMoveUp;
    //public bool shouldMove;
    public float speed;
    public float t;
    void Start()
    {
        if (isHide) {
            transform.position = Bottom.position;
            //Pillar.transform.position = Pillar.Bottom.position;
        }
        else {
            transform.position = Top.position;
            //Pillar.transform .position = Pillar.Top.position;
        }
        //shouldMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (shouldMove) {
            if(shouldMoveUp){
                t += Time.deltaTime;
                t = t > 1 ? 1 : t;
            }else{
                t -= Time.deltaTime;
                t = t < 0 ? 0 : t;
            }
            transform.position = Vector3.Lerp(Bottom.position, Top.position, t * speed);
            Pillar.transform.position = Vector3.Lerp(Pillar.Bottom.position, Pillar.Top.position, t * speed);
        //}
    }

    public void OnClick(){
        if(!game.Started){
            game.Started = true;
        }
        shouldMoveUp = false;
    }
}
