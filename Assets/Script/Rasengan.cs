using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
namespace Completed
{
    public class Rasengan : MonoBehaviour
    {
        public float speed = 10;
        public Rigidbody2D Rasengan1;
        public Transform player;
        public float fireRate = 2;
        public GameObject Player;
        private float curTimeout;

        public Texture2D icon;

        void Start()
        {
        }
        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), icon))
            {
                Fire();
            }
        }

        void SetRotation()
        {
            transform.position = Player.transform.position;
        }
        private void Fire()
        {
                curTimeout += Time.deltaTime;
                if (curTimeout > fireRate)
                {
                    curTimeout = 0;
                    Rigidbody2D clone = Instantiate(Rasengan1, player.position, Quaternion.identity) as Rigidbody2D;
                    clone.velocity = transform.TransformDirection(player.up * speed);
                    clone.transform.up = player.up;
                }
        }
    }
}