using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AIGameObjectManager : MonoBehaviour
{
    // References each of the player hand and zone card game objects
    public GameObject playerHandCard1;
    public GameObject playerHandCard2;
    public GameObject playerHandCard3;
    public GameObject playerHandCard4;
    public GameObject playerHandCard5;
    public GameObject playerHandCard6;

    public GameObject playerZoneCard1;
    public GameObject playerZoneCard2;
    public GameObject playerZoneCard3;
    public GameObject playerZoneCard4;
    public GameObject playerZoneCard5;
    public GameObject playerZoneCard6;
    public GameObject playerZoneCard7;

    // List for storing these hand and zone card objects as well as displays they will be using
    public static List<GameObject> playerHandCardObjs = new List<GameObject>();
    public static List<GameObject> playerZoneCardObjs = new List<GameObject>();

    public static List<CardDisplay> playerHandCardDisplays = new List<CardDisplay>();
    public static List<AIGameZoneDisplay> playerZoneCardDisplays = new List<AIGameZoneDisplay>();

    // References each of the opponents hand and zone card game objects
    public GameObject opponentHandCard1;
    public GameObject opponentHandCard2;
    public GameObject opponentHandCard3;
    public GameObject opponentHandCard4;
    public GameObject opponentHandCard5;
    public GameObject opponentHandCard6;

    public GameObject opponentZoneCard1;
    public GameObject opponentZoneCard2;
    public GameObject opponentZoneCard3;
    public GameObject opponentZoneCard4;
    public GameObject opponentZoneCard5;
    public GameObject opponentZoneCard6;
    public GameObject opponentZoneCard7;

    // List for storing these hand and zone card objects as well as displays they will be using
    public static List<GameObject> opponentHandCardObjs = new List<GameObject>();
    public static List<GameObject> opponentZoneCardObjs = new List<GameObject>();

    public static List<CardDisplay> opponentHandCardDisplays = new List<CardDisplay>();
    public static List<AIGameZoneDisplay> opponentZoneCardDisplays = new List<AIGameZoneDisplay>();

    // References each of the players zone game objects
    public GameObject playerZone1;
    public GameObject playerZone2;
    public GameObject playerZone3;
    public GameObject playerZone4;
    public GameObject playerZone5;
    public GameObject playerZone6;
    public GameObject playerZone7;

    // References each of the opponent zone game objects
    public GameObject opponentZone1;
    public GameObject opponentZone2;
    public GameObject opponentZone3;
    public GameObject opponentZone4;
    public GameObject opponentZone5;
    public GameObject opponentZone6;
    public GameObject opponentZone7;

    // A list for storing all the player and opponent zone game objects
    public static List<GameObject> playerZones = new List<GameObject>();
    public static List<GameObject> opponentZones = new List<GameObject>();

    // Reference to the player and opponent base object
    public GameObject playerBase;
    public GameObject opponentBase;

    // Static reference of the player and opponent base object
    public static GameObject playerBaseObj;
    public static GameObject opponentBaseObj;

    // References to the turn text game object
    public TextMeshProUGUI turnText;
    public static TextMeshProUGUI turnTextObj;

    // References to the end turn button game object
    public Button endTurnButton;
    public static Button endTurnButtonObj;

    // References to the player base text game object
    public TextMeshProUGUI playerBaseText;
    public TextMeshProUGUI opponentBaseText;

    // References to the player base text game object
    public static TextMeshProUGUI playerBaseTextObj;
    public static TextMeshProUGUI opponentBaseTextObj;

    // References to the player coin amount text game object
    public TextMeshProUGUI playerCoinAmount;
    public static TextMeshProUGUI playerCoinAmountObj;

    // References to the opponent coin amount text game object
    public TextMeshProUGUI opponentCoinAmount;
    public static TextMeshProUGUI opponentCoinAmountObj;

    // References to the log text game object
    public TextMeshProUGUI logText;
    public static TextMeshProUGUI logTextObj;

    // References to the win popup and win popup text game objects
    public GameObject winPopup;
    public TextMeshProUGUI wonPlayerText;
    public static GameObject winPopupObj;
    public static TextMeshProUGUI wonPlayerTextObj;
    
    private void Awake()
    {
        // Adds all zone and hand card game objects to their respective lists
        playerHandCardObjs.Add(playerHandCard1);
        playerHandCardObjs.Add(playerHandCard2);
        playerHandCardObjs.Add(playerHandCard3);
        playerHandCardObjs.Add(playerHandCard4);
        playerHandCardObjs.Add(playerHandCard5);
        playerHandCardObjs.Add(playerHandCard6);

        playerZoneCardObjs.Add(playerZoneCard1);
        playerZoneCardObjs.Add(playerZoneCard2);
        playerZoneCardObjs.Add(playerZoneCard3);
        playerZoneCardObjs.Add(playerZoneCard4);
        playerZoneCardObjs.Add(playerZoneCard5);
        playerZoneCardObjs.Add(playerZoneCard6);
        playerZoneCardObjs.Add(playerZoneCard7);

        opponentHandCardObjs.Add(opponentHandCard1);
        opponentHandCardObjs.Add(opponentHandCard2);
        opponentHandCardObjs.Add(opponentHandCard3);
        opponentHandCardObjs.Add(opponentHandCard4);
        opponentHandCardObjs.Add(opponentHandCard5);
        opponentHandCardObjs.Add(opponentHandCard6);

        opponentZoneCardObjs.Add(opponentZoneCard1);
        opponentZoneCardObjs.Add(opponentZoneCard2);
        opponentZoneCardObjs.Add(opponentZoneCard3);
        opponentZoneCardObjs.Add(opponentZoneCard4);
        opponentZoneCardObjs.Add(opponentZoneCard5);
        opponentZoneCardObjs.Add(opponentZoneCard6);
        opponentZoneCardObjs.Add(opponentZoneCard7);

        // For each hand card, add a card display
        for (int handCard = 0; handCard < 6; handCard++)
        {
            playerHandCardDisplays.Add(playerHandCardObjs[handCard].GetComponent<CardDisplay>());
            opponentHandCardDisplays.Add(opponentHandCardObjs[handCard].GetComponent<CardDisplay>());
        }

        // For each zone card, add a card display
        for (int zoneCard = 0; zoneCard < 7; zoneCard++)
        {
            playerZoneCardDisplays.Add(playerZoneCardObjs[zoneCard].GetComponent<AIGameZoneDisplay>());
            opponentZoneCardDisplays.Add(opponentZoneCardObjs[zoneCard].GetComponent<AIGameZoneDisplay>());
        }

        // Add each player and opponent zones to their respective lists
        playerZones.Add(playerZone1);
        playerZones.Add(playerZone2);
        playerZones.Add(playerZone3);
        playerZones.Add(playerZone4);
        playerZones.Add(playerZone5);
        playerZones.Add(playerZone6);
        playerZones.Add(playerZone7);

        opponentZones.Add(opponentZone1);
        opponentZones.Add(opponentZone2);
        opponentZones.Add(opponentZone3);
        opponentZones.Add(opponentZone4);
        opponentZones.Add(opponentZone5);
        opponentZones.Add(opponentZone6);
        opponentZones.Add(opponentZone7);

        // Below sets all referenced objects to a static version
        turnTextObj = turnText;
        endTurnButtonObj = endTurnButton;

        playerCoinAmountObj = playerCoinAmount;
        opponentCoinAmountObj = opponentCoinAmount;

        playerBaseObj = playerBase;
        opponentBaseObj = opponentBase;

        playerBaseTextObj = playerBaseText;
        opponentBaseTextObj = opponentBaseText;

        logTextObj = logText;

        winPopupObj = winPopup;
        wonPlayerTextObj = wonPlayerText;

    }
}
