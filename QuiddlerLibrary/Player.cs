using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiddlerLibrary
{   //Class that handles a player entity, including their total score, the hand of cards they possess, and various methods helpful
    //to playing the game
    class Player : IPlayer
    {
        private int _cardCount;
        private int _totalPoints;
        private List<string> _hand;
        public int CardCount => _cardCount;
        public int TotalPoints => _totalPoints;
        private Deck _deck;

        //Constructor
        public Player(Deck d)
        {
            _deck = d;
            _hand = new List<string>();
            _totalPoints = 0;
        }
        //Sets a new card to the discard. The pile is an illusion, only stores most recent card, as it is only card accessible
        //in the game
        public bool Discard(string card)
        {
            bool foundCard = false;
            foreach(string c in _hand)
            {
                if (card == c)
                {
                    _deck.DiscardCard(c);
                    _hand.Remove(c);
                    foundCard = true;
                    break;
                }
            }
            return foundCard;
        }
        //Draws top card from deck.
        public string DrawCard()
        {
            if (_deck.CardCount == 0)
                throw new InvalidOperationException("Deck is out of cards");
            else
            {
                string card = _deck.DrawCard();
                _hand.Add(card);
                _cardCount++;
                return card;
            }
        }
        //Retrieves discard card instead of drawing from deck.
        public string PickupTopDiscard()
        {
            string card = _deck.TopDiscard;
            _hand.Add(card);
            return card;
        }
        //Plays a word after testing it in Testword. Adds the score to the player's total.
        public int PlayWord(string candidate)
        {
            int score = TestWord(candidate);
            if (score == 0)
                return score;
            string[] play = candidate.Split(' ');
            for (int i = 0; i < play.Length; i++)
            {
                foreach (string card in _hand)
                {
                    if (card == play[i])
                    {
                        _hand.Remove(card);
                        break;
                    }
                }
            }
            _totalPoints += score;
            return score;
        }
        //Tests a word for validity. Invalid words score 0. Word must not use all cards in hand, must use only cards in hand,
        //must be a real word according to Microsoft Word's spellcheck.
        public int TestWord(string candidate)
        {
            int score;
            bool validWord = false;
            string[] play = candidate.Split(' ');
            if (play.Length >= _hand.Count) //If user played entire hand, the word is no good
                return 0;
            List<string> handCopy = new List<string>(_hand);
            for(int i = 0; i < play.Length; i++) //If candidate word cannot be built from hand, word is no good
            {
                foreach(string card in handCopy)
                {
                    if(card == play[i])
                    {
                        validWord = true;
                        handCopy.Remove(card);
                        break;
                    }
                    validWord = false;
                }
            }
            if (!validWord)
                return 0;
            string check = "";
            for(int i = 0; i < play.Length; i++)
            {
                check += play[i];
            }
            if (!_deck._checker.CheckSpelling(check.ToLower()))
                return 0;
            score = _deck.ScoreTally(play); //Tally points based on Deck's enum
            return score;
        }
        //returns a string that contains the player's hand; bounded by square brackets '[' ']' and delimited by spaces ' '.
        public override string ToString()
        {
            string hand = "[ ";
            foreach(string card in _hand)
            {
                hand += card + " ";
            }
            hand += "]";
            return hand;
        }
    }
}
