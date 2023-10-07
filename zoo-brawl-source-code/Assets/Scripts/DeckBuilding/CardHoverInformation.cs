using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CardHoverInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // References the game object that shows the fact box and the object that shows the fact text
    public GameObject factBox;
    public TextMeshProUGUI factText;

    // References the card display which contains the information of the cards in the collection
    public CardDisplay cardDisplay;

    // When the pointer is over the card, sets the fact text to the animals fact and set the card box to active
    public void OnPointerEnter(PointerEventData eventData)
    {
        factText.text = CardCollection.cardFacts[cardDisplay.id - 1];
        factBox.SetActive(true);
    }

    // When the pointer leaves the card, sets card boxes active state to false
    public void OnPointerExit(PointerEventData eventData)
    {
        factBox.SetActive(false);
    }
}
