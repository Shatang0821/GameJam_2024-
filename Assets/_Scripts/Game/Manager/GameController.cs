﻿using System.Collections.Generic;
using FrameWork.EventCenter;
using FrameWork.Utils;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameController : Singleton<GameController>
{
    private List<Card> _selectedCards;                          //選択したカード
    private int _matchedCardTotal;                              //マッチしたカード総数
    
    private GameBoard _gameBoard;
    private Deck _deck;

    private int _cardTotal;
    public void Init(GameObject cardContainer)
    {
        _selectedCards = new List<Card>();
        
        //Deckクラスの初期化
        _deck = new Deck(cardContainer);
		
        //GameBoardクラスの初期化
        _gameBoard = new GameBoard(_deck,cardContainer);
        _gameBoard.Subscribe();

        _cardTotal = _deck.Cards.Count;
    }

    public void OnEnable()
    {
        _gameBoard?.OnEnable();
        _deck?.OnEnable();
    }

    public void OnDisable()
    {
        _gameBoard?.OnDisable();
        _deck?.OnDisable();
    }

    ~GameController()
    {
        _gameBoard.Unsubscribe();
    }
        
    // カード選択時の処理
    public async UniTaskVoid SelectCard(Vector3 mouseWorldPos)
    {
        var card = _gameBoard.SelecteCard(mouseWorldPos);
        if (card == null)
        {
            return;
        }
        
        if (!_selectedCards.Contains(card) && _selectedCards.Count < 2)
        {
            
            card.ToggleCardFace(true);
            //
            _selectedCards.Add(card);
        }

        // 2枚のカードが選択されたら自動的に一致判定を行う
        if (_selectedCards.Count == 2)
        {
            EventCenter.TriggerEvent(EventKey.OnGameStateChange, GamePlayState.CheckCards);
            await CheckCard();
        }
    }
    
    /// <summary>
    /// カードが一致の時のチェック
    /// </summary>
    /// <param name="total"></param>
    private async UniTask CheckCard()
    {
        if (_selectedCards[0].Id == _selectedCards[1].Id)
        {
            await UniTask.Delay(450);
            _selectedCards[0].SetCardMatched();
            _selectedCards[1].SetCardMatched();
            _matchedCardTotal += _selectedCards.Count;
            if (_matchedCardTotal == _cardTotal)
            {
                EventCenter.TriggerEvent(EventKey.OnGameStateChange,GamePlayState.End);
                return;
            }
        }
        else
        {
            await UniTask.Delay(500);
            _selectedCards[0].ToggleCardFace(false);
            _selectedCards[1].ToggleCardFace(false);
        }
        _selectedCards.Clear();
        EventCenter.TriggerEvent(EventKey.OnGameStateChange,GamePlayState.SelectCards);  
    }
}