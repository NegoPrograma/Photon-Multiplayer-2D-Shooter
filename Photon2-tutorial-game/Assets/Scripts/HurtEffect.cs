using UnityEngine;
using System.Collections;
using String = System.String;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class HurtEffect : MonoBehaviourPun,IOnEventCallback
{
    public SpriteRenderer playerSprite;

    public enum EventCodes{
        COLOR_CHANGE = 0
    }

    private void OnEnable(){
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable(){
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    public void OnEvent(EventData photonData){
        byte eventCode = photonData.Code;
        object content = photonData.CustomData;
        if(eventCode == ((byte)EventCodes.COLOR_CHANGE)){
            object[] dataReceived = content as object[];
            if(dataReceived.Length == 4){
                if((int)dataReceived[0] == base.photonView.ViewID){
                    playerSprite.color = new Color((float)dataReceived[1],(float)dataReceived[2],(float)dataReceived[3]);

                }
            }
        }
    }

    private void ChangeColor_RED(){
        float r =1f;
        float g =0f;
        float b =0f;

        object[] dataToSend = new object[]{base.photonView.ViewID,r,g,b};
        RaiseEventOptions options = new RaiseEventOptions(){
            //Essas opções dizem que todos receberão o evento, incluindo quem mandou
            //e que o evento não vai ser guardado pra quem veio depois.
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        SendOptions sendOptions = new SendOptions(){
            Reliability = true,
        };
        PhotonNetwork.RaiseEvent((byte)EventCodes.COLOR_CHANGE,dataToSend,options,sendOptions);
    
    }

    private void ChangeColor_WHITE(){
        float r =1f;
        float g =1f;
        float b =1f;

        object[] dataToSend = new object[]{base.photonView.ViewID,r,g,b};
        RaiseEventOptions options = new RaiseEventOptions(){
            //Essas opções dizem que todos receberão o evento, incluindo quem mandou
            //e que o evento não vai ser guardado pra quem veio depois.
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        SendOptions sendOptions = new SendOptions(){
            Reliability = true,
        };
        PhotonNetwork.RaiseEvent((byte)EventCodes.COLOR_CHANGE,dataToSend,options,sendOptions);
    
    }



    public void GotHit(){
        ChangeColor_RED();

        StartCoroutine("ChangeColorOverTime");
    }

    public void ResetToWhite(){
        ChangeColor_WHITE();
    }

    IEnumerator ChangeColorOverTime(){
        yield return new WaitForSeconds(0.2f);
        ChangeColor_WHITE();
    }


}
