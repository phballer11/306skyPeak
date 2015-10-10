﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collision : MonoBehaviour {
	private Rigidbody rb;
	private Animation anim;
	private bool collision;
	public Text healthText;
	private int life;
	private FlyMovement flyMovement;
	private Vector3 moveBackPosition;
	private float time;
	public Canvas gameOver;
    public Image achievement_Pressure, achievement_Wipeout;
    private bool ispause;
	private float deadTime;
	private bool isSaved;

    public AudioClip gameoverSound;
    public AudioClip hitSound;
    private AudioSource source;
    private int count = 0;


    void Start()
    {

        Debug.Log("Collision script added to: " + gameObject.name);

		collision = false;
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animation> ();
		flyMovement = GetComponent<FlyMovement> ();
		life = 3;
		gameOver.enabled = false;
        achievement_Pressure.enabled = false;
        achievement_Wipeout.enabled = false;
        ispause = false;
		isSaved = false;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "life:" + life;
        if (flyMovement.enabled == false)
        {

            if (time > 5)
            {
                flyMovement.enabled = true;
                Debug.Log("enabled");
                time = 0;

            }
            else
            {
                Debug.Log("moveBackPosition" + moveBackPosition);
                Debug.Log("current:" + transform.position);
                pushBack();
            }
            time = time + (Time.deltaTime) * 1;
        }

        if (ispause)
        {
            Time.timeScale = 0f;
        }

        if (life < 1)
        {

            if (!isSaved)
            {
                deadTime = time;
                isSaved = true;
            }
            Debug.Log(time - deadTime);

            if (time - deadTime > 3)
            {

                //	Application.LoadLevel(Application.loadedLevel);

            }
            if (count == 0)
            {
                gameOver.enabled = true;
                count = count + 1;
                source.clip = gameoverSound;
                source.Play();

                //If time is less than 10 seconds give the wipeout 
                if (time <= 10)
                {
                    achievement_Wipeout.enabled = true;
                }
            }

        }

    }


    //collision detection
    void OnCollisionEnter(UnityEngine.Collision collision)
    {

		if (!collision.gameObject.CompareTag ("Mail box")) {
			//check the life
			if (life > 0) {
				life--;
				source.clip = hitSound;
				source.Play ();
				//  using (System.IO.StreamWriter file =
				//   new System.IO.StreamWriter(@"C:\Users\Public\skypeak_log.txt", true))
				//{
				//  file.WriteLine("Expected outcome: life " + (life + 1).ToString() + " -> " + "collision" + "-->" + life.ToString() + " at time " + System.DateTime.Now.ToString("h:mm:ss tt"));
				//file.WriteLine("assert: life " + (life + 1).ToString() + " -> " + "collision" + "-->" + life.ToString() + " at time " + System.DateTime.Now.ToString("h:mm:ss tt"));
				//}

			}

			if (life < 1) {
				flyMovement.enabled = false;
				rb.useGravity = true;
				anim.Play ("falling");
				//if (count == 0)
				//{
				//  count = count + 1;
				//source.clip = gameoverSound;
				//source.Play();
				//Debug.Log("game over");
				//}
				gameOver.enabled = true;

			}
		}


    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(100);

    }

    void constrain()
    {
        if (transform.position.y <= 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            //remove rigid body force
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            anim.Play("hitTheFloor");

        }
    }
    //help method for pushing back the owl
    void pushBack()
    {
        transform.position = transform.position - transform.forward * Time.deltaTime;
    }
    //help method for comparing two Vector3 objects.
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.01;
    }
}
