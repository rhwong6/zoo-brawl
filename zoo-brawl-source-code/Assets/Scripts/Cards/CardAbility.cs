using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility
{
    // Variables to store the abilities name, and a boolean to store if the card has an ability
    public string abilityName;
    public bool hasAbility;

    // Variable to store the ability type
    public AbilityType abilityType;

    // Variable to store if the card has an action ability, which targets the action ability effects, what the action type is and its values
    public bool action;
    public Target actionTarget;
    public SpeciesTarget actionSpeciesTarget;
    public List<ActionType> actionType;
    public int[] actionValue;

    // Variable to store if the card has a modification ability, which targets the modification effect targets, what modification type it is and its values
    public bool statModification;
    public Target modificationTarget;
    public SpeciesTarget modificationSpeciesTarget;
    public List<ModificationType> modificationType;
    public int[] modificationValue;

    // Variable to store if the card has an attack modificaiton ability, what the attack modification type it is, and the values of these modifications
    public bool attackModification;
    public List<AttackModificationType> attackModificationType;
    public int[] attackModificationValue;

    // Enums that specify which values ability type, target, species target, action type, modificaiton type, attack modification type has
    public enum AbilityType
    {
        none,
        onPlay,
        onField,
        onAttack
    }

    public enum Target 
    { 
        none,
        self,
        allEnemies,
        facingEnemy,
        allAllies,
        adjacentAllies
    }

    public enum SpeciesTarget
    {
        none,
        feline,
        canine,
        ursidae,
        reptilia,
        delphindae,
        all
    }

    public enum ActionType
    {
        none,
        stun,
        quickAttack
    }

    public enum ModificationType
    {
        none,
        modifyAttack,
        modifyArmour,
        modifyHealth
    }

    public enum AttackModificationType
    {
        none,
        increasedAttackerAttack,
        increasedAttackerArmour,
        ignoredOpponentAttack,
        ignoreOpponentArmour
    }

    // Base constructor for cards that do not have an ability
    public CardAbility()
    {
        abilityName = "none";
        hasAbility = false;

        abilityType = AbilityType.none;

        action = false;
        actionTarget = Target.none;
        actionSpeciesTarget = SpeciesTarget.none;
        actionType = new List<ActionType> { };
        actionValue = new int[] { };

        statModification = false;
        modificationTarget = Target.none;
        modificationSpeciesTarget = SpeciesTarget.none;
        modificationType = new List<ModificationType> { };
        modificationValue = new int[] { };

        attackModification = false;
        attackModificationType = new List<AttackModificationType> { };
        attackModificationValue = new int[] { };
    }

    // Constructor for if the card has multiple abilities of differing types
    public CardAbility(string abilityName, AbilityType abilityType, 
                       bool action, Target actionTarget, SpeciesTarget actionSpeciesTarget, List<ActionType> actionType, int[] actionValue,
                       bool statModification, Target modificationTarget, SpeciesTarget modificationSpeciesTarget, List<ModificationType> modificationType, int[] modificationValue,
                       bool attackModification, List<AttackModificationType> attackModificationType, int[] attackModificationValue)
    {
        this.abilityName = abilityName;
        hasAbility = true;

        this.abilityType = abilityType;

        this.action = action;
        this.actionTarget = actionTarget;
        this.actionSpeciesTarget = actionSpeciesTarget;
        this.actionType = actionType;
        this.actionValue = actionValue;

        this.statModification = statModification;
        this.modificationTarget = modificationTarget;
        this.modificationSpeciesTarget = modificationSpeciesTarget;
        this.modificationType = modificationType;
        this.modificationValue = modificationValue;

        this.attackModification = attackModification;
        this.attackModificationType = attackModificationType;
        this.attackModificationValue = attackModificationValue;

    }

    // Constructor for cards with an action ability
    public CardAbility(string abilityName, AbilityType abilityType,
                       bool action, Target actionTarget, SpeciesTarget actionSpeciesTarget, List<ActionType> actionType, int[] actionValue)
    {
        this.abilityName = abilityName;
        hasAbility = true;

        this.abilityType = abilityType;

        this.action = action;
        this.actionTarget = actionTarget;
        this.actionSpeciesTarget = actionSpeciesTarget;
        this.actionType = actionType;
        this.actionValue = actionValue;

        statModification = false;
        modificationTarget = Target.none;
        modificationSpeciesTarget = SpeciesTarget.none;
        modificationType = new List<ModificationType> { };
        modificationValue = new int[] { };

        attackModification = false;
        attackModificationType = new List<AttackModificationType> { };
        attackModificationValue = new int[] { };
    }

    // Constructor for cards that have a stat modificaiton ability
    public CardAbility(string abilityName, AbilityType abilityType,
                       bool statModification, Target modificationTarget, SpeciesTarget modificationSpeciesTarget, List<ModificationType> modificationType, int[] modificationValue)
    {
        this.abilityName = abilityName;
        hasAbility = true;

        this.abilityType = abilityType;

        action = false;
        actionTarget = Target.none;
        actionSpeciesTarget = SpeciesTarget.none;
        actionType = new List<ActionType> { };
        actionValue = new int[] { };

        this.statModification = statModification;
        this.modificationTarget = modificationTarget;
        this.modificationSpeciesTarget = modificationSpeciesTarget;
        this.modificationType = modificationType;
        this.modificationValue = modificationValue;

        attackModification = false;
        attackModificationType = new List<AttackModificationType> { };
        attackModificationValue = new int[] { };
    }

    // Constructor for cards that have an attack modification ability
    public CardAbility(string abilityName, AbilityType abilityType,
                       bool attackModification, List<AttackModificationType> attackModificationType, int[] attackModificationValue)
    {
        this.abilityName = abilityName;
        hasAbility = true;

        this.abilityType = abilityType;

        action = false;
        actionTarget = Target.none;
        actionSpeciesTarget = SpeciesTarget.none;
        actionType = new List<ActionType> { };
        actionValue = new int[] { };

        statModification = false;
        modificationTarget = Target.none;
        modificationSpeciesTarget = SpeciesTarget.none;
        modificationType = new List<ModificationType> { };
        modificationValue = new int[] { };

        this.attackModification = attackModification;
        this.attackModificationType = attackModificationType;
        this.attackModificationValue = attackModificationValue;

    }
}
