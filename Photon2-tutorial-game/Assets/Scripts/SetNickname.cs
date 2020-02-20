using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class SetNickname : MonoBehaviour
{
    public static string nickname;
    [SerializeField] private Text nicknameInputFieldTextComponent;
    void Start() 
    {
        nickname="";       
    }

    public void OnButtonClick(){
        nickname= nicknameInputFieldTextComponent.text;
        PhotonNetwork.NickName = nickname;
    }

}
