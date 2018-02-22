using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Completed
{
    public class Enemy : MovingObject
	{
		public int playerDamage;
        public int hp = 40;
        public AudioClip attackSound1;
		public AudioClip attackSound2;

        private Animator animator;
		private Transform target;
		private int skipMove;

		protected override void Start ()
		{
			GameManager.instance.AddEnemyToList (this);
			animator = GetComponent<Animator> ();
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			base.Start ();
		}
        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            if (skipMove == 1)
            {
                skipMove = 0;
                return;
            }

            base.AttemptMove <T> (xDir, yDir);
            skipMove = Random.Range(0, 2);
        }
		private void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "FireFire") {
				hp = 0;
				other.gameObject.SetActive (false);
				Destroy (gameObject);
			}
		}
        public void MoveEnemy()
        {

            if (hp > 0)
            {
                int xDir = 0;
                int yDir = 0;

                if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
                {
                    yDir = target.position.y > transform.position.y ? 1 : -1;
                }
                else
                {
                    xDir = target.position.x > transform.position.x ? 1 : -1;
                }
                AttemptMove<Player>(xDir, yDir);
            }
        }

        protected override void OnCantMove <T> (T component)
		{
			Player hitPlayer = component as Player;
			hitPlayer.LoseFood (playerDamage);
			animator.SetTrigger ("enemyAttack");

			SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
		}
        public void TakeDamage(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                gameObject.SetActive (false);
            }
        }
        protected override void EnemyAttack<T>(T component)
        {
        }
    }



}
