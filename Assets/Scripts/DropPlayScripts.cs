using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    MY_CARDS,
    DESK,
    ENEMY_DESK,
    ENEMY_CARDS,
    ENEMY_CARD
}

public class DropPlayScripts : MonoBehaviour, IDropHandler
{
    public FieldType fieldType;
    Game game;
    public void OnDrop(PointerEventData eventData)
    {
        CardScripts card = eventData.pointerDrag.GetComponent<CardScripts>();
        CardInfoScript cardInfo = eventData.pointerDrag.GetComponent<CardInfoScript>();
        Debug.Log("DROP!");
        
        if (card)
        {

            //Logic for accept cards

            if (fieldType == FieldType.MY_CARDS ||
                    fieldType == FieldType.DESK||
                    ((cardInfo.MyType==CardType.Virus|| cardInfo.MyType == CardType.Worm)&& fieldType == FieldType.ENEMY_CARD) ||
                    (cardInfo.MyType == CardType.Trojan && fieldType == FieldType.ENEMY_CARDS)
                    )
            {
                card.defaultParent = transform;
               
            }
            else if (cardInfo.MyType == CardType.Trojan && fieldType == FieldType.ENEMY_CARD)
            {
                card.defaultParent = transform.parent;
                
            }
            else if (fieldType == FieldType.ENEMY_DESK)
            {
                card.defaultParent = GameObject.Find("Field").transform;
            }
        }
    }

    void Awake()
    {
        game = FindObjectOfType<Game>();
    }
}
