using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanManager : MonoBehaviour
{
    public static int score = 0;
    public static int hightScore = 0;
    public static int lives = 100;
    public static int dashes = 6;
    public static int boostAbilityTime = 10;
    public static bool isDie = false;
    public static bool isWin = false;

    public List<Ghost> ghosts = new List<Ghost>();

    public static Abilities abilities;

    Vector3 pos;

    [Header("Voice")]
    AudioSource source;
    [SerializeField]AudioClip eatDot;    
    [SerializeField]AudioClip powerCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dot")
        {
            other.gameObject.GetComponent<Animator>().SetBool("isDestroy", true);
            
            //Play sound
            source.PlayOneShot(eatDot);
        }

        if (other.gameObject.tag == "Power Pallet")
        { 
            other.gameObject.GetComponent<Animator>().SetBool("isDestroy", true);
            foreach (Ghost g in ghosts)
            {
                g.isActive = false;
            }

            //Play sound
            source.PlayOneShot(powerCollect);
        }

        if(other.gameObject.tag == "SpeedBoost")
        {
            abilities.SpeedBoost();
            other.gameObject.GetComponent<Animator>().SetBool("isDestroy", true);
            source.PlayOneShot(powerCollect);
        }
    }
    float t = 0;
    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag == "Ghost")
        {

            if (collision.gameObject.GetComponent<Ghost>().isActive == false)
            {
                collision.gameObject.SetActive(false);
                score++;
            }
            else
            {
                if (t >= 0.008f)
                {
                    lives--;
                    t = 0;
                }
                t  += Time.deltaTime;
            }     
        }
    }


    private void Start()
    {
        source = GetComponent<AudioSource>();
        abilities = GetComponent<Abilities>();

        pos = transform.position;
        score = 0;
        lives = 100;
        isDie = true;
        hightScore = PlayerPrefs.GetInt("HightScore");
    }

    private void Update()
    {
        if (score >= hightScore)
        {
            hightScore = score;
            PlayerPrefs.SetInt("HightScore", hightScore);
        }

        if(lives <= 0) 
        {
            isDie = true;
        }

        if (isDie || isWin)
        {
            GetComponent<Movement>().enabled = false;
            transform.position = pos;

        }
        else
        {
            GetComponent<Movement>().enabled = true;
        }
    }

    public void SetActiveGhosts()
    {
        foreach (Ghost g in ghosts)
        {
            g.gameObject.SetActive(true);
        }
    }
}
