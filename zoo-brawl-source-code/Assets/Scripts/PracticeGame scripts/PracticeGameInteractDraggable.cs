using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PracticeGameInteractDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject line;
    LineRenderer lineRenderer;

    public string player;

    private int startCard;
    private int endCard;

    Vector3 mousePos;
    RaycastHit2D hit;

    public static event Action<int, int> CardsBattle;
    public static event Action<string, int> AttackBase;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (player == "player" && PracticeGameState.turn == "player")
        {
            startCard = PracticeGameObjectManager.playerZones.IndexOf(gameObject);

            if (PracticeGameState.playerZoneCards[startCard] != 0 && PracticeGameState.playerZoneAnimalAwake[startCard] == 2)
            {
                line.SetActive(true);

                Vector2 cardPosition = transform.position;
                lineRenderer.SetPosition(0, cardPosition);
            }
        }
        else if (player == "opponent" && PracticeGameState.turn == "opponent")
        {
            startCard = PracticeGameObjectManager.opponentZones.IndexOf(gameObject);

            if (PracticeGameState.opponentZoneCards[startCard] != 0 && PracticeGameState.opponentZoneAnimalAwake[startCard] == 2)
            {
                line.SetActive(true);

                Vector2 cardPosition = transform.position;
                lineRenderer.SetPosition(0, cardPosition);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        /*
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.SetPosition(1, mousePosition);
        */

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        lineRenderer.SetPosition(1, mousePos);
        hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (player == "player" && hit.collider != null && (PracticeGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject) != -1 || hit.collider.gameObject == PracticeGameObjectManager.opponentBaseObj))
        {
            if (hit.collider.gameObject == PracticeGameObjectManager.opponentBaseObj || PracticeGameState.opponentZoneCards[PracticeGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject)] != 0)
            {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }
        }
        else if (player == "opponent" && hit.collider != null && (PracticeGameObjectManager.playerZones.IndexOf(hit.collider.gameObject) != -1 || hit.collider.gameObject == PracticeGameObjectManager.playerBaseObj))
        {
            if (hit.collider.gameObject == PracticeGameObjectManager.playerBaseObj || PracticeGameState.playerZoneCards[PracticeGameObjectManager.playerZones.IndexOf(hit.collider.gameObject)] != 0)
            {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }
        }
        else
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.SetActive(false);

        if (hit.collider != null)
        {
            if (player == "player" && PracticeGameState.turn == "player" && hit.collider.gameObject == PracticeGameObjectManager.opponentBaseObj)
            {
                if (PracticeGameState.playerZoneAnimalAwake[startCard] == 2)
                {
                    if (AttackBase != null)
                    {
                        AttackBase(player, startCard);
                    }
                }
            }
            else if (player == "opponent" && PracticeGameState.turn == "opponent" && hit.collider.gameObject == PracticeGameObjectManager.playerBaseObj)
            {
                if (PracticeGameState.opponentZoneAnimalAwake[startCard] == 2)
                {
                    if (AttackBase != null)
                    {
                        AttackBase(player, startCard);
                    }
                }
            }
            else if (player == "player" && PracticeGameState.turn == "player" && PracticeGameState.playerZoneCards[startCard] != 0 && PracticeGameState.opponentZoneCards[PracticeGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject)] != 0 && PracticeGameState.playerZoneAnimalAwake[startCard] == 2)
            {
                endCard = PracticeGameObjectManager.opponentZones.IndexOf(hit.collider.gameObject);

                if (CardsBattle != null)
                {
                    CardsBattle(startCard, endCard);
                }
            }
            else if (player == "opponent" && PracticeGameState.turn == "opponent" && PracticeGameState.opponentZoneCards[startCard] != 0 && PracticeGameState.playerZoneCards[PracticeGameObjectManager.playerZones.IndexOf(hit.collider.gameObject)] != 0 && PracticeGameState.opponentZoneAnimalAwake[startCard] == 2)
            {
                endCard = PracticeGameObjectManager.playerZones.IndexOf(hit.collider.gameObject);

                if (CardsBattle != null)
                {
                    CardsBattle(startCard, endCard);
                }
            }

        }
    }
}
