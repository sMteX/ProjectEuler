using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectEuler.problems
{

    public class Problem54 : Problem
    {
        private static class RankingHelpers {
            public static bool areSameSuit(List<Card> cards) {
                return cards.All(c => c.suit == cards[0].suit);
            }
            public static bool isStraight(List<Card> cards) {
                for (int i = 0; i < cards.Count - 1; i++) {
                    if (cards[i+1].rank - cards[i].rank != 1) {
                        return false;
                    }
                }
                return true;
            }
            public static List<int> getRankHistogram(List<Card> cards) {
                List<int> list = Enumerable.Repeat(0, 13).ToList();
                foreach (var card in cards) {
                    list[(int)card.rank]++;
                }
                return list;
            }
            public static (bool, Rank?) hasFourOfAKind(List<int> histogram) {
                for (int i = 0; i < histogram.Count; i++) {
                    if (histogram[i] == 4) {
                        return (true, (Rank)i);
                    }
                }
                return (false, null);
            }
            public static (bool, Rank?) hasFullHouse(List<int> histogram) {
                bool foundTwo = false, foundThree = false;
                Rank? r = null;
                for (int i = 0; i < histogram.Count; i++) {
                    if (histogram[i] == 2) {
                        foundTwo = true;
                    } else if (histogram[i] == 3) {
                        foundThree = true;
                        r = (Rank)i;
                    }
                }
                if (foundThree && foundTwo) {
                    return (true, r);
                }
                return (false, null);
            }
            public static (bool, Rank?) hasThreeOfAKind(List<int> histogram) {
                for (int i = 0; i < histogram.Count; i++) {
                    if (histogram[i] == 3) {
                        return (true, (Rank)i);
                    }
                }
                return (false, null);
            }
            public static (bool, List<Rank>) hasTwoPairs(List<int> histogram) {
                List<Rank> p = pairs(histogram);
                if (p.Count == 2) {
                    return (true, p);
                }
                return (false, null);
            }
            public static List<Rank> pairs (List<int> histogram) {
                // count pairs, descending
                List<Rank> r = new List<Rank>();

                for (int i = histogram.Count - 1; i >= 0; i--) {
                    if (histogram[i] == 2) {
                        r.Add((Rank)i);
                    }
                }
                return r;
            }
        }

        private enum HandRanking {
            HighCard, OnePair, TwoPairs, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
        }
        private enum Suit {
            Clubs, Spades, Hearts, Diamonds
        }
        private enum Rank {
            Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
        }
        private struct Card {
            private static Dictionary<char, Rank> rankDict = new Dictionary<char, Rank> {
                { '2', Rank.Two },
                { '3', Rank.Three },
                { '4', Rank.Four },
                { '5', Rank.Five },
                { '6', Rank.Six },
                { '7', Rank.Seven },
                { '8', Rank.Eight },
                { '9', Rank.Nine },
                { 'T', Rank.Ten },
                { 'J', Rank.Jack },
                { 'Q', Rank.Queen },
                { 'K', Rank.King },
                { 'A', Rank.Ace }
            };
            private static Dictionary<char, Suit> suitDict = new Dictionary<char, Suit> {
                { 'C', Suit.Clubs },
                { 'S', Suit.Spades },
                { 'H', Suit.Hearts },
                { 'D', Suit.Diamonds}
            };

            public Rank rank { get; }
            public Suit suit { get; }

            public Card(string card) {
                if (!rankDict.ContainsKey(card[0]) || !suitDict.ContainsKey(card[1])) {
                    throw new NotImplementedException();
                }

                this.rank = rankDict[card[0]];
                this.suit = suitDict[card[1]];
            }
            public Card(Rank rank, Suit suit)
            {
                this.rank = rank;
                this.suit = suit;
            }
        }
        private class Hand {
            public List<Card> cards = new List<Card>();

            public List<(HandRanking, Rank)> BestHands = new List<(HandRanking, Rank)>();

            public Hand(List<Card> cards)
            {
                this.cards = cards;

                // analyze cards, determine best hands that the player has in decreasing, and also the highest card in that hand
                // e.g. 2H 2D 4C 4D 4S => (FullHouse, 4), (ThreeOfAKind, 4), (TwoPairs, 4), (OnePair, 4), (HighCard, 4), (HighCard, 2)
                this.cards.Sort((c1, c2) => c1.rank.CompareTo(c2.rank));
                // sorted in ASCENDING order by rank


                List<int> histogram = RankingHelpers.getRankHistogram(this.cards);

                // royal flush
                if (RankingHelpers.areSameSuit(this.cards) && RankingHelpers.isStraight(this.cards) && this.cards[0].rank == Rank.Ten) {
                    
                    this.BestHands.Add((HandRanking.RoyalFlush, Rank.Ace));
                    // automatically has many others, but it's NOT NEEDED
                    // there must be a clear winner and Royal Flush is the best possible hand
                    return;
                }

                // straight flush
                if (RankingHelpers.areSameSuit(this.cards) && RankingHelpers.isStraight(this.cards)) {
                    this.BestHands.Add((HandRanking.StraightFlush, this.cards.Last().rank));
                    // automatically has Flush, Straight, few HighCards
                    this.BestHands.Add((HandRanking.Flush, this.cards.Last().rank));
                    this.BestHands.Add((HandRanking.Straight, this.cards.Last().rank));
                    for (int i = this.cards.Count - 1; i >= 0; i--) {
                        this.BestHands.Add((HandRanking.HighCard, this.cards[i].rank));
                    }
                    
                    return;
                } 
                // from now on, it's not so clear - Four of a Kind might or might not have Flush e.g.
                // Four of a kind
                var (has4, rank4) = RankingHelpers.hasFourOfAKind(histogram);
                if (has4) {
                    this.BestHands.Add((HandRanking.FourOfAKind, rank4.Value));
                }
                // Full house
                var (hasFH, rankFH) = RankingHelpers.hasFullHouse(histogram);
                if (hasFH) {
                    this.BestHands.Add((HandRanking.FullHouse, rankFH.Value));
                }
                // Flush
                if (RankingHelpers.areSameSuit(this.cards)) {
                    this.BestHands.Add((HandRanking.Flush, this.cards.Last().rank));
                }
                // Straight
                if (RankingHelpers.isStraight(this.cards)) {
                    this.BestHands.Add((HandRanking.Straight, this.cards.Last().rank));
                }
                // Three of a kind
                var (has3, rank3) = RankingHelpers.hasThreeOfAKind(histogram);
                if (has3) {
                    this.BestHands.Add((HandRanking.ThreeOfAKind, rank3.Value));
                }
                // Two pairs
                var (has2pairs, pairList) = RankingHelpers.hasTwoPairs(histogram);
                if (has2pairs) {
                    foreach (var rank in pairList) {
                        this.BestHands.Add((HandRanking.TwoPairs, rank));
                    }
                }
                // Pairs
                var pairs = RankingHelpers.pairs(histogram);
                foreach (var rank in pairs) {
                    this.BestHands.Add((HandRanking.OnePair, rank));
                }
                // High card
                for (int i = this.cards.Count - 1; i >= 0; i--) {
                    this.BestHands.Add((HandRanking.HighCard, this.cards[i].rank));
                }
            }

        }
        private class Game {
            private Hand p1, p2;
            
            public Game(string game) {
                List<Card> cards1 = new List<Card>();
                List<Card> cards2 = new List<Card>();

                string[] cards = game.Split(" ");
                for (int i = 0; i < 5; i++) {
                    cards1.Add(new Card(cards[i]));
                }
                for (int i = 5; i < cards.Length; i++) {
                    cards2.Add(new Card(cards[i]));
                }

                this.p1 = new Hand(cards1);
                this.p2 = new Hand(cards2);
            }

            public int Winner {
                get {
                    int limit = Math.Min(p1.BestHands.Count, p2.BestHands.Count);
                    for (int i = 0; i < limit; i++) {
                        var (h1, r1) = p1.BestHands[i];
                        var (h2, r2) = p2.BestHands[i];
                        // better hand altogether
                        if (h1 > h2) {
                            return 1;
                        }
                        if (h2 > h1) {
                            return 2;
                        }
                        // same hand, better higher card
                        if (r1 > r2) {
                            return 1;
                        }
                        if (r2 > r1) {
                            return 2;
                        }
                    }
                    // from the definition of the problem - "and in each hand there is a clear winner."
                    // we shouldn't get here without a clear winner, just throw
                    throw new InvalidDataException();
                }
            }
        }

        public void run()
        {
            var games = parseGames();
            int wins = games.FindAll(game => game.Winner == 1).Count;
            System.Console.WriteLine($"Amount of games that player 1 won: {wins}");
        }

        private List<Game> parseGames() {
            StreamReader reader = File.OpenText("problems/Problem54_poker.txt");
            string line;

            List<Game> games = new List<Game>();

            while ((line = reader.ReadLine()) != null) {
                games.Add(new Game(line));
            }

            return games;
        }
    }
}