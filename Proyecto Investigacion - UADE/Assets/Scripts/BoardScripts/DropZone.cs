using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private enum DropZoneType { DECK_OF_CARDS, BOARD };
    
    [SerializeField] private DropZoneType _dropZoneType = DropZoneType.BOARD;

    private Draggable _draggeable = null;

    public void OnDrop(PointerEventData eventData)
    {
        if (_draggeable != null) 
        {
            float cardCost = _draggeable.GetComponent<CardDisplay>().CardCost;
            CheckCoinChanges(_draggeable, cardCost);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        _draggeable = eventData.pointerDrag.GetComponent<Draggable>();

        if (_draggeable != null)
        {
            _draggeable.ChangePlaceHolderParent(this.transform.parent); // I am changing just the placeholder parent because if I change parent too, if y didnt wanted to leave the card on the other drop zone it will leave it anyway
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (_draggeable != null)
        {
            _draggeable.ChangePlaceHolderParent(this.transform);
        }
    }

    private void CheckCoinChanges(Draggable draggeable, float cardCost) //Coins manager condition
    {
        if(draggeable.Played == false && _dropZoneType == DropZoneType.BOARD)
        {
            if(GameManager.instance.CurrentCoins >= cardCost)
            {
                draggeable.Played = true;
                GameManager.instance.ChangeCoins(-cardCost);
                draggeable.ChangeParent(this.transform);
            }
        }
        else if (draggeable.Played == true && _dropZoneType == DropZoneType.DECK_OF_CARDS)
        {
            draggeable.Played = false;
            GameManager.instance.ChangeCoins(cardCost);
            draggeable.ChangeParent(this.transform);
        }
    }
}
