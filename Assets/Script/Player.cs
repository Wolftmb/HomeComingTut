﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Completed
{
	public class Player : MovingObject
	{
		public float restartLevelDelay = 1f;
		public int pointsPerFood = 10;
		public int pointsPerSoda = 20;
        public int wallDamage = 20;
        public int damage = 20;
        public Text foodText;
		public AudioClip moveSound1;
		public AudioClip moveSound2;
		public AudioClip eatSound1;
		public AudioClip eatSound2;
		public AudioClip drinkSound1;
		public AudioClip drinkSound2;
		public AudioClip gameOverSound;

        private Animator animator;
		private int food;
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;
#endif
		

		protected override void Start ()
		{

            animator = GetComponent<Animator>();
			food = GameManager.instance.playerFoodPoints;
			foodText.text = "Еда: " + food;

			base.Start ();
		}
		
		private void OnDisable ()
		{
			GameManager.instance.playerFoodPoints = food;
		}
		
		
		private void Update ()
		{
            if (!GameManager.instance.playersTurn) return;
			
			int horizontal = 0;
			int vertical = 0;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
			
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			
			vertical = (int) (Input.GetAxisRaw ("Vertical"));

			if(horizontal != 0)
			{
				vertical = 0;
			}
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            if (Input.touchCount > 0)
			{
				Touch myTouch = Input.touches[0];
				
				if (myTouch.phase == TouchPhase.Began)
				{
					touchOrigin = myTouch.position;
				}
				
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					Vector2 touchEnd = myTouch.position;
					float x = touchEnd.x - touchOrigin.x;
					float y = touchEnd.y - touchOrigin.y;

					touchOrigin.x = -1;

					if (Mathf.Abs(x) > Mathf.Abs(y))
						horizontal = x > 0 ? 1 : -1;
					else
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif
			if(horizontal != 0 || vertical != 0)
			{
				AttemptMove<Wall> (horizontal, vertical);
			}
		}

        protected override void AttemptMove <T> (int xDir, int yDir)
		{
			food--;
			foodText.text = "Еда: " + food;

			base.AttemptMove <T> (xDir, yDir);
			
			RaycastHit2D hit;
			
			CheckIfGameOver ();

			GameManager.instance.playersTurn = false;
		}
		
		protected override void OnCantMove <T> (T component)
		{
			Wall hitWall = component as Wall;
			hitWall.DamageWall (wallDamage);
			animator.SetTrigger ("playerChop");
		}

        protected override void EnemyAttack<T>(T component)
		{
			Enemy baddie = component as Enemy;
			animator.SetTrigger ("playerChop");
			baddie.TakeDamage (damage);
		}

        private void OnTriggerEnter2D (Collider2D other)
		{
			if(other.tag == "Exit")
			{
				Invoke ("Restart", restartLevelDelay);
				
				enabled = false;
			}
			
			else if(other.tag == "Food")
			{
				food += pointsPerFood;

				foodText.text = "+" + pointsPerFood + " Еда: " + food;
				
				SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);

				other.gameObject.SetActive (false);
			}

			else if(other.tag == "Soda")
			{
				food += pointsPerSoda;

				foodText.text = "+" + pointsPerSoda + " Еда: " + food;

				SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
				
				other.gameObject.SetActive (false);
			}
		}
      
        private void Restart ()
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}
		

		public void LoseFood (int loss)
		{
			animator.SetTrigger ("playerHit");
			food -= loss;
			foodText.text = "-"+ loss + " Еда: " + food;
			CheckIfGameOver ();
		}
		
		private void CheckIfGameOver ()
		{
			if (food <= 0) 
			{
				SoundManager.instance.PlaySingle (gameOverSound);
				
				SoundManager.instance.musicSource.Stop();
				
				GameManager.instance.GameOver ();
			}
		}

    }
}
