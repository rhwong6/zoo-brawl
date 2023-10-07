using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class PracticePlayerDropZone : MonoBehaviour, IDropHandler
{
    public GameObject playerZone1;
    public GameObject playerZone2;
    public GameObject playerZone3;
    public GameObject playerZone4;
    public GameObject playerZone5;
    public GameObject playerZone6;
    public GameObject playerZone7;

    public GameObject outerZone;

    List<GameObject> playerZones = new List<GameObject>();

    // Change these if using test game (need to rework)
    List<Card> handCards = PracticeGameState.playerHandCards;
    int[] zoneCards = PracticeGameState.playerZoneCards;
    int[] zoneAnimalAwake = PracticeGameState.playerZoneAnimalAwake;

    public delegate void CardAddedToZoneHandler(int cardID, int cardZoneNum);
    public event CardAddedToZoneHandler CardAddedToZone;

    private void Awake()
    {
        playerZones.Add(playerZone1);
        playerZones.Add(playerZone2);
        playerZones.Add(playerZone3);
        playerZones.Add(playerZone4);
        playerZones.Add(playerZone5);
        playerZones.Add(playerZone6);
        playerZones.Add(playerZone7);
    }

    public void OnDrop(PointerEventData eventData)
    {   
        if (PracticeGameState.turn == "player")
        {
            PracticeHandDraggable hd = eventData.pointerDrag.GetComponent<PracticeHandDraggable>();
            if (hd != null)
            {
                int cardID = handCards[hd.handPos].GetCardID();

                GameObject dropZone = eventData.pointerEnter;
                int dropZoneIndex = playerZones.IndexOf(dropZone);

                if (dropZone != outerZone && zoneCards[dropZoneIndex] == 0 && PracticeGameState.playerCoins >= CardCollection.cardCollection[cardID - 1].GetCardCost())
                {
                    //Debug.Log(CardCollection.cardCollection[cardID - 1].getCardName() + " DROPPED ONTO: " + dropZoneIndex);

                    handCards.RemoveAt(hd.handPos);
                    zoneCards[dropZoneIndex] = cardID;
                    zoneAnimalAwake[dropZoneIndex] = 1;

                    if (CardAddedToZone != null)
                    {
                        //Debug.Log("CARDADDEDTOZONE IS RAISED");
                        StartCoroutine(UpdateHandAndZone(cardID, dropZoneIndex));
                    }
                }
            }
        }
    }

    IEnumerator UpdateHandAndZone(int cardID, int cardZoneNum)
    {
        yield return new WaitForSeconds(0.001f);
        CardAddedToZone(cardID, cardZoneNum);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}