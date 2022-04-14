using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiddlerLibrary
{   //Handles the cards of the deck, the Microsoft Word spellchecker, creates Player objects.
    //Centre of the Quiddler game's backend
    public class Deck : IDeck
    {
        private string _about = "Test Client for: Quiddler (TM) Library, © Lee Hutson and James Hill";
        private int _cardCount;
        private int _cardsPerPlayer;
        private string _topDiscard;
        private List<string> _deck;
        private const int TotalCards = 118;
        internal Checker _checker;

        //enums are capitalized to get around "in" being a c# keyword
        private enum CardFrequency
        {
            E = 12,
            A = 10,
            I = 8,
            O = 8,
            N = 6,
            R = 6,
            T = 6,
            U = 6,
            D = 4,
            G = 4,
            L = 4,
            S = 4,
            Y = 4,
            B = 2,
            C = 2,
            F = 2,
            H = 2,
            J = 2,
            K = 2,
            M = 2,
            P = 2,
            Q = 2,
            V = 2,
            W = 2,
            X = 2,
            Z = 2,
            CL = 2,
            ER = 2,
            IN = 2,
            QU = 2,
            TH = 2
        }

        private enum CardValue
        {
            Q = 15,
            Z = 14,
            J = 13,
            X = 12,
            V = 11,
            W = 10,
            CL = 10,
            QU = 9,
            TH = 9,
            B = 8,
            C = 8,
            K = 8,
            H = 7,
            ER = 7,
            IN = 7,
            F = 6,
            G = 6,
            P = 6,
            D = 5,
            M = 5,
            N = 5,
            R = 5,
            U = 4,
            Y = 4,
            L = 3,
            S = 3,
            T = 3,
            A = 2,
            E = 2,
            I = 2,
            O = 2
        }
        public string About => _about;

        public int CardCount => _cardCount;

        public int CardsPerPlayer
        {
            get => _cardsPerPlayer;

            set
            {
                if (value < 3 || value > 10)
                    throw new ArgumentOutOfRangeException("Value must be between 3 and 10 (inclusive).");
                else
                    _cardsPerPlayer = value;
            }
        }
        //returns the value of the top card in the discard "pile"
        public string TopDiscard
        {
            get
            {
                if(_topDiscard is null)
                {
                    _topDiscard = DrawCard();
                }
                return _topDiscard;
            }
        }
        //Creates a player and passes this deck as a reference
        public IPlayer NewPlayer()
        {
            IPlayer player = new Player(this);
            for (int i = 0; i < _cardsPerPlayer; i++)
            {
                player.DrawCard();
            }
            return player;
        }
        //Iterates over the deck, counting the number of each card sorted alphabetically and returns the results
        public override string ToString()
        {
            if (_cardCount == 0)
                return "Deck is out of cards!";
            SortedDictionary<string, int> cardList = new SortedDictionary<string, int>();
            string result = "The deck now contains the following " + _cardCount + " cards...\n";
            for(int i = TotalCards - _cardCount; i < TotalCards; i++)
            {
                try
                {
                    cardList.Add(_deck[i], 1);
                }
                catch (ArgumentException)
                {
                    cardList[_deck[i]]++;
                }
            }
            foreach(KeyValuePair<string, int> kvp in cardList)
            {
                result += kvp.Key + "(" + kvp.Value + ") ";
            }

            return result;
        }

        //Constructor
        public Deck()
        {
            _deck = new List<string>();
            CardFrequency current;
            int freq;
            _checker = new Checker();

            //For each card name, populate that card into the deck card-frequency number of times
            foreach (string card in Enum.GetNames(typeof(CardFrequency)))
            {
                //broken down into multiple steps for clarity and debugging; could all be inside the inner for()
                current = (CardFrequency)Enum.Parse(typeof(CardFrequency), card);
                freq = (int)current;
                for (int i = 0; i < freq; i ++)
                { 
                    _deck.Add(card.ToLower());
                }
            }
            Random rng = new Random();
            _deck = _deck.OrderBy(card => rng.Next()).ToList();
            _cardCount = _deck.Count();
        }
        //Pulls top card from deck and hands it to the player
        internal string DrawCard()
        {
            string card = _deck[TotalCards - _cardCount];
            _cardCount--;
            return card;
        }
        //sets the only discard card to the passed value
        internal void DiscardCard(string card)
        {
            _topDiscard = card;
        }
        //Checks each card against the Card Value enum to determine points of the current play
        internal int ScoreTally(string[] play)
        {
            int score = 0;                                  
            for (int i = 0; i < play.Length; i++)
            {
                score += (int)(CardValue)Enum.Parse(typeof(CardValue), play[i].ToUpper());
            }
            return score;
        }
        //Closes the Microsoft Word application
        public void Dispose()
        {
            _checker.Quit();
        }
    }
}
