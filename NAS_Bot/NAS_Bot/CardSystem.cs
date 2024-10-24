using System;

namespace NAS_Bot
{
    public class CardSystem
    {
        public int[] cardNumbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
        public string[] cardSuits = {"Clubs", "Spades", "Diamonds", "Hearts"};
        
        public int SelectedNumber { get; set; }
        public string SelectedCard { get; set; }

        public CardSystem()
        {
            var random = new Random();
            var numberIndex = random.Next(0, cardNumbers.Length - 1);
            var suitIndex = random.Next(0, cardSuits.Length - 1);

            SelectedNumber = cardNumbers[numberIndex];
            SelectedCard = $"{cardNumbers[numberIndex]} of {cardSuits[suitIndex]}";
        }
    }
}