using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCardCover : MonoBehaviour
{
    // References to the card cover game object
    public GameObject cardCover;

    // References the attribute box game objects
    public GameObject costBox;
    public GameObject attackBox;
    public GameObject armourBox;
    public GameObject healthBox;

    void Start()
    {
        // If the AI cover cards boolean is true, display the card cover and does not display the rest
        if (AIGameState.aiCoverCards)
        {
            cardCover.SetActive(true);
            costBox.SetActive(false);
            attackBox.SetActive(false);
            armourBox.SetActive(false);
            healthBox.SetActive(false);
        }
        else
        {
            // Else displays all the attribute boxes and does not display the card cover
            cardCover.SetActive(false);
            costBox.SetActive(true);
            attackBox.SetActive(true);
            armourBox.SetActive(true);
            healthBox.SetActive(true);
        }
    }

}
