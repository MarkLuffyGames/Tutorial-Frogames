using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BattleState
{
    Start,
    SelectPlayerAction,
    SelectPlayerMovemet,
    ExecuteActions,
    PlayerAction,
    RivalAction,
}
public class BattleManager : MonoBehaviour
{
    [Tooltip("Unidad de batalla del jugador.")]
    [SerializeField] private BattleUnit playerUnit;
    [Tooltip("Unidad de batalla del rival.")]
    [SerializeField] private BattleUnit rivalUnit;
    [Tooltip("HUD del jugador.")]
    [SerializeField] private BattleHUD playerHUD;
    [Tooltip("HUD del rival.")]
    [SerializeField] private BattleHUD rivalHUD;
    [Tooltip("Caja de dialogo de la batalla.")]
    [SerializeField] private BattleDialogBox battleDialogBox;

    [SerializeField] private BattleState state;

    [SerializeField] private int currentSelectedAction;
    [SerializeField] private int currentSelectedMovement;

    private InputAction moveMenu;
    private InputAction select;
    private InputAction back;

    private bool isFainted;

    private void Start()
    {
        moveMenu = InputSystem.actions.FindAction("MoveMenu");
        select = InputSystem.actions.FindAction("Select");
        back = InputSystem.actions.FindAction("Back");

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        state = BattleState.Start;

        currentSelectedAction = 0;
        currentSelectedMovement = 0;

        playerUnit.SetupPokemon();
        rivalUnit.SetupPokemon();

        playerHUD.SetPokemonData(playerUnit.pokemon);
        rivalHUD.SetPokemonData(rivalUnit.pokemon);

        battleDialogBox.SetPokemonMovement(playerUnit.pokemon);


        yield return battleDialogBox.SetDialog($"Un {rivalUnit.pokemon.Base.PokemonName} salvaje aparecio.");
        yield return new WaitForSeconds(1);
        PlayerActionSelect();
    }

    private void PlayerActionSelect()
    {
        state = BattleState.SelectPlayerAction;

        battleDialogBox.ToggleDialogText(false);
        battleDialogBox.ToggleMovesBox(false);
        battleDialogBox.ToggleActionBox(true);
        battleDialogBox.SetDialogActionText($"�Que deberia hacer {playerUnit.pokemon.Base.PokemonName}?");
        battleDialogBox.SelectedAction(currentSelectedAction);

    }

    private void PlayerMovement()
    {
        state = BattleState.SelectPlayerMovemet;

        battleDialogBox.ToggleMovesBox(true);
        battleDialogBox.SelectedMovement(currentSelectedMovement, playerUnit.pokemon.Moves[currentSelectedMovement]);
    }

    private void Update()
    {

        switch (state)
        {
            case BattleState.Start:
                break;
            case BattleState.SelectPlayerAction:
                HandlePlayerActionSelection();
                break;
            case BattleState.SelectPlayerMovemet:
                HandlePlayerMovementSelection();
                break;
            case BattleState.ExecuteActions:
                break;
            case BattleState.RivalAction:
                break;
            default:
                break;
        }
    }

    private void HandlePlayerActionSelection()
    {
        if (select.WasPressedThisFrame())
        {

            if(currentSelectedAction == 0)
            {
                PlayerMovement();
            }
            else if(currentSelectedAction == 1)
            {
                //Mochila
            }
            else if(currentSelectedAction == 2)
            {
                //Pokemon
            }
            else if(currentSelectedAction == 3)
            {
                //Huir
            }
        }

        if (moveMenu.ReadValue<Vector2>() == Vector2.zero) return;

        Selector(ref currentSelectedAction, 3);

        battleDialogBox.SelectedAction(currentSelectedAction);

    } 

    private void HandlePlayerMovementSelection()
    {
        if (select.WasPressedThisFrame())
        {
            StartCoroutine(ExecuteActions());
        }

        if (back.WasPressedThisFrame())
        {
            PlayerActionSelect();
        }

        if (moveMenu.ReadValue<Vector2>() == Vector2.zero) return;

        Selector(ref currentSelectedMovement, playerUnit.pokemon.Moves.Count -1);

        battleDialogBox.SelectedMovement(currentSelectedMovement, playerUnit.pokemon.Moves[currentSelectedMovement]);

    }

