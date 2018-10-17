using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private Text healthText;
    private Player playerHealth;

    void Start ()
	{
	    healthText = GetComponent<Text>();
	    playerHealth = FindObjectOfType<Player>();
    }
	
	void Update ()
	{
	    healthText.text = ("health: ") + playerHealth.GetHealth().ToString();
    }
}
