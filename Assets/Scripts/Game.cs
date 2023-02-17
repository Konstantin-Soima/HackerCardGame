using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static bool youTurn = true;
    GameObject gamowerBaner;

    public void changeTurn()
    {
        youTurn = !youTurn;
        Debug.Log((youTurn) ? "your turn" : "enemy turn");
        var enemyField = GameObject.Find("Enemy Cards");
        var myField = GameObject.Find("My Cards");
        //if not make turn
        if (!youTurn) EnemyTurn(enemyField.GetComponentsInChildren<CardInfoScript>().Where( x=> x.enabled).ToList<CardInfoScript>()) ;
        //test, may be who win?
        haveWinner(myField.GetComponentsInChildren<CardInfoScript>().Where(x => x.enabled), enemyField.GetComponentsInChildren<CardInfoScript>().Where(x => x.enabled));
    }


    public bool isMyTurn()
    {
        return youTurn;
    }

    bool haveWinner(IEnumerable<CardInfoScript> myCards, IEnumerable<CardInfoScript> enemyCards)
    {
        bool gameover = false;
        int myPowers = 0;
        int enemyPowers = 0;
        foreach(var card in myCards)
        {
            if (card.SelfCard.MyType != CardType.AntiVirus) myPowers += card.SelfCard.Power;
        }
        foreach (var card in enemyCards)
        {
            if (card.SelfCard.MyType != CardType.AntiVirus) enemyPowers += card.SelfCard.Power;
        }
        if (enemyPowers==0 || myPowers == 0)
        {
            //somabody wins
            Debug.Log("You: " + myPowers.ToString() + " Enemy: " + enemyPowers.ToString());
            //gamowerBaner = GameObject.Find("Game_Over");
            Text txt = gamowerBaner.GetComponent<Text>();
            if (enemyPowers== myPowers)
            {
                txt.text = "DRAW";

            }
            else if (enemyPowers > myPowers)
            {
                txt.text = "YOU LOSE";
            }
            else if (myPowers > enemyPowers)
            {
                txt.text = "YOU WIN";
            }
            
            gamowerBaner.SetActive(true);
        }
        return gameover;
    }

    //Turns
    void EnemyTurn(List<CardInfoScript> cards)
    {
        if (cards.Count < 1)
        {
            changeTurn();
            return;
        }
        int iWill = Random.Range(0, cards.Count);
        CardInfoScript usedCard = cards[iWill];
        foreach (var card in cards)
        {
            Debug.Log("i have "+card.SelfCard.Name + " with "+card.SelfCard.Power.ToString() + " and is "+card.enabled.ToString());
        }
        switch (usedCard.SelfCard.MyType){
            case CardType.AntiVirus: //put them TO OWN FIELD for defend
                var enemyBoard = GameObject.Find("EnemyField");
                usedCard.ShowCardInfo(usedCard.SelfCard);
                usedCard.transform.SetParent(enemyBoard.transform);
                break;
            case CardType.Trojan:
                var myField = GameObject.Find("My Cards");
                usedCard.ShowCardInfo(usedCard.SelfCard);
                usedCard.transform.SetParent(myField.transform);
                CardScripts cs = usedCard.GetComponent<CardScripts>();
                cs.Attack(usedCard, myField.transform);
                break;
            case CardType.Virus:case CardType.Worm:
                var myField2 = GameObject.Find("My Cards");
                CardInfoScript[] liveCards = myField2.GetComponentsInChildren<CardInfoScript>().Where(x => x.enabled).ToArray();
                int iKill = Random.Range(0, liveCards.Length);
                CardInfoScript victimCard = liveCards[iKill];
                usedCard.ShowCardInfo(usedCard.SelfCard);
                usedCard.transform.SetParent(victimCard.transform);
                CardScripts cs2 = usedCard.GetComponent<CardScripts>();
                cs2.Attack(usedCard, victimCard.transform);
                break;
        }
        changeTurn();
    }

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        gamowerBaner = GameObject.Find("Game_Over");
        gamowerBaner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
