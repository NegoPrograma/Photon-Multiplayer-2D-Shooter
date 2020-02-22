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


    void Awake()
    {
        bulletSpeed = 800f;
        bulletLifeTime = 5f;
        bulletDamage = 10;
        object[] dir = photonView.InstantiationData;
        bulletDirection = (Vector2) dir[0];
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
            if(!collidedPlayer.playerView.IsMine){
                Debug.Log("Triggered!");
                //bulletView.RPC("DestroyBullet",RpcTarget.All);
                DestroyBullet();
            }
        }
    }

    public void moveBullet(){
        rigid.AddForce(bulletDirection*bulletSpeed*Time.deltaTime,ForceMode2D.Force);
    }
}