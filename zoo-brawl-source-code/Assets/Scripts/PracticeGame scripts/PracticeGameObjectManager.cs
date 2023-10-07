using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PracticeGameObjectManager : MonoBehaviour
{
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

    public static List<GameObject> playerHandCardObjs = new List<GameObject>();
    public static List<GameObject> playerZoneCardObjs = new List<GameObject>();

    public static List<CardDisplay> playerHandCardDisplays = new List<CardDisplay>();
    public static List<PracticeGameZoneDisplay> playerZoneCardDisplays = new List<PracticeGameZoneDisplay>();

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

    public static List<GameObject> opponentHandCardObjs = new List<GameObject>();
    public static List<GameObject> opponentZoneCardObjs = new List<GameObject>();

    public static List<CardDisplay> opponentHandCardDisplays = new List<CardDisplay>();
    public static List<PracticeGameZoneDisplay> opponentZoneCardDisplays = new List<PracticeGameZoneDisplay>();

    public GameObject playerZone1;
    public GameObject playerZone2;
    public GameObject playerZone3;
    public GameObject playerZone4;
    public GameObject playerZone5;
    public GameObject playerZone6;
    public GameObject playerZone7;

    public GameObject opponentZone1;
    public GameObject opponentZone2;
    public GameObject opponentZone3;
    public GameObject opponentZone4;
    public GameObject opponentZone5;
    public GameObject opponentZone6;
    public GameObject opponentZone7;

    public static List<GameObject> playerZones = new List<GameObject>();
    public static List<GameObject> opponentZones = new List<GameObject>();

    public GameObject playerBase;
    public GameObject opponentBase;

    public static GameObject playerBaseObj;
    public static GameObject opponentBaseObj;

    public static bool playerInteract = false;

    private void Awake()
    {
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

        for (int handCard = 0; handCard < 6; handCard++)
        {
            playerHandCardDisplays.Add(playerHandCardObjs[handCard].GetComponent<CardDisplay>());
            opponentHandCardDisplays.Add(opponentHandCardObjs[handCard].GetComponent<CardDisplay>());
        }

        for (int zoneCard = 0; zoneCard < 7; zoneCard++)
        {
            playerZoneCardDisplays.Add(playerZoneCardObjs[zoneCard].GetComponent<PracticeGameZoneDisplay>());
            opponentZoneCardDisplays.Add(opponentZoneCardObjs[zoneCard].GetComponent<PracticeGameZoneDisplay>());
        }

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

        playerBaseObj = playerBase;
        opponentBaseObj = opponentBase;

        /*
        Debug.Log("PLAYERZONES COUNT: " + playerZones.Count);
        Debug.Log("OPPONENTZONES COUNT: " + opponentZones.Count);
        */
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
