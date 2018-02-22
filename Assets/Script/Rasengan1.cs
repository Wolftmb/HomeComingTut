using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Completed
{
	public class Rasengan1 : MonoBehaviour
	{
        public AudioClip fire;

        // Use this for initialization
        void Start()
        {
            SoundManager.instance.PlaySingle(fire);
			Destroy (gameObject, 2);
        }
	}
}
