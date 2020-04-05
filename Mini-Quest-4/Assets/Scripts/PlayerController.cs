using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //------------------------------------------------
    //Amount of cash player should collect to complete level
    public float CashTotal = 1400.0f;

    //Amount of cash for this player
    private float cash = 0.0f;

    //Reference to transform
    private Transform ThisTransform = null;

    //Respawn time in seconds after dying
    public float RespawnTime = 2.0f;

    //Player health
    public int health = 100;

    //Private damage texture
    private Texture2D DamageTexture = null;

    //Screen coordinates
    private Rect ScreenRect;

    //Show damage texture?
    private bool ShowDamage = false;

    //Damage texture interval (amount of time in seconds to show texture)
    private float DamageInterval = 0.2f;

    //Get Mecanim animator component in children
    private Animator AnimComp = null;


    //------------------------------------------------
    //Called when object is created
    void Start()
    {
        //Get cached transform
        ThisTransform = transform;

        //For death animation
        AnimComp = GetComponentInChildren<Animator>();

        //setting up active weapon

    }

    //------------------------------------------------
    //Accessors to set and get cash
    public float Cash
    {
        //Return cash value
        get { return cash; }

        //Set cash and validate, if required
        set
        {
            //Set cash
            cash = value;

            //Check collection limit - post notification if limit reached
            if (cash >= CashTotal)
                GameManager.Notifications.PostNotification(this, "CashCollected");
        }
    }
    //------------------------------------------------
    //Accessors to set and get health
 
    //------------------------------------------------
    //Function to apply damage to the player
    public IEnumerator ApplyDamage(int Amount = 0)
    {
        //Reduce health
        Health -= Amount;

        //Post damage notification
        GameManager.Notifications.PostNotification(this, "PlayerDamaged");

        //Show damage texture
        ShowDamage = true;

        //Wait for interval
        yield return new WaitForSeconds(DamageInterval);

        //Hide damage texture
        ShowDamage = false;
    }

    //------------------------------------------------
    //Function called when player dies
    public IEnumerator Die()
    {
        //Disable input
        GameManager.Instance.InputAllowed = false;

        //Trigger death animation if available
        if (AnimComp) AnimComp.SetTrigger("ShowDeath");

        //Wait for respawn time
        yield return new WaitForSeconds(RespawnTime);

        //Restart level
        SceneManager.LoadScene(0);
    }
    //------------------------------------------------
    void Update()
    {
        //Build screen rect on update (in case screen size changes)
        ScreenRect.x = ScreenRect.y = 0;
        ScreenRect.width = Screen.width;
        ScreenRect.height = Screen.height;

       
    }

}
