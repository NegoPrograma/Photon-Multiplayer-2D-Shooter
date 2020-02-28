using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BulletController : MonoBehaviourPun
{

    public Rigidbody2D rigid;
    public float bulletSpeed;
    public float bulletLifeTime;
    public float bulletTimeCount;
    public float bulletDamage;
    public Vector2 bulletDirection;
    public PhotonView bulletView;
    public string bulletOwner;

    public GameManagerScript gameManager;
    void Awake()
    {
        bulletSpeed = 1200f;
        bulletLifeTime = 5f;
        bulletDamage = 10;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        object[] data = photonView.InstantiationData;
        bulletDirection = (Vector2) data[0];
        bulletOwner =(string) data[1];
        //bulletView.RPC("moveBullet",RpcTarget.AllBuffered);
    }

    void Start(){
        moveBullet();
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletTimeCount > bulletLifeTime){
            //this.bulletView.RPC("DestroyBullet",RpcTarget.All);
            DestroyBullet();
        }
        bulletTimeCount += Time.deltaTime;            
    }
    //[PunRPC]
    public void DestroyBullet(){
            PhotonNetwork.Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision){
        //check if collided with a player prefab that isn't yours
        PlayerController collidedPlayer = collision.gameObject.GetComponent<PlayerController>();
        if(collidedPlayer != null){
            if(!collidedPlayer.playerView.IsMine && bulletView.Owner != collidedPlayer.playerView.Owner){
                float playerHP = collidedPlayer.playerCurrentHealth-10;    
                string playerName = collidedPlayer.playerName.text;
                collidedPlayer.playerView.RPC("TakeDamage",RpcTarget.AllBuffered,bulletDamage);
                collidedPlayer.GetComponent<HurtEffect>().GotHit();
                if(playerHP == 0){
                    gameManager.photonView.RPC("PlayerGotKilledBy",RpcTarget.All,playerName,bulletOwner);
                    Debug.Log("yeah yeah its coming here!");
                }
                DestroyBullet();
            }
        }
    }

    public void moveBullet(){
        rigid.AddForce(bulletDirection*bulletSpeed*Time.deltaTime,ForceMode2D.Force);
    }
}