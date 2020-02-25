using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class ScreenSwitch : MonoBehaviour
{
    public GameObject loginScreen;
    public static bool hasLoggedIn = false;
    public  GameObject lobbyScreen;
    
    void Start(){
        lobbyScreen.SetActive(false);
        loginScreen.SetActive(true);
        /*
        this is on case player has already logged in b4
        and is just returning to lobby.
        */
        if(hasLoggedIn){
            lobbyScreen.SetActive(true);
            loginScreen.SetActive(false);
            PhotonNetwork.NickName = SetNickname.nickname;
        }
    }
    public void toggleScreenState(){
        if(!PhotonNetwork.NickName.Equals("")){
            lobbyScreen.SetActive(true);
            loginScreen.SetActive(false);
            hasLoggedIn = true;
        }
    }

    
}
