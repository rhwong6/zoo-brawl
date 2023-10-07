using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class PracticeOpponentDropZone : MonoBehaviour, IDropHandler
{
    public GameObject opponentZone1;
    public GameObject opponentZone2;
    public GameObject opponentZone3;
    public GameObject opponentZone4;
    public GameObject opponentZone5;
    public GameObject opponentZone6;
    public GameObject opponentZone7;

    public GameObject opponentOuterZone;

    List<GameObject> opponentZones = new List<GameObject>();

    // Change these if using test game (need to rework)
    List<Card> handCards = PracticeGameState.opponentHandCards;
    int[] zoneCards = PracticeGameState.opponentZoneCards;
    int[] zoneAnimalAwake = PracticeGameState.opponentZoneAnimalAwake;

    public delegate void CardAddedToOpponentZoneHandler(int cardID, int cardZoneNum);
    public event CardAddedToOpponentZoneHandler CardAddedToOpponentZone;

    private void Awake()
    {
        opponentZones.Add(opponentZone1);
        opponentZones.Add(opponentZone2);
        opponentZones.Add(opponentZone3);
        opponentZones.Add(opponentZone4);
        opponentZones.Add(opponentZone5);
        opponentZones.Add(opponentZone6);
        opponentZones.Add(opponentZone7);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (PracticeGameState.turn == "opponent")
        {
            PracticeHandDraggable hd = eventData.pointerDrag.GetComponent<PracticeHandDraggable>();
            if (hd != null)
            {
                int cardID = handCards[hd.handPos].GetCardID();

                GameObject dropZone = eventData.pointerEnter;
                int dropZoneIndex = opponentZones.IndexOf(dropZone);

                if (dropZone != opponentOuterZone && zoneCards[dropZoneIndex] == 0 && PracticeGameState.opponentCoins >= CardCollection.cardCollection[cardID - 1].GetCardCost())
                {

                    //Debug.Log(CardCollection.cardCollection[cardID - 1].getCardName() + " DROPPED ONTO: " + dropZoneIndex);

                    handCards.RemoveAt(hd.handPos);
                    zoneCards[dropZoneIndex] = cardID;
                    zoneAnimalAwake[dropZoneIndex] = 1;

                    if (CardAddedToOpponentZone != null)
                    {
                        //Debug.Log("CARDADDEDTOZONE IS RAISED");
                        StartCoroutine(UpdateOpponentHandAndZone(cardID, dropZoneIndex));
                    }
                }
            }
        }
    }

    IEnumerator UpdateOpponentHandAndZone(int cardID, int cardZoneNum)
    {
        yield return new WaitForSeconds(0.001f);
        CardAddedToOpponentZone(cardID, cardZoneNum);
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
