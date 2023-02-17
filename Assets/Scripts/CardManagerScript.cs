using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name; //название Вируса
    public Sprite Face; //раскрас карты
    //public Sprite Logo; //Илюстрация
    public Sprite Back; //Рубашка
    public string Title; //Описание
    public int Power; //Attack and Deffend Point
    public CardType MyType;
    public Card (string Name, int Power, CardType ctype, string Title = "")
    {
        this.Name = Name;
        this.Power = Power;
        this.Title = Title;
        this.MyType = ctype;
        Back = Resources.Load<Sprite>("Images/card_back");
        switch (ctype)
        {
            case CardType.Virus:
                Face = Resources.Load<Sprite>("Images/card_V");
                break;
            case CardType.Trojan:
                Face = Resources.Load<Sprite>("Images/card_T");
                break;
            case CardType.AntiVirus:
                Face = Resources.Load<Sprite>("Images/card_A");
                break;
            case CardType.Worm:
                Face = Resources.Load<Sprite>("Images/card_W");
                break;
            default:
                Face = Resources.Load<Sprite>("Images/card_W");
                break;
        }
    }

}

public enum CardType
{
    Virus,
    AntiVirus,
    Worm,
    Trojan
}


public static class CardManager
{

    public static List<Card> AllMyCards = new List<Card>();
    public static List<Card> AllEnemyCards = new List<Card>();
}

public class CardManagerScript : MonoBehaviour
{

public static string[][] virusesNames = {
        new string[]{"Virus\nILoveYou","Virus\nCode RED","Virus\nMydoom" },
        new string[]{ "Antivirus\nA vas?", "Antivirus\nKawalski", "Antivirus\nDr. Wap", "Antivirus\nNorman"},
        new string[]{"Worm\nMorris","Worm\nConficker" },
        new string[]{"Trojan\nPenetrator","Trojan\nStorm"}};

    public void Awake()
    {
        int prevCardType = -1;
        for (int i = 0; i < 6; i++)
        {
            int rnd = Random.Range(0,4);
            if (rnd==prevCardType) rnd = Random.Range(0, 4); //anti 'Korean-random'
            prevCardType = rnd;
            CardType ct = (CardType)rnd;
            string name = virusesNames[rnd][Random.Range(0, virusesNames[rnd].Length-1)];
            int power = Random.Range(1, 6);
            if (ct == CardType.Trojan) power = Random.Range(1, 4);
            CardManager.AllMyCards.Add(new Card(name,power,ct));
        }
        for (int i = 0; i < 6; i++)
        {
            int rnd = Random.Range(0, 4);
            if (rnd == prevCardType) rnd = Random.Range(0, 4); //anti 'Korean-random'
            prevCardType = rnd;
            CardType ct = (CardType)rnd;
            string name = virusesNames[rnd][Random.Range(0, virusesNames[rnd].Length - 1)];
            int power = Random.Range(1, 6);
            if (ct == CardType.Trojan) power = Random.Range(1, 4);
            CardManager.AllEnemyCards.Add(new Card(name, power, ct));
        }
    }
}
