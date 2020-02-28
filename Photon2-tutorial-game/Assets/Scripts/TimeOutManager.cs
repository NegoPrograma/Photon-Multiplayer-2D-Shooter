using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TimeOutManager : MonoBehaviourPun
{

    private float idleTime = 0f;
    private float timer = 5f;

    public GameObject timeOutCanvas;
    public Text timeOutText;
    private bool timerHasEnded = false;

    public GameManagerScript gameManager;
    void Start()
    {
        
    }

    void Update()
    {
        if(!timerHasEnded){
            if(Input.anyKey){
                idleTime = 0;
                timer = 5;
                timeOutCanvas.SetActive(false);
            }
            idleTime += Time.deltaTime;

            if(idleTime >= 10){
                    playerIsAFK();
            }
            if(timeOutCanvas.activeSelf){
                timer -= Time.deltaTime;
                timeOutText.text = "Disconnecting in: "+ timer.ToString("F0");
                if(timer <= 0){
                    timerHasEnded = true;
                }
            }
        }
        else {
            //expel player
            gameManager.LeaveRoom();
        }
    }

    public void playerIsAFK(){
        if(!(Application.internetReachability==NetworkReachability.NotReachable))
            timeOutCanvas.SetActive(true);
    }
}
