using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ScreenSwitch : MonoBehaviour
{
    public GameObject loginScreen;

    public  GameObject lobbyScreen;
    void Start(){
        lobbyScreen.SetActive(false);
        loginScreen.SetActive(true);
    }
    public void toggleScreenState(){
        if(!PhotonNetwork.NickName.Equals("")){
            lobbyScreen.SetActive(true);
            loginScreen.SetActive(false);
        }
    }
}
