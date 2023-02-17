using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardScripts : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera mainCamera;
    Vector3 offset;
    public Transform defaultParent;
    public Transform originalParent;
    public bool isDraggable;
    public int defaultIndex;
    Game game;

    void Awake()
    {
        mainCamera = Camera.allCameras[0];
        game = FindObjectOfType<Game>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);
        defaultParent = transform.parent;
        originalParent = transform.parent;
        defaultIndex = transform.GetSiblingIndex();
        Debug.Log("index=" + defaultIndex.ToString());
        isDraggable = defaultParent.GetComponent<DropPlayScripts>().fieldType == FieldType.MY_CARDS;
        if (!isDraggable || !game.isMyTurn()) return;
        transform.SetParent(defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable || !game.isMyTurn()) return;
        Vector3 newPos = /*mainCamera.ScreenToWorldPoint(*/eventData.position/*)*/;
        newPos.z = 20;
        transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable || !game.isMyTurn()) return;
        transform.SetParent(defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (originalParent.Equals(defaultParent))
        {
            transform.SetSiblingIndex(defaultIndex);
            return; //not attack if nothing change
        }
        Debug.Log("Attack Power " + GetComponent<CardInfoScript>().Power.text);
        Attack(GetComponent<CardInfoScript>(), defaultParent);
        game.changeTurn();
    }

    
    //TODO: Attack mast be in CardScript or CardInfoScript
    public void Attack(CardInfoScript cardInfo, Transform parent)
    {
        switch (cardInfo.MyType)
        {
            case CardType.Virus:
                //Attack on parrent card
                CardInfoScript parrentCardInfo = parent.GetComponent<CardInfoScript>();
                Debug.Log("parrent is " + parrentCardInfo.SelfCard.Name);
                //Test on antivirus 
                parrentCardInfo.SelfCard.Power -= cardInfo.SelfCard.Power;
                if (parrentCardInfo.SelfCard.Power < 1) //Delete if Win
                {
                    parrentCardInfo.ShowCardInfo(parrentCardInfo.SelfCard); //show lose card animation
                    Debug.Log("You kill them");
                    Thread.Sleep(500);
                    Destroy(parrentCardInfo.gameObject);
                }
                Thread.Sleep(500);
                Destroy(cardInfo.gameObject);
                break;
            case CardType.Trojan:
                //Attack on neibhour card
                //Get all childs
                CardInfoScript[] neibhourCardInfo = parent.GetComponentsInChildren<CardInfoScript>();
                foreach (var ncard in neibhourCardInfo)
                {
                    ncard.SelfCard.Power -= cardInfo.SelfCard.Power;
                    if (ncard.SelfCard.Power < 1)
                    {
                        Destroy(ncard.gameObject);
                    } else
                    {
                        DropPlayScripts dps = parent.GetComponent<DropPlayScripts>();
                        if (dps.fieldType == FieldType.MY_CARDS)
                        {
                            ncard.ShowCardInfo(ncard.SelfCard);
                        } else if (dps.fieldType == FieldType.ENEMY_CARDS)
                        {
                            ncard.HideCardInfo(ncard.SelfCard);
                        }
                    }
                }
                break;
            case CardType.Worm:
                //Attack on Parrent card and replace it with 0 healt

                CardInfoScript parrentCardInfoW = parent.GetComponent<CardInfoScript>();
                Debug.Log("parrent is " + parrentCardInfoW.SelfCard.Name);
                //Test on antivirus 
                parrentCardInfoW.SelfCard.Power -= cardInfo.SelfCard.Power;
                if (parrentCardInfoW.SelfCard.Power < 1) //Delete if Win
                {
                    parrentCardInfoW.ShowCardInfo(parrentCardInfoW.SelfCard); //show lose card animation
                    Debug.Log("You kill them");
                    Thread.Sleep(500);
                    Card newWorm = cardInfo.SelfCard;
                    newWorm.Power = (parrentCardInfoW.SelfCard.Power == 0) ? 1 : parrentCardInfoW.SelfCard.Power * -1;
                    parrentCardInfoW.ShowCardInfo(newWorm);
                }
                Thread.Sleep(500);
                Destroy(cardInfo.gameObject);
                break;
        }
    }

    public void DieAnimation(CardInfoScript card)
    {

    }
}
