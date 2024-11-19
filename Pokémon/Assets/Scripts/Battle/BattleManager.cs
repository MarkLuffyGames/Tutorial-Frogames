using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public enum BattleState
{
    Start,
    SelectPlayerAction,
    SelectPlayerMovemet,
    ExecuteActions,
    PlayerAction,
    RivalAction,
    SelectPokemon
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
    [Tooltip("Menu de seleccion de Pokemons")]
    [SerializeField] private PartyHUD playerPartyHUD;

    [SerializeField] private BattleState state;

    [SerializeField] private int currentSelectedAction;
    [SerializeField] private int currentSelectedMovement;

    private InputAction moveMenu;
    private InputAction select;
    private InputAction back;

    private bool isFainted;

    public event Action<bool> OnFinishedBattle;

    private PokemonParty playerParty;
    private Pokemon wildPokemon;

    private void Start()
    {
        moveMenu = InputSystem.actions.FindAction("MoveMenu");
        select = InputSystem.actions.FindAction("Select");
        back = InputSystem.actions.FindAction("Back");


        moveMenu.performed += MoveMenu_performed;
        select.started += Select_started;
        back.started += Back_started;
    }

    private void MoveMenu_performed(InputAction.CallbackContext obj)
    {
        if (moveMenu.WasPressedThisFrame())
        {
            var noveDir = OneDirectionMove.OneDirection(moveMenu.ReadValue<Vector2>().normalized,Vector3.zero);
            switch (state)
            {
                case BattleState.SelectPlayerAction:
                    HandlePlayerActionSelection(noveDir);
                    break;
                case BattleState.SelectPlayerMovemet:
                    HandlePlayerMovementSelection(noveDir);
                    break;
                case BattleState.SelectPokemon:
                    HandleSelectPokemon(noveDir);
                    break;
                default:
                    break;
            }
        }
        
    }
    private void Select_started(InputAction.CallbackContext obj)
    {
        switch (state)
        {
            case BattleState.SelectPlayerAction:
                SelectAction();
                break;
            case BattleState.SelectPlayerMovemet:
                SelectMovement();
                break;
            case BattleState.SelectPokemon:
                StartCoroutine(SelectPokemon());
                break;
            default:
                break;
        }
    }

    private void Back_started(InputAction.CallbackContext obj)
    {
        switch (state)
        {
            case BattleState.SelectPlayerMovemet:
                PlayerActionSelect();
                break;
            case BattleState.SelectPokemon:
                PlayerActionSelect();
                break;
            default:
                break;
        }
    }

    public void HandleStartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        state = BattleState.Start;

        currentSelectedAction = 0;
        currentSelectedMovement = 0;

        playerUnit.SetupPokemon(playerParty.GetFirstNoFaintedPokemon());
        rivalUnit.SetupPokemon(wildPokemon);

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

        playerPartyHUD.gameObject.SetActive(false);
        battleDialogBox.ToggleDialogText(false);
        battleDialogBox.ToggleMovesBox(false);
        battleDialogBox.ToggleActionBox(true);
        battleDialogBox.SetDialogActionText($"¿Que deberia hacer {playerUnit.pokemon.Base.PokemonName}?");
        battleDialogBox.SelectedAction(currentSelectedAction);

    }

    private void PlayerMovement()
    {
        state = BattleState.SelectPlayerMovemet;

        battleDialogBox.ToggleMovesBox(true);
        battleDialogBox.SelectedMovement(currentSelectedMovement, playerUnit.pokemon.Moves[currentSelectedMovement]);
    }

    private void SelectingPokemon()
    {
        state = BattleState.SelectPokemon;
        currentPokemonSelected = 0;
        playerPartyHUD.gameObject.SetActive(true);
        playerPartyHUD.SetPartyData(playerParty.Pokemons);
    }

    private void HandlePlayerActionSelection(Vector2 moveDir)
    {
        if (moveMenu.ReadValue<Vector2>() == Vector2.zero) return;

        Selector(ref currentSelectedAction, 3, moveDir);

        battleDialogBox.SelectedAction(currentSelectedAction);

    } 

    private void SelectAction()
    {
        if (currentSelectedAction == 0)
        {
            PlayerMovement();
        }
        else if (currentSelectedAction == 1)
        {
            //Mochila
        }
        else if (currentSelectedAction == 2)
        {
            SelectingPokemon();
        }
        else if (currentSelectedAction == 3)
        {
            OnFinishedBattle(false);
        }
    }

    private void HandlePlayerMovementSelection(Vector2 moveDir)
    {
        if (moveMenu.ReadValue<Vector2>() == Vector2.zero) return;

        Selector(ref currentSelectedMovement, playerUnit.pokemon.Moves.Count -1, moveDir);

        battleDialogBox.SelectedMovement(currentSelectedMovement, playerUnit.pokemon.Moves[currentSelectedMovement]);

    }

    private void SelectMovement()
    {
        if (playerUnit.pokemon.Moves[currentSelectedMovement].PowerPoints > 0)
        {
            StartCoroutine(ExecuteMovement());
        }
        else
        {
            //No quedan PP.
        }
    }

    private void Selector( ref int index, int positionAmount, Vector2 moveDir)
    {
        if (moveDir == Vector2.up)
        {
            if (index == 2 || index == 3)
            {
                index -= 2;
            }
        }
        else if (moveDir == Vector2.down)
        {
            if (index == 0 || index == 1)
            {
                if(index + 2 <= positionAmount) index += 2;
            }
        }
        else if (moveDir == Vector2.left)
        {
            if (index == 1 || index == 3)
            {
                index -= 1;
            }
        }
        else if (moveDir == Vector2.right)
        {
            if (index == 0 || index == 2)
            {
                if (index + 1 <= positionAmount) index += 1;
            }
        }
    }

    private IEnumerator ExecuteMovement()
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
            isFainted = false;
            if(playerUnit.pokemon.HP == 0)
            {   
                if(playerParty.GetFirstNoFaintedPokemon() != null)
                {
                    playerUnit.SetupPokemon(playerParty.GetFirstNoFaintedPokemon());
                    playerHUD.SetPokemonData(playerUnit.pokemon);
                    battleDialogBox.SetPokemonMovement(playerUnit.pokemon);

                    yield return battleDialogBox.SetDialog($"¡Adelante {playerUnit.pokemon.Base.PokemonName}!");

                    PlayerActionSelect();
                }
                else
                {
                    OnFinishedBattle(false);
                }
            }
            else
            {
                OnFinishedBattle(true);
            }
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
            // Ataques de Estado.
        }

    }

    private IEnumerator RivalAttack()
    {
        state = BattleState.RivalAction;

        Move randomMove = RandomMove(rivalUnit.pokemon.Moves);

        if (randomMove.MoveBase.MoveClass != MoveClass.Status)
        {
            yield return StartCoroutine(DamageMove(
                rivalUnit, playerUnit, playerHUD, randomMove));
        }
        else
        {
            // Ataques de Estado.
        }

    }

    private Move RandomMove(List<Move> moves)
    {
        int random = Random.Range(0, moves.Count);
        if (moves[random].PowerPoints > 0)
        {
            return moves[random];
        }
        else
        {
            bool remainPP = false;
            foreach (Move move in moves)
            {
                if(move.PowerPoints > 0)
                {
                    remainPP = true;
                    break;
                }
            }
            if (remainPP)
            {
                return RandomMove(moves);
            }
            
        }

        return null;
    }

    private IEnumerator DamageMove(BattleUnit attacker, BattleUnit defender, BattleHUD defenderHUD, Move move)
    {
        move.PowerPoints--;

        var damageDescription = defender.pokemon.RecibeDamage(move, attacker.pokemon);
        var isfainted = damageDescription.IsFainted;

        yield return battleDialogBox.SetDialog(
            $"{attacker.pokemon.Base.PokemonName} a usado " +
            $"{move.MoveBase.MoveName}.");

        attacker.AnimationAttack();

        yield return new WaitForSeconds(0.5f);

        defender.AnimationRecibeDamage();

        yield return StartCoroutine(defenderHUD.UpdateData(defender.pokemon.HP));

        if (damageDescription.type != "")
        {
            yield return battleDialogBox.SetDialog(
            $"¡El ataque es {damageDescription.type}!");
        }

        if (damageDescription.Critical)
        {
            yield return battleDialogBox.SetDialog(
            $"¡Ha sido un golpe crítico!");
        }

        if (isfainted)
        {
            this.isFainted = true;
            yield return battleDialogBox.SetDialog($"{defender.pokemon.Base.PokemonName} se a debilitado.");
            defender.AnimationFainted();
            yield return new WaitForSeconds(1.5f);
        }
    }


    int currentPokemonSelected;
    bool isBattlePokemon;
    private void HandleSelectPokemon(Vector2 moveDir)
    {
        if (moveDir == Vector2.up)
        {
            if(isBattlePokemon) currentPokemonSelected = 0;
            if(currentPokemonSelected == 0) isBattlePokemon = false;
            currentPokemonSelected--;
            if (currentPokemonSelected == 0) isBattlePokemon = true;
            if (currentPokemonSelected < 0) currentPokemonSelected = playerParty.Pokemons.Count - 1;
        }
        else if (moveDir == Vector2.down)
        {
            if(isBattlePokemon) currentPokemonSelected = 0;
            if (currentPokemonSelected == 0) isBattlePokemon = false;
            currentPokemonSelected++;
            if (currentPokemonSelected == 0) isBattlePokemon = true;
            if (currentPokemonSelected > playerParty.Pokemons.Count - 1) currentPokemonSelected = 0;
        }
        else if (moveDir == Vector2.left)
        {
            if(currentPokemonSelected != playerParty.Pokemons.Count)
            {
                isBattlePokemon = true;
            }
            else
            {
                //Estoy en el boton exit.
            }
        }
        else if (moveDir == Vector2.right)
        {
            isBattlePokemon = false;
            if (currentPokemonSelected == 0) currentPokemonSelected++;
        }


        playerPartyHUD.SelectPokemon(isBattlePokemon ? 0 : currentPokemonSelected);
    }

    private IEnumerator SelectPokemon()
    {
        state = BattleState.ExecuteActions;
        playerPartyHUD.gameObject.SetActive(false);
        battleDialogBox.ToggleDialogText(true);
        battleDialogBox.ToggleMovesBox(false);
        battleDialogBox.ToggleActionBox(false);
        yield return battleDialogBox.SetDialog($"Vuelva {playerUnit.pokemon.Base.PokemonName}.");
        playerUnit.AnimationFainted();
        yield return new WaitForSeconds(1);
        yield return battleDialogBox.SetDialog($"Ve {playerParty.Pokemons[currentPokemonSelected].Base.PokemonName}");
        playerUnit.SetupPokemon(playerParty.Pokemons[currentPokemonSelected]);
        playerHUD.SetPokemonData(playerUnit.pokemon);
        battleDialogBox.SetPokemonMovement(playerUnit.pokemon);
        PlayerActionSelect();
    }
}
