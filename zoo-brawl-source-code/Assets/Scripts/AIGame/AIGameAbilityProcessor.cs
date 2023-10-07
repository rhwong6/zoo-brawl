using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AIGameAbilityProcessor
{
    public void OnPlayAbility(string player, CardAbility cardAbility, int cardZoneNum)
    {
        // If a card has an on play ability
        if (cardAbility.abilityType == CardAbility.AbilityType.onPlay)
        {
            // If the card has an action ability, uses the ActionEffect method
            if (cardAbility.action)
            {
                ActionEffect(player, cardAbility, cardZoneNum);
            }
        }

        // If a card has an on field ability
        if (cardAbility.abilityType == CardAbility.AbilityType.onField)
        {
            // If the card has a modification ability, uses the StatModificationEffect method
            if (cardAbility.statModification)
            {
                StatModificationEffect(player, cardAbility, cardZoneNum, cardAbility.modificationValue);
            }
        }
    }

    public void OnDeath(string player, CardAbility cardAbility, int cardZoneNum)
    {
        // If the card has an on field effect uses the StatModificationEffect method with negative values
        if (cardAbility.abilityType == CardAbility.AbilityType.onField)
        {
            int[] negativeModificationValues = cardAbility.modificationValue.Select(value => -value).ToArray();
            StatModificationEffect(player, cardAbility, cardZoneNum, negativeModificationValues);
        }
    }

    public Tuple<int[,], int[,]> OnAttack(string player, AIGameZoneDisplay card1, AIGameZoneDisplay card2, CardAbility cardAbility)
    {
        // Initialises 2 arrays for modifiers as a tuple (one for modification of attacker, one for modification of defender)
        Tuple<int[,], int[,]> modifiers = Tuple.Create(new int[2, 3], new int[2, 3]);

        // If the card has an on attack ability, sets the modifiers to using the OnAttackEffect method
        if (cardAbility.abilityType == CardAbility.AbilityType.onAttack)
        {
            modifiers = OnAttackEffect(player, card1, card2, cardAbility);
        }

        // returns the modifiers
        return modifiers;
    }

    public void ActionEffect(string player, CardAbility cardAbility, int cardZoneNum)
    {
        // Uses the ActionTarget method which returns the player zone being targetted, the player awake values being targetted, and which zones to target
        Tuple<int[], int[], bool[]> actionTarget = ActionTarget(player, cardZoneNum, cardAbility.actionTarget, cardAbility.actionSpeciesTarget);

        int[] zoneTarget = actionTarget.Item1;
        int[] animalAwakeTarget = actionTarget.Item2;
        bool[] animalTarget = actionTarget.Item3;

        // For each action ability the card has
        foreach (CardAbility.ActionType action in cardAbility.actionType)
        {
            switch (action)
            {
                // If the card has a stun ability, sets the targetted cards awake values to 4 (4 means they are stunned and cannot attack the next turn)
                case CardAbility.ActionType.stun:
                    for (int zone = 0; zone < 7; zone++)
                    {
                        if (animalTarget[zone])
                        {
                            animalAwakeTarget[zone] = 4;
                        }
                    }
                    break;

                // If the card has the ability quickAttack, sets the targetted cards awake value 2 (2 means they can attack this turn)
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
        // Initialises arrays for player zone being targetted, the player awake values being targetted, and which zones to target
        int[] zoneTarget = new int[] { };
        int[] animalAwakeTarget = new int[] { };
        bool[] animalTarget = new bool[7];

        for (int zone = 0; zone < 7; zone++)
        {
            animalTarget[zone] = false;
        }

        switch (actionTarget)
        {
            // If the target of the ability is all allies, sets the zone and awake targets to the players playing the card
            case CardAbility.Target.allAllies:
                if (player == "player")
                {
                    zoneTarget = AIGameState.playerZoneCards;
                    animalAwakeTarget = AIGameState.playerZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = AIGameState.opponentZoneCards;
                    animalAwakeTarget = AIGameState.opponentZoneAnimalAwake;
                }

                // Sets all the targeted zones to true
                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0)
                    {
                        animalTarget[zone] = true;
                    }
                }
                break;

            // If the target of the ability is all enemies, sets the zone and awake targets to the opponent of the player playing the card
            case CardAbility.Target.allEnemies:
                if (player == "player")
                {
                    zoneTarget = AIGameState.opponentZoneCards;
                    animalAwakeTarget = AIGameState.opponentZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = AIGameState.playerZoneCards;
                    animalAwakeTarget = AIGameState.playerZoneAnimalAwake;
                }

                // Sets all the targeted zones to true
                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0)
                    {
                        animalTarget[zone] = true;
                    }
                }
                break;

            // If the target of the ability is self, sets the zone and awake targets to the player playing the card
            case CardAbility.Target.self:
                if (player == "player")
                {
                    zoneTarget = AIGameState.playerZoneCards;
                    animalAwakeTarget = AIGameState.playerZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = AIGameState.opponentZoneCards;
                    animalAwakeTarget = AIGameState.opponentZoneAnimalAwake;
                }

                // Sets the targeted zones to the zone the player played the card
                animalTarget[cardZoneNum] = true;
                break;

            // If the target of the ability is adjacent allies, sets the zone and awake targets to the player playing the card
            case CardAbility.Target.adjacentAllies:
                if (player == "player")
                {
                    zoneTarget = AIGameState.playerZoneCards;
                    animalAwakeTarget = AIGameState.playerZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = AIGameState.opponentZoneCards;
                    animalAwakeTarget = AIGameState.opponentZoneAnimalAwake;
                }

                // Sets the targeted zones to the zone adjacent to where the player played the card
                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0 && (zone == (cardZoneNum - 1) || (zone == (cardZoneNum + 1))))
                    {
                        animalTarget[zone] = true;
                    }
                }
                break;

            // If the target of the ability is the facing enemy, sets the zone and awake targets to the opponent of the player playing the card
            case CardAbility.Target.facingEnemy:
                if (player == "player")
                {
                    zoneTarget = AIGameState.opponentZoneCards;
                    animalAwakeTarget = AIGameState.opponentZoneAnimalAwake;
                }
                else
                {
                    zoneTarget = AIGameState.playerZoneCards;
                    animalAwakeTarget = AIGameState.playerZoneAnimalAwake;
                }

                // Sets the targeted zones to the zone to where the player played the card
                for (int zone = 0; zone < 7; zone++)
                {
                    if (zoneTarget[zone] != 0 && zone == cardZoneNum)
                    {
                        animalTarget[zone] = true;
                    }
                }
                break;
        }

        // Uses the ActionTargetSpecies method to further filter these targetted zones to the animal species the abiity targets
        animalTarget = ActionTargetSpecies(actionSpeciesTarget, zoneTarget, animalTarget);

        // Creates a tuple for the player zone being targetted, the player awake values being targetted, and which zones to target and returns it
        Tuple<int[], int[], bool[]> zoneAndTarget = Tuple.Create(zoneTarget, animalAwakeTarget, animalTarget);
        return zoneAndTarget;
    }

    public bool[] ActionTargetSpecies(CardAbility.SpeciesTarget actionSpeciesTarget, int[] zoneTarget, bool[] animalTarget)
    {
        string animalType = "";

        // Selects which animal type to target depending on the targetted species variable in the card ability
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

        // If the species target is all, does not need to modify the targetted cards
        if (actionSpeciesTarget != CardAbility.SpeciesTarget.all)
        {
            // Removes from the targetted cards boolean array any cards that do not match the species of the species target ability
            for (int zone = 0; zone < 7; zone++)
            {
                if (CardCollection.cardCollection[zoneTarget[zone] - 1].GetCardType() != animalType)
                {
                    animalTarget[zone] = false;
                }
            }
        }

        // Returns the cards to target boolean array
        return animalTarget;
    }

    public void StatModificationEffect(string player, CardAbility cardAbility, int cardZoneNum, int[] modificationValues)
    {
        Tuple<int[,], bool[]> modificationTarget = ModificationTarget(player, cardZoneNum, cardAbility.modificationTarget, cardAbility.modificationSpeciesTarget);

        int[,] modifier = modificationTarget.Item1;
        bool[] zoneTarget = modificationTarget.Item2;


        for (int statModification = 0; statModification < cardAbility.modificationType.Count; statModification++) {
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

    }

    public Tuple<int[,], bool[]> ModificationTarget(string player, int cardZoneNum, CardAbility.Target modificationTarget, CardAbility.SpeciesTarget modificationTargetSpecies)
    {
        // Initialises a 2d array to store the values of the modifiers and an array for which zones to target with the modifiers
        int[,] modifier = new int[7, 3];
        bool[] zoneTarget = new bool[7];

        for (int zone = 0; zone < 7; zone++)
        {
            zoneTarget[zone] = false;
        }

        switch (modificationTarget)
        {
            // If the target of the modifiers are all allies, targets all zones and uses the ModifierSearch method to set the modifier to the players modifiers
            case CardAbility.Target.allAllies:
                for (int zone = 0; zone < 7; zone++)
                {
                    zoneTarget[zone] = true;
                }

                modifier = ModifierSearch(player, modificationTargetSpecies);
                break;

            // If the target of the modifiers are all enemies, targets all zones and uses the ModifierSearch method to set the modifier to the opponents modifiers
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

            // If the target of the modifiers is self, targets the zone where the card was played and uses the ModifierSearch method to set the modifier to the players modifiers
            case CardAbility.Target.self:
                zoneTarget[cardZoneNum] = true;
                modifier = ModifierSearch(player, modificationTargetSpecies);
                break;

            // If the target of the modifiers are adjacent allies, targets adjacent zones to where the card was played and uses the ModifierSearch method to set the modifier to the players modifiers
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

        // Creates a tuple containing which modifier is changed, and an array for which modifiers to target
        Tuple<int[,], bool[]> modifierAndTarget = Tuple.Create(modifier, zoneTarget);
        return modifierAndTarget;
    }

    public int[,] ModifierSearch(string modifierPlayer, CardAbility.SpeciesTarget speciesTarget)
    {
        // Initialises modifier 2d array
        int[,] modifier = new int[,] { };

        // Depending on which players modifier is being modified, and which species the ability targets, sets the modifier to target appropriately
        switch (speciesTarget)
        {
            case CardAbility.SpeciesTarget.all:
                if (modifierPlayer == "player")
                {
                    modifier = AIGameState.playerUniversalModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentUniversalModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.feline:
                if (modifierPlayer == "player")
                {
                    modifier = AIGameState.playerFelineModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentFelineModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.canine:
                if (modifierPlayer == "player")
                {
                    modifier = AIGameState.playerCanineModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentCanineModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.ursidae:
                if (modifierPlayer == "player")
                {
                    modifier = AIGameState.playerUrsidaeModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentUrsidaeModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.reptilia:
                if (modifierPlayer == "player")
                {
                    modifier = AIGameState.playerReptiliaModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentReptiliaModifiers;
                }
                break;

            case CardAbility.SpeciesTarget.delphindae:
                if (modifierPlayer == "player")
                {
                    modifier = AIGameState.playerDelphindaeModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentDelphindaeModifiers;
                }
                break;
        }

        // Returns this modifier to target
        return modifier;
    }

    public Tuple<int[,], int[,]> OnAttackEffect(string player, AIGameZoneDisplay card1, AIGameZoneDisplay card2, CardAbility cardAbility)
    {
        // Initialises an attacker modifier, and defender modifier
        int[,] attackerModifiers = new int[2, 3];
        int[,] defenderModifiers = new int[2, 3];

        // For each attacker modifier ability
        for (int modification = 0; modification < cardAbility.attackModificationType.Count; modification++)
        {
            switch (cardAbility.attackModificationType[modification])
            {
                // If the ability is increase attacker attack, increase the attacker modifier value by the attack modification value in the ablity
                case CardAbility.AttackModificationType.increasedAttackerAttack:
                    attackerModifiers[0, 0] += cardAbility.attackModificationValue[modification];
                    break;

                // If the ability is increase attacker armour, increase the attacker modifier value by the armour modification value in the ablity
                case CardAbility.AttackModificationType.increasedAttackerArmour:
                    attackerModifiers[1, 1] += cardAbility.attackModificationValue[modification];
                    break;

                // If the ability is decrease defender armour, decreases the defender modifier value by the armour modification value in the ablity
                case CardAbility.AttackModificationType.ignoreOpponentArmour:
                    if (card2.cardArmour >= cardAbility.attackModificationValue[modification])
                    {
                        defenderModifiers[1, 1] -= cardAbility.attackModificationValue[modification];
                    }
                    break;
            }
        }

        // Returns the attacker and defender modifiers 2d arrays as a tuple
        return Tuple.Create(attackerModifiers, defenderModifiers);
    }
}
