using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public PokemonBase pokemonBase;
    public int level;
    public bool isPlayer;

    private Image pokemonImage;
    private Vector2 initPosition;
    private Color initialColor;

    public Pokemon pokemon;

    private void Awake()
    {
        pokemonImage = GetComponent<Image>();
        initPosition = transform.localPosition;
        initialColor = pokemonImage.color;
    }
    public void SetupPokemon()
    {
        pokemon = new Pokemon(pokemonBase, level);
        pokemonImage.DOFade(1, 0);
        pokemonImage.sprite = isPlayer ? pokemonBase.BackSprite : pokemonBase.FrontSprite;
        AnimationStartBattle();
    }

    public void AnimationStartBattle()
    {
        transform.localPosition = new Vector2(initPosition.x + (isPlayer? -1:1) * 400, initPosition.y);

        pokemonImage.transform.DOLocalMoveX(initPosition.x, 1);
    }

    public void AnimationAttack()
    {
        var seq = DOTween.Sequence();
        seq.Append(pokemonImage.transform.DOLocalMoveX(initPosition.x + (isPlayer ? 1 : -1) * 50, 0.2f));
        seq.Append(pokemonImage.transform.DOLocalMoveX(initPosition.x, 0.2f));
    }

    public void AnimationRecibeDamage()
    {
        var seq = DOTween.Sequence();
        

        for (int i = 0; i < 3 ; i++)
        {
            seq.Append(pokemonImage.DOColor(Color.grey, 0.15f));
            seq.Append(pokemonImage.DOColor(initialColor, 0.05f));
        }
    }

    public void AnimationFainted()
    {
        var seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMoveY(initPosition.y - 200, 1));
        seq.Join(pokemonImage.DOFade(0, 1));
    }
}
