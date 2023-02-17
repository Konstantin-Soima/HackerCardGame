using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;
    public Image Face;
    public Text Name;
    public Text Power;
    public Text Title;
    public int intPower;
    public CardType MyType;

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        Face.sprite = card.Back;
        Power.text = "";
        Name.text = "";
        Title.text = "";
    }

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;
        Face.sprite = card.Face;
        Face.preserveAspect = true;
        Name.text = card.Name;
        intPower = card.Power;
        Power.text = card.Power.ToString();
        Title.text = card.Title;
        MyType = card.MyType;
    }
    private void Start()
    {
        //show my card and hide enemy
        if (transform.parent.name== "Enemy Cards")
            HideCardInfo(CardManager.AllEnemyCards[transform.GetSiblingIndex()]);
        else
            ShowCardInfo(CardManager.AllMyCards[transform.GetSiblingIndex()]);
    }
}
