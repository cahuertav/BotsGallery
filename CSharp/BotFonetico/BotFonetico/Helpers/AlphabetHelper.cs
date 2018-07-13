using System.Collections.Generic;

namespace BotFonetico.Helpers
{
    public class AlphabetHelper
    {
        private Dictionary<string, string> alphabetDictionary;

        private void PopulateDictionary()
        {
            alphabetDictionary = new Dictionary<string, string>();

            alphabetDictionary.Add("a", "alfa");
            alphabetDictionary.Add("b", "bravo");
            alphabetDictionary.Add("c", "coca");
            alphabetDictionary.Add("d", "delta");
            alphabetDictionary.Add("e", "eco");
            alphabetDictionary.Add("f", "foxtrot");
            alphabetDictionary.Add("g", "golfo");
            alphabetDictionary.Add("h", "hotel");
            alphabetDictionary.Add("i", "india");
            alphabetDictionary.Add("j", "julieta");
            alphabetDictionary.Add("k", "kilo");
            alphabetDictionary.Add("l", "lima");
            alphabetDictionary.Add("m", "metro");
            alphabetDictionary.Add("n", "noviembre");
            alphabetDictionary.Add("o", "oscar");
            alphabetDictionary.Add("p", "papa");
            alphabetDictionary.Add("q", "quebec");
            alphabetDictionary.Add("r", "romeo");
            alphabetDictionary.Add("s", "sierra");
            alphabetDictionary.Add("t", "tango");
            alphabetDictionary.Add("u", "unico");
            alphabetDictionary.Add("v", "victor");
            alphabetDictionary.Add("w", "wiskey");
            alphabetDictionary.Add("x", "extra");
            alphabetDictionary.Add("y", "yanke");
            alphabetDictionary.Add("z", "zulu");
        }

        public string GetLetterName(string selectedLetter)
        {
            PopulateDictionary();

            var theValue = alphabetDictionary[selectedLetter];
            return theValue;
        }
    }
}