    private void Selector( ref int index, int positionAmount)
    {
        if (moveMenu.ReadValue<Vector2>() == Vector2.up)
        {
            if (index == 2 || index == 3)
            {
                index -= 2;
            }
        }
        else if (moveMenu.ReadValue<Vector2>() == Vector2.down)
        {
            if (index == 0 || index == 1)
            {
                if(index + 2 <= positionAmount) index += 2;
            }
        }
        else if (moveMenu.ReadValue<Vector2>() == Vector2.left)
        {
            if (index == 1 || index == 3)
            {
                index -= 1;
            }
        }
        else if (moveMenu.ReadValue<Vector2>() == Vector2.right)
        {
            if (index == 0 || index == 2)
            {
                if (index + 1 <= positionAmount) index += 1;
            }
        }
    }

    private IEnumerator ExecuteActions()
    {
        state = BattleState.ExecuteActions;

        battleDialogBox.ToggleActionBox(false);
        battleDialogBox.ToggleDialogText(true);
        battleDialogBox.ToggleMovesBox(false);

        bool playerFirst = false;

        if (playerUnit.pokemon.Speed > rivalUnit.pokemon.Speed)
        {
            playerFirst = true;
        }
        else if (playerUnit.pokemon.Speed < rivalUnit.pokemon.Speed)
        {
            playerFirst = false;
        }
        else
        {
            playerFirst = Random.Range(0, 2) == 0 ? true : false;
        }

        if(playerFirst)
        {
            yield return StartCoroutine(PlayerAttack());
            if(!isFainted) yield return StartCoroutine(RivalAttack());
        }
        else
        {
            yield return StartCoroutine(RivalAttack());
            if(!isFainted) yield return StartCoroutine(PlayerAttack());
        }

        if(!isFainted)
        {
            PlayerActionSelect();
        }
        else
        {
            
        }
        
    }

    private IEnumerator PlayerAttack()
    {
        state = BattleState.PlayerAction;

        if (playerUnit.pokemon.Moves[currentSelectedMovement].MoveBase.MoveClass != MoveClass.Status)
        {
            yield return StartCoroutine(DamageMove(
                playerUnit, rivalUnit, rivalHUD, playerUnit.pokemon.Moves[currentSelectedMovement]));
        }
        else
        {

        }

    }

    private IEnumerator RivalAttack()
    {
        state = BattleState.RivalAction;

        int randomMove = Random.Range(0, rivalUnit.pokemon.Moves.Count);

        if (rivalUnit.pokemon.Moves[randomMove].MoveBase.MoveClass != MoveClass.Status)
        {
            yield return StartCoroutine(DamageMove(
                rivalUnit, playerUnit, playerHUD, rivalUnit.pokemon.Moves[randomMove]));
        }
        else
        {
            
        }

    }

    private IEnumerator DamageMove(BattleUnit attacker, BattleUnit defender, BattleHUD defenderHUD, Move move)
    {
        var damageDescription = defender.pokemon.RecibeDamage(move, attacker.pokemon);
        var isfainted = damageDescription.IsFainted;

        yield return battleDialogBox.SetDialog(
            $"{attacker.pokemon.Base.PokemonName} a usado " +
            $"{move.MoveBase.MoveName}.");

        attacker.AnimationAttack();

        yield return new WaitForSeconds(0.5f);

        if (damageDescription.type != "")
        {
            yield return battleDialogBox.SetDialog(
            $"El ataque es {damageDescription.type}.");
        }

        defender.AnimationRecibeDamage();

        yield return StartCoroutine(defenderHUD.UpdateData(defender.pokemon.HP));

        if (damageDescription.Critical)
        {
            yield return battleDialogBox.SetDialog(
            $"Ha sido un golpe cr�tico.");
        }

        

        if (isfainted)
        {
            this.isFainted = true;
            yield return battleDialogBox.SetDialog($"{defender.pokemon.Base.PokemonName} se a debilitado");
            defender.AnimationFainted();
        }
    }
}
