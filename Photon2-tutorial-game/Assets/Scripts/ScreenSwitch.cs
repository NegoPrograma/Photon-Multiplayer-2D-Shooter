using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSwitch : MonoBehaviour
{
    [SerializeField]
    private  GameObject loginScreen;

    [SerializeField]
    private  GameObject lobbyScreen;
    void Start(){
        lobbyScreen.SetActive(false);
        loginScreen.SetActive(true);
    }
    public void toggleScreenState(){
        if(!SetNickname.nickname.Equals("")){
            lobbyScreen.SetActive(true);
            loginScreen.SetActive(false);
        }
    }
}
