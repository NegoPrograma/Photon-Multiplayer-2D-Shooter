using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerController : MonoBehaviourPunCallbacks
{

    public float playerSpeed = 45f;
    public PhotonView playerView;

    private Rigidbody2D rigid;

    public bool isAlive;
    public Vector2 playerAim;
    public GameObject bulletSprite;
    public GameObject bulletSpawn;

    public Image playerHealthHeader;
    public float playerMaxHealth;
    public float playerCurrentHealth;
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        playerView = gameObject.GetComponent<PhotonView>();
        playerMaxHealth = 100f;
        playerCurrentHealth = playerMaxHealth;
        isAlive = true;
    }

    void Update()
    {
        if(playerView.IsMine){
            PlayerMove();
            PlayerRotation();
            if(Input.GetKeyDown(KeyCode.E)){
               ShootBullet();
            //    playerView.RPC("ShootBulletRPC",RpcTarget.All);
            }
        }
    }

    public void PlayerMove(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rigid.velocity = new Vector2(x,y)*playerSpeed;
    }

    public void PlayerRotation(){
        Vector3 mousePos = Input.mousePosition;

        mousePos = Camera.main.WorldToScreenPoint(mousePos);

        playerAim = new Vector2(
            mousePos.x-transform.position.x,
            mousePos.y-transform.position.y
        );

        transform.up = playerAim;
    }

    public void ShootBullet(){
        PhotonNetwork.Instantiate("MyBullet",bulletSpawn.transform.position,bulletSpawn.transform.rotation);
    }


    [PunRPC]
    public void ShootBulletRPC(){
        Instantiate(this.bulletSprite,this.bulletSpawn.transform.position,this.bulletSpawn.transform.rotation);
    }


    public void takeDamage(float value,Player shotOwner){
        playerView.RPC("takeDamageNetwork",RpcTarget.AllBuffered,value,shotOwner);
    }


    [PunRPC]
    void takeDamageNetwork(float damage,Player shotOwner){
        HealthUpdate(damage);
        object playerPastScore;
        shotOwner.CustomProperties.TryGetValue("Score",out playerPastScore);
        int actualScore =(int) playerPastScore;
        actualScore += 10;

        Hashtable tempCustomProprierties = new Hashtable();
        tempCustomProprierties.Add("Score",actualScore);
        /*
        Lembra que no joinRoom a gente seta umas proprierades?
        ao chamar novamente o setCustomProperties o que acontece não é uma total substituição
        e sim que o método verifica a hashtable que vc deu e se tiver uma chave igual a hashtable anterior
        ele apenas atualiza o valor, sem interferir nos valores que possuem chaves diferentes.
        
        */
        shotOwner.SetCustomProperties(tempCustomProprierties,null,null);
            if(playerCurrentHealth <= 0 && isAlive){
                playerView.RPC("isGameOver",RpcTarget.All);
            }

        //settando score pela Photon.Pun.UtilityScripts:
        shotOwner.AddScore(10);            
        
    }

    [PunRPC]
    void isGameOver(){
        //o owner significa que esse objeto foi instanciado a partir do seu client
                Debug.Log("GameOver");
                isAlive = false;
                //exibindo pontuação
                foreach(var player in PhotonNetwork.PlayerList){
                    object scoreObject;
                    player.CustomProperties.TryGetValue("Score", out scoreObject);
                    Debug.Log("Player name: "+ player.NickName + "\nPlayer score: " + scoreObject.ToString() + "\nPlayer score via Photon:"+ player.GetScore().ToString());
                }
    }


    public void HealthUpdate(float damage){
        this.playerCurrentHealth +=damage;
        this.playerHealthHeader.fillAmount = this.playerCurrentHealth/100;
    }
}
