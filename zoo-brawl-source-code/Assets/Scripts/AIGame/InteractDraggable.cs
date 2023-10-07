using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InteractDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // References the line game object and initialises a lineRenderer
    public GameObject line;
    LineRenderer lineRenderer;

    // Stores if the card is the player or opponents
    public string player;

    // Initialises to store the zones of the card starting the interaction and ending the interaction
    private int startCard;
    private int endCard;

    // Initialises a vector for the mouse position and a raycase hit that returns the object the mouse is over
    Vector3 mousePos;
    RaycastHit2D hit;

    // Initialises events that is raised when a card battle takes place, or if a player chooses to attack the base
    public static event Action<int, int> CardsBattle;
    public static event Action<string, int> AttackBase;

    void Start()
    {
        // Sets the lineRenderer to the lineRenderer component in the line object
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // If the player selects a player card on the players turn
        if (player == "player" && AIGameState.turn == "player")
        {
            // Sets the card starting the interaction to the index of the card gameobject on the zone
            startCard = AIGameObjectManager.playerZones.IndexOf(gameObject);

            // If the card selected exists, and if the card can attack (as its awake value is 2)
            if (AIGameState.playerZoneCards[startCard] != 0 && AIGameState.playerZoneAnimalAwake[startCard] == 2)
            {
                // Displays the line and sets the linerenderers starting position to the position of the card
                line.SetActive(true);

                Vector2 cardPosition = transform.position;
                lineRenderer.SetPosition(0, cardPosition);
            }
        }
        else if (player == "opponent" && AIGameState.turn == "opponent")
        {
            startCard = AIGameObjectManager.opponentZones.IndexOf(gameObject);

            if (AIGameState.opponentZoneCards[startCard] != 0 && AIGameState.opponentZoneAnimalAwake[startCard] == 2)
            {
                line.SetActive(true);

                Vector2 cardPosition = transform.position;
                lineRenderer.SetPosition(0, cardPosition);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Sets the mouse position to where the mouse is int he world
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Sets the ending point of the linerenderer to the mouse position
        lineRenderer.SetPosition(1, mousePos);
        hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // If the players hits an opponents card object or base object
        if (player == "player" && hit.collider != null && (AIGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject) != -1 || hit.collider.gameObject == AIGameObjectManager.opponentBaseObj))
        {
            // If the card object the player hit is a card that is on the zone
            if (hit.collider.gameObject == AIGameObjectManager.opponentBaseObj || AIGameState.opponentZoneCards[AIGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject)] != 0)
            {
                // Sets the colour of the linerenderer to green
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }
        } 
        else if (player == "opponent" && hit.collider != null && (AIGameObjectManager.playerZones.IndexOf(hit.collider.gameObject) != -1 || hit.collider.gameObject == AIGameObjectManager.playerBaseObj))
        {
            if (hit.collider.gameObject == AIGameObjectManager.playerBaseObj || AIGameState.playerZoneCards[AIGameObjectManager.playerZones.IndexOf(hit.collider.gameObject)] != 0)
            {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }
        }
        else
        {
            // Else, sets the colour of the linerenderer to red
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Disables the line display
        line.SetActive(false);

        if (hit.collider != null)
        {
            // If the player is attacking the opponents base on their turn
            if (player == "player" && AIGameState.turn == "player" && hit.collider.gameObject == AIGameObjectManager.opponentBaseObj)
            {
                // If the players card starting the attack has an awake value of 2
                if (AIGameState.playerZoneAnimalAwake[startCard] == 2)
                {
                    if (AttackBase != null)
                    {
                        // Raises the AttackBase event
                        AttackBase(player, startCard);
                    }
                }
            }
            else if (player == "opponent" && AIGameState.turn == "opponent" && hit.collider.gameObject == AIGameObjectManager.playerBaseObj)
            {
                if (AIGameState.opponentZoneAnimalAwake[startCard] == 2)
                {
                    if (AttackBase != null)
                    {
                        AttackBase(player, startCard);
                    }
                }
            }
            // If the player is attacking a valid opponents card on their turn
            else if (player == "player" && AIGameState.turn == "player" && AIGameState.playerZoneCards[startCard] != 0 && AIGameState.opponentZoneCards[AIGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject)] != 0 && AIGameState.playerZoneAnimalAwake[startCard] == 2)
            {
                endCard = AIGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject);

                if (CardsBattle != null)
                {
                    // Raises the CardsBattle event
                    CardsBattle(startCard, endCard);
                }
            }
            else if (player == "opponent" && AIGameState.turn == "opponent" && AIGameState.opponentZoneCards[startCard] != 0 && AIGameState.playerZoneCards[AIGameObjectManager.playerZones.IndexOf(hit.collider.gameObject)] != 0 && AIGameState.opponentZoneAnimalAwake[startCard] == 2)
            {
                endCard = AIGameObjectManager.playerZones.IndexOf(hit.collider.gameObject);

                if (CardsBattle != null)
                {
                    CardsBattle(startCard, endCard);
                }
            }

        }
    }
}
