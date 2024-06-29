using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public PokemonBase pokemonBase;
    public int level;
    public bool isPlayer;

    private Image image;
    private Vector2 initPosition;

    public Pokemon pokemon;

    private void Awake()
    {
        image = GetComponent<Image>();
        initPosition = transform.localPosition;

    }
    public void SetupPokemon()
    {
        pokemon = new Pokemon(pokemonBase, level);

        image.sprite = isPlayer ? pokemonBase.BackSprite : pokemonBase.FrontSprite;
        AnimationStartBattle();
    }

    public void AnimationStartBattle()
    {
        transform.localPosition = new Vector2(initPosition.x + (isPlayer? -1:1) * 400, initPosition.y);

        image.transform.DOLocalMoveX(initPosition.x, 1);
    }

}
