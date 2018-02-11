﻿namespace RequestifyTF2
{
    using System;
    using System.Text;

    internal class CodeGenerator
    {
        private readonly string[] _consonant =
            { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };

        private readonly Random _rand = new Random();

        // set a new string array of vowels
        private readonly string[] _vowel = { "a", "e", "i", "o", "u" };

        public string GenerateWord(int length)
        {
            if (length < 1) // do not allow words of zero length
                throw new ArgumentException("Length must be greater than 0");

            var word = new StringBuilder();

            if (this._rand.Next() % 2 == 0) // randomly choose a vowel or consonant to start the word
                word.Append(this._consonant[this._rand.Next(0, 20)]);
            else
                word.Append(this._vowel[this._rand.Next(0, 4)]);

            for (var i = 1; i < length; i += 2)
            {
                // the counter starts at 1 to account for the initial letter
                // and increments by two since we append two characters per pass
                var c = this._consonant[this._rand.Next(0, 20)];
                var v = this._vowel[this._rand.Next(0, 4)];

                if (c == "q") // append qu if the random consonant is a q
                    word.Append("qu");
                else // otherwise just append a random consant and vowel
                    word.Append(c + v);
            }

            // the word may be short a letter because of the way the for loop above is constructed
            if (word.Length < length) // we'll just append a random consonant if that's the case
                word.Append(this._consonant[this._rand.Next(0, 20)]);
            return word.ToString();
        }
    }
}