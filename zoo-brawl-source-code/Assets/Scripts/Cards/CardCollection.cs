using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    // Static lists that store which cards are in the collection, the cards abilities, and card facts
    public static List<Card> cardCollection = new List<Card>();
    public static List<CardAbility> cardAbility = new List<CardAbility>();
    public static List<string> cardFacts = new List<string>();

    // Boolean that stores if the cards have been added
    private static bool cardsAdded = false;

    void Awake() 
    {
        if (cardsAdded == false)
        {
            // Adds all cards with their attributes to the collection
            cardCollection.Add(new Card(1, "Lion", "Intimidating Roar: Opponent animals on their zone cannot attack next turn", 8, 6, 1, 6, "Feline", "Lion"));
            cardCollection.Add(new Card(2, "Wolf", "Pack Tactics: Your canines have +1 attack and +1 armour while this animal is on the field", 3, 3, 1, 3, "Canine", "Wolf"));
            cardCollection.Add(new Card(3, "Grizzly Bear", "Towering Presence: All opponents animal have -1 attack while this card is on the field", 8, 7, 2, 7, "Ursidae", "Grizzly Bear"));
            cardCollection.Add(new Card(4, "Pit Viper", "Venomous Strike: When attacking an animal, deal an additional 2 damage", 2, 3, 0, 2, "Reptilia", "Pit Viper"));
            cardCollection.Add(new Card(5, "Killer Whale", "Razor teeth: When attacking, ignore 2 armour from the apponent", 4, 4, 1, 5, "Delphindae", "Killer Whale"));

            cardCollection.Add(new Card(6, "Cheetah", "Lightning speed: Can attack the turn this was played", 4, 3, 0, 6, "Feline", "Cheetah"));
            cardCollection.Add(new Card(7, "Coyote", "Hunting Team: Adjacent animals +1 attack while this card is on the field", 2, 3, 0, 3, "Canine", "Coyote"));
            cardCollection.Add(new Card(8, "Polar Bear", "Thick fur: When attacking, reduce the amount of damage taken by 1", 6, 4, 2, 6, "Ursidae", "Polar Bear"));
            cardCollection.Add(new Card(9, "Komodo Dragon", "Opportunistic Strike: When attacking an animal, deal an additional 2 damage and take 2 less damage", 7, 5, 2, 6, "Reptilia", "Komodo Dragon"));
            cardCollection.Add(new Card(10, "Spinner Dolphin", "Blinding Splash: On play, stun the animal opposite this animal", 2, 2, 0, 3, "Delphindae", "Spinner Dolphin"));

            cardCollection.Add(new Card(11, "Leopard", "Stealthy Pounce: When attacking, reduce the amount of damage taken by 2", 5, 3, 0, 6, "Feline", "Leopard"));
            cardCollection.Add(new Card(12, "African Wild Dog", "Group Support: Adjacent animals have +1 armour while this card is on the field", 2, 3, 0, 2, "Canine", "African Wild Dog"));
            cardCollection.Add(new Card(13, "Black Bear", "Surprise Attack: Can attack the turn this was played", 5, 4, 2, 6, "Ursidae", "Black Bear"));
            cardCollection.Add(new Card(14, "Crocodile", "Group Nesting: Your Reptilia have +1 armour while this card is on the field", 7, 6, 2, 5, "Reptilia", "Crocodile"));
            cardCollection.Add(new Card(15, "Common Dolphin", "Echolocation Accuracy: All delphindae have +2 attack while this animal is on the field", 2, 2, 0, 2, "Delphindae", "Common Dolphin"));

            // Adds all card abilities to the card ability list
            cardAbility.Add(new CardAbility("Intimidating Roar", CardAbility.AbilityType.onPlay,
                                            true, CardAbility.Target.allEnemies, CardAbility.SpeciesTarget.all, new List<CardAbility.ActionType> { CardAbility.ActionType.stun }, new int[] { }));

            cardAbility.Add(new CardAbility("Pack Tactics", CardAbility.AbilityType.onField,
                                            true, CardAbility.Target.allAllies, CardAbility.SpeciesTarget.canine, new List<CardAbility.ModificationType> { CardAbility.ModificationType.modifyAttack, CardAbility.ModificationType.modifyArmour }, new int[] { 1, 1 }));

            cardAbility.Add(new CardAbility("Towering Presence", CardAbility.AbilityType.onField,
                                            true, CardAbility.Target.allEnemies, CardAbility.SpeciesTarget.all, new List<CardAbility.ModificationType> { CardAbility.ModificationType.modifyAttack }, new int[] { -1 }));

            cardAbility.Add(new CardAbility("Venomous Strike", CardAbility.AbilityType.onAttack,
                                            true, new List<CardAbility.AttackModificationType> { CardAbility.AttackModificationType.increasedAttackerAttack }, new int[] { 2 }));

            cardAbility.Add(new CardAbility("Razor Teeth", CardAbility.AbilityType.onAttack,
                                            true, new List<CardAbility.AttackModificationType> { CardAbility.AttackModificationType.ignoreOpponentArmour }, new int[] { 2 }));

            cardAbility.Add(new CardAbility("Lightning speed", CardAbility.AbilityType.onPlay,
                                            true, CardAbility.Target.self, CardAbility.SpeciesTarget.all, new List<CardAbility.ActionType> { CardAbility.ActionType.quickAttack }, new int[] { }));

            cardAbility.Add(new CardAbility("Hunting Team", CardAbility.AbilityType.onField,
                                            true, CardAbility.Target.adjacentAllies, CardAbility.SpeciesTarget.all, new List<CardAbility.ModificationType> { CardAbility.ModificationType.modifyAttack }, new int[] { 1 }));

            cardAbility.Add(new CardAbility("Thick Fur", CardAbility.AbilityType.onAttack,
                                            true, new List<CardAbility.AttackModificationType> { CardAbility.AttackModificationType.increasedAttackerArmour }, new int[] { 1 }));

            cardAbility.Add(new CardAbility("Opportunistic Strike", CardAbility.AbilityType.onAttack,
                                            true, new List<CardAbility.AttackModificationType> { CardAbility.AttackModificationType.increasedAttackerAttack, CardAbility.AttackModificationType.increasedAttackerArmour }, new int[] { 2, 2 }));

            cardAbility.Add(new CardAbility("Blinding Splash", CardAbility.AbilityType.onPlay,
                                            true, CardAbility.Target.facingEnemy, CardAbility.SpeciesTarget.all, new List<CardAbility.ActionType> { CardAbility.ActionType.stun }, new int[] { }));

            cardAbility.Add(new CardAbility("Stealthy Pouncy", CardAbility.AbilityType.onAttack,
                                            true, new List<CardAbility.AttackModificationType> { CardAbility.AttackModificationType.increasedAttackerArmour }, new int[] { 2 }));

            cardAbility.Add(new CardAbility("Group Support", CardAbility.AbilityType.onField,
                                            true, CardAbility.Target.adjacentAllies, CardAbility.SpeciesTarget.all, new List<CardAbility.ModificationType> { CardAbility.ModificationType.modifyArmour }, new int[] { 1 }));

            cardAbility.Add(new CardAbility("Surprise Attack", CardAbility.AbilityType.onPlay,
                                            true, CardAbility.Target.self, CardAbility.SpeciesTarget.all, new List<CardAbility.ActionType> { CardAbility.ActionType.quickAttack }, new int[] { }));

            cardAbility.Add(new CardAbility("Group Nesting", CardAbility.AbilityType.onField,
                                            true, CardAbility.Target.allAllies, CardAbility.SpeciesTarget.reptilia, new List<CardAbility.ModificationType> { CardAbility.ModificationType.modifyArmour }, new int[] { 1 }));

            cardAbility.Add(new CardAbility("Echolocation Accuracy", CardAbility.AbilityType.onField,
                                            true, CardAbility.Target.allAllies, CardAbility.SpeciesTarget.delphindae, new List<CardAbility.ModificationType> { CardAbility.ModificationType.modifyAttack }, new int[] { 2 }));

            // Adds all card facts to the card fact list
            cardFacts.Add("With a roar that can be heard from 5 miles afar");
            cardFacts.Add("Highly sociable, working together to hunt and maintain territory");
            cardFacts.Add("An intimidating foe with a weight of up to 270 kilograms");
            cardFacts.Add("One lethal bite is all it takes to immobilise and kill their prey");
            cardFacts.Add("2nd in the world ranking of the most powerful jaw with a strength of up to 19,000 psi");
            cardFacts.Add("The worlds fastest land animal, reaching speeds of up to 70 mph");
            cardFacts.Add("Known for their unusual hunting partnerships such as including badgers to take down large prey");
            cardFacts.Add("With the thickest fur of any bear species, can withstand freezing temperatures and vicious attacks");
            cardFacts.Add("Highly poisonous, the largest living lizard in the world");
            cardFacts.Add("Can spin their bodies in the air as many as seven rotations");
            cardFacts.Add("Solitary attackers that can leap up to 6 meters to strike a prey");
            cardFacts.Add("Opportunistic hunters that team up in packs of up to ten others");
            cardFacts.Add("Excellent climbers, and suprisingly agile hunters with a running speed of up to 30mph");
            cardFacts.Add("One of the oldest living reptiles on the planet, with the highest level of socialisation in their species");
            cardFacts.Add("With an echolocation range of up to 200m, search for prey with incredible accuracy");

            cardsAdded = true;
        }
    }
}
