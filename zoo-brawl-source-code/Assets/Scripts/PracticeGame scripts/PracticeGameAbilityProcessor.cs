using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PracticeGameAbilityProcessor
{

    public void OnPlayAbility(string player, CardAbility cardAbility, int cardZoneNum)
    {
        if (cardAbility.abilityType == CardAbility.AbilityType.onPlay)
        {
            if (cardAbility.action)
            {
                ActionEffect(player, cardAbility, cardZoneNum);
            }
        }

        if (cardAbility.abilityType == CardAbility.AbilityType.onField)
        {
            if (cardAbility.statModification)
            {
                StatModificationEffect(player, cardAbility, cardZoneNum, cardAbility.modificationValue);
            }
        }
    }

    public void OnDeath(string player, CardAbility cardAbility, int cardZoneNum)
    {
        if (cardAbility.abilityType == CardAbility.AbilityType.onField)
        {
            int[] negativeModificationValues = cardAbility.modificationValue.Select(value => -value).ToArray();
            StatModificationEffect(player, cardAbility, cardZoneNum, negativeModificationValues);
        }
    }

    public int[,] OnAttack(string player, PracticeGameZoneDisplay card1, PracticeGameZoneDisplay card2, CardAbility cardAbility)
    {
        int[,] modifiers = new int[2, 3];

        if (cardAbility.abilityType == CardAbility.AbilityType.onAttack)
        {
            modifiers = OnAttackEffect(player, card1, card2, cardAbility);
        }

        return modifiers;
    }

    public void ActionEffect(string player, CardAbility cardAbility, int cardZoneNum)
    {
        Tuple<int[], int[], bool[]> actionTarget = ActionTarget(player, cardZoneNum, cardAbility.actionTarget, cardAbility.actionSpeciesTarget);

        int[] zoneTarget = actionTarget.Item1;
        int[] animalAwakeTarget = actionTarget.Item2;
        bool[] animalTarget = actionTarget.Item3;

        //Debug.Log("ANIMALTARGET: " + string.Join(", ", animalTarget));

        foreach (CardAbility.ActionType action in cardAbility.actionType)
        {
            switch (action)
            {
                case CardAbility.ActionType.stun:
                    for (int zone = 0; zone < 7; zone++)
                    {
                        if (animalTarget[zone])
                        {
                            animalAwakeTarget[zone] = 4;
                        }
                    }
                    break;

                case CardAbility.ActionType.quickAttack:
                    for (int zone = 0; zone < 7; zone++)
                    {
                        if (animalTarget[zone])
                        {
                            animalAwakeTarget[zone] = 2;
                        }
                    }
                    break;

            }
        }
    }

    public Tuple<int[], int[], bool[]> ActionTarget(string player, int cardZoneNum, CardAbility.Target actionTarget, CardAbility.SpeciesTarget actionSpeciesTarget)
    {
        int[] zoneTarget = new int[] { };
        int[] animalAwakeTarget = new int[] { };
        bool[] animalTarget = new bool[7];

        for (int zone = 0; zone < 7; zone++)
        {
            animalTarget[zone] = false;
        }

        switch (actionTarget)
        {
            case CardAbility.Target.allAllies:
                if (player == "player")
                {
                    zoneTarget = PracticeGameState.playerZoneCards;
                    animalAwakeTarget = PracticeGameState.playerZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = PracticeGameState.opponentZoneCards;
                    animalAwakeTarget = PracticeGameState.opponentZoneAnimalAwake;
                }

                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0)
                    {
                        animalTarget[zone] = true;
                    }
                }
                break;

            case CardAbility.Target.allEnemies:
                if (player == "player")
                {
                    zoneTarget = PracticeGameState.opponentZoneCards;
                    animalAwakeTarget = PracticeGameState.opponentZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = PracticeGameState.playerZoneCards;
                    animalAwakeTarget = PracticeGameState.playerZoneAnimalAwake;
                }

                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0)
                    {
                        animalTarget[zone] = true;
                    }
                }
                break;

            case CardAbility.Target.self:
                if (player == "player")
                {
                    zoneTarget = PracticeGameState.playerZoneCards;
                    animalAwakeTarget = PracticeGameState.playerZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = PracticeGameState.opponentZoneCards;
                    animalAwakeTarget = PracticeGameState.opponentZoneAnimalAwake;
                }

                animalTarget[cardZoneNum] = true;
                break;

            case CardAbility.Target.adjacentAllies:
                if (player == "player")
                {
                    zoneTarget = PracticeGameState.playerZoneCards;
                    animalAwakeTarget = PracticeGameState.playerZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = PracticeGameState.opponentZoneCards;
                    animalAwakeTarget = PracticeGameState.opponentZoneAnimalAwake;
                }

                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0 && (zone == (cardZoneNum - 1) || (zone == (cardZoneNum + 1))))
                    {
                        animalTarget[zone] = true;
                    }
                }
                break;

            case CardAbility.Target.facingEnemy:
                if (player == "player")
                {
                    zoneTarget = PracticeGameState.opponentZoneCards;
                    animalAwakeTarget = PracticeGameState.opponentZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = PracticeGameState.playerZoneCards;
                    animalAwakeTarget = PracticeGameState.playerZoneAnimalAwake;
                }

                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0 && zone == cardZoneNum)
                    {
                        animalTarget[zone] = true;
                    }
                }

                break;
        }

        animalTarget = ActionTargetSpecies(actionSpeciesTarget, zoneTarget, animalTarget);

        Tuple<int[], int[], bool[]> zoneAndTarget = Tuple.Create(zoneTarget, animalAwakeTarget, animalTarget);

        return zoneAndTarget;
    }

    public bool[] ActionTargetSpecies(CardAbility.SpeciesTarget actionSpeciesTarget, int[] zoneTarget, bool[] animalTarget)
    {
        string animalType = "";

        switch (actionSpeciesTarget)
        {
            case CardAbility.SpeciesTarget.feline:
                animalType = "Feline";
                break;

            case CardAbility.SpeciesTarget.canine:
                animalType = "Canine";
                break;

            case CardAbility.SpeciesTarget.ursidae:
                animalType = "Ursidae";
                break;

            case CardAbility.SpeciesTarget.reptilia:
                animalType = "Reptilia";
                break;

            case CardAbility.SpeciesTarget.delphindae:
                animalType = "Delphindae";
                break;
        }

        if (actionSpeciesTarget != CardAbility.SpeciesTarget.all)
        {
            for (int zone = 0; zone < 7; zone++)
            {
                if (CardCollection.cardCollection[zoneTarget[zone] - 1].GetCardType() != animalType)
                {
                    animalTarget[zone] = false;
                }
            }
        }

        return animalTarget;
    }

    public void StatModificationEffect(string player, CardAbility cardAbility, int cardZoneNum, int[] modificationValues)
    {
        Tuple<int[,], bool[]> modificationTarget = ModificationTarget(player, cardZoneNum, cardAbility.modificationTarget, cardAbility.modificationSpeciesTarget);

        int[,] modifier = modificationTarget.Item1;
        bool[] zoneTarget = modificationTarget.Item2;

        //Debug.Log("ZONETARGET: " + string.Join(", ", zoneTarget));

        for (int statModification = 0; statModification < cardAbility.modificationType.Count; statModification++)
        {
            switch (cardAbility.modificationType[statModification])
            {
                case CardAbility.ModificationType.modifyAttack:
                    for (int zone = 0; zone < 7; zone++)
                    {
                        if (zoneTarget[zone])
                        {
                            modifier[zone, 0] += modificationValues[statModification];
                        }
                    }
                    break;

                case CardAbility.ModificationType.modifyArmour:
                    for (int zone = 0; zone < 7; zone++)
                    {
                        if (zoneTarget[zone])
                        {
                            modifier[zone, 1] += modificationValues[statModification];
                        }
                    }
                    break;

                case CardAbility.ModificationType.modifyHealth:
                    for (int zone = 0; zone < 7; zone++)
                    {
                        if (zoneTarget[zone])
                        {
                            modifier[zone, 2] += modificationValues[statModification];
                        }
                    }
                    break;
            }
        }

        /*
        string debugString = "";
        for (int i = 0; i < 7; i++)
        {
            debugString += "[";
            for (int j = 0; j < 3; j++)
            {
                debugString += PracticeGameState.playerDelphindaeModifiers[i, j];
                debugString += ", ";
            }
            debugString += "]";
        }
        Debug.Log(debugString);
        */
    }

    public Tuple<int[,], bool[]> ModificationTarget(string player, int cardZoneNum, CardAbility.Target modificationTarget, CardAbility.SpeciesTarget modificationTargetSpecies)
    {
        int[,] modifier = new int[7, 3];
        bool[] zoneTarget = new bool[7];

        for (int zone = 0; zone < 7; zone++)
        {
            zoneTarget[zone] = false;
        }

        switch (modificationTarget)
        {
            case CardAbility.Target.allAllies:
                for (int zone = 0; zone < 7; zone++)
                {
                    zoneTarget[zone] = true;
                }

                modifier = ModifierSearch(player, modificationTargetSpecies);
                break;

            case CardAbility.Target.allEnemies:
                for (int zone = 0; zone < 7; zone++)
                {
                    zoneTarget[zone] = true;
                }

                if (player == "player")
                {
                    modifier = ModifierSearch("opponent", modificationTargetSpecies);
                }
                else
                {
                    modifier = ModifierSearch("player", modificationTargetSpecies);
                }
                break;

            case CardAbility.Target.self:
                zoneTarget[cardZoneNum] = true;
                modifier = ModifierSearch(player, modificationTargetSpecies);
                break;

            case CardAbility.Target.adjacentAllies:
                for (int zone = 0; zone < 7; zone++)
                {
                    if (zone == cardZoneNum - 1 || zone == cardZoneNum + 1)
                    {
                        zoneTarget[zone] = true;
                    }
                    modifier = ModifierSearch(player, modificationTargetSpecies);
                }
                break;
        }

        Tuple<int[,], bool[]> modifierAndTarget = Tuple.Create(modifier, zoneTarget);

        return modifierAndTarget;
    }

    public int[,] ModifierSearch(string modifierPlayer, CardAbility.SpeciesTarget speciesTarget)
    {
        int[,] modifier = new int[,] { };

        switch (speciesTarget)
        {
            case CardAbility.SpeciesTarget.all:
                if (modifierPlayer == "player")
                {
                    modifier = PracticeGameState.playerUniversalModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentUniversalModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.feline:
                if (modifierPlayer == "player")
                {
                    modifier = PracticeGameState.playerFelineModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentFelineModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.canine:
                if (modifierPlayer == "player")
                {
                    modifier = PracticeGameState.playerCanineModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentCanineModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.ursidae:
                if (modifierPlayer == "player")
                {
                    modifier = PracticeGameState.playerUrsidaeModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentUrsidaeModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.reptilia:
                if (modifierPlayer == "player")
                {
                    modifier = PracticeGameState.playerReptiliaModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentReptiliaModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.delphindae:
                if (modifierPlayer == "player")
                {
                    modifier = PracticeGameState.playerDelphindaeModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentDelphindaeModifiers;
                }
                break;
        }


        return modifier;
    }

    public int[,] OnAttackEffect(string player, PracticeGameZoneDisplay card1, PracticeGameZoneDisplay card2, CardAbility cardAbility)
    {
        int[,] modifiers = new int[2, 3];

        for (int modification = 0; modification < cardAbility.attackModificationType.Count; modification++)
        {

            switch (cardAbility.attackModificationType[modification])
            {
                case CardAbility.AttackModificationType.increasedAttackerAttack:
                    modifiers[0, 0] += cardAbility.attackModificationValue[modification];
                    break;

                case CardAbility.AttackModificationType.ignoreOpponentArmour:
                    if (card2.cardArmour >= cardAbility.attackModificationValue[modification])
                    {
                        modifiers[1, 1] -= cardAbility.attackModificationValue[modification];
                    }
                    break;
            }
        }

        return modifiers;
    }
}
