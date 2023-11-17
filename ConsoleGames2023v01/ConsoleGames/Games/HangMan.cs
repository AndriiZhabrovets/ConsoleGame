using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace ConsoleGames.Games
{
	public class HangMan : Game
	{
        public override string Name => "HangMan";
        public override string Description => "You have 6 lives. Each turn you can guess one letter from the word";
        public override string Rules => "The program accepts only letter";
        public override string Credits => "Zhabrovets Andrii, anzhabro@ksr.ch";
        public override int Year => 2023;
        public override bool TheHigherTheBetter => false;
        public override int LevelMax => 3;
        public override Score HighScore { get; set; }




        public override Score Play(int level = 1)
        {
            Score score = new Score();
            score.LevelCompleted = false;

            bool levelCompleted = false;

            if (level > LevelMax) level = LevelMax;

            char[] Alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            int Lives = 6;
            char[] UsedLetters = new char[Alphabet.Length];
            for (int i = 0; i < Alphabet.Length; i++)
            {
                UsedLetters[i] = '_';
            }
            string SecretWord = csvRandomReader(level);
            char[] EncodedWord = WordEncoder(SecretWord.ToArray());
            bool EndGame = false;
            HangTheMan(Lives, UsedLetters, EncodedWord, EndGame);
            while (true)
            {
                char Guess = ReadOneChar(UsedLetters, Alphabet);
                EvaluateTheSituation(Guess, SecretWord.ToArray(), ref Lives, ref UsedLetters, Alphabet, ref EncodedWord, ref EndGame, ref levelCompleted);
                HangTheMan(Lives, UsedLetters, EncodedWord, EndGame);
                if (EndGame)
                {
                    Console.WriteLine("Click The Button to Continue");
                    Console.ReadKey();
                    score.Level = level;
                    score.LevelCompleted = levelCompleted;
                    score.Points = Lives;
                    break;
                    
                }
                
            }
            return score;
        }

        static string csvRandomReader(int level)
        {
            string WordsSimplePath = "words_simple.csv";
            string WordsMediumPath = "words_medium.csv";
            string WordsHardPath = "words_hard.csv";
            string ChosenPath = "";
            if (level == 1)
            {
                ChosenPath = WordsSimplePath;
            }else if (level == 2)
            {
                ChosenPath = WordsMediumPath;
            }else if (level == 3)
            {
                ChosenPath = WordsHardPath;
            }

            using (var reader = new StreamReader(ChosenPath))
            {
                string line = reader.ReadLine();
                string[] csvArray = line.Split(',');
                Random rand = new Random();
                int RandomIndex = rand.Next(csvArray.Length);
                return csvArray[RandomIndex].ToUpper().Trim();

            }


        }

        static char ReadOneChar(char[] UsedLetters, char[] WhiteList)
        {
            char GuessLetter;
            while (true)
            {
                Console.WriteLine("Enter your guess: ");
                //string UserInput = Console.ReadLine().ToUpper();
                char CharToCheck = Char.ToUpper(Console.ReadKey().KeyChar);
                try
                {
                    //char CharToCheck = Convert.ToChar(UserInput);
                    if (WhiteList.Contains(CharToCheck) && !UsedLetters.Contains(CharToCheck))
                    {
                        GuessLetter = CharToCheck;
                        break;
                    }

                    Console.WriteLine("Wrong Input!");
                    continue;
                }
                catch
                {
                    Console.WriteLine("Wrong Input!");
                    continue;
                }
            }
            return GuessLetter;
        }

        static void EvaluateTheSituation(char Guess, char[] SecretWord, ref int Lives, ref char[] UsedLetters, char[] Alphabet, ref char[] EncodedWord, ref bool EndGame, ref bool levelCompleted)
        {
            bool Hit = false;
            int AmountOfCorrect = SecretWord.Count(x => x == Guess);
            int UsedIndex = Array.IndexOf(Alphabet, Guess);

            UsedLetters[UsedIndex] = Alphabet[UsedIndex];


            for (int i = 0; i < SecretWord.Length; i++)
            {
                Console.WriteLine(SecretWord[i]);
                if (SecretWord[i] == Guess)
                {
                    Hit = true;
                    EncodedWord[i] = SecretWord[i];
                }
            }
            if (!EncodedWord.Contains('_'))
            {
                levelCompleted = true;
                EndGame = true;
            }
            if (!Hit)
            {
                Lives = Lives - 1;
                if (Lives == 0)
                {
                    EndGame = true;
                    EncodedWord = SecretWord;
                }
            }
        }

        static void HangTheMan(int AmountLives, char[] UsedLetters, char[] EncodedWord, bool GameOver)
        {
            Console.Clear();
            string Hangman = "";

            if (AmountLives == 6)
            {

                Hangman = @"
_____________________
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||
| |/         || 
| |          || 
| |         (  )
| |        (    )
| |         \__/
| |       
| |         
| |           
| |            
| |           
| |             
| |            
| |             
""""""""""""""""""""|_________|""""""|
|""|""""""""""""""––––––––––'""|""|
| |                   | |
: :                   : :  
. .                   . .

            ";
            }
            else if (AmountLives == 5)
            {
                Hangman = @"
___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |         
| |       
| |         
| |           
| |            
| |           
| |             
| |            
| |             
""""""""""""""""""""|_        |""""""|
|""|""""""""""""""\ \       '""|""|
| |        \ \        | |
: :         \ \       : :  
. .          `'       . .

            ";
            }
            else if (AmountLives == 4)
            {
                Hangman = @"
    ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |          |. .| 
| |          |   | 
| |          | . |  
| |          |   |  
| |            
| |           
| |             
| |            
| |             
""""""""""""""""""""|_        |""""""|
|""|""""""""""""""\ \       '""|""|
| |        \ \        | |
: :         \ \       : :  
. .          `'       . .
            ";
            }
            else if (AmountLives == 3)
            {
                Hangman = @"
    ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'
| |        /Y . .| 
| |       // |   | 
| |      //  | . |  
| |     ')   |   |  
| |            
| |           
| |             
| |            
| |             
""""""""""""""""""""|_        |""""""|
|""|""""""""""""""\ \       '""|""|
| |        \ \        | |
: :         \ \       : :  
. .          `'       . .
            ";
            }
            else if (AmountLives == 2)
            {
                Hangman = @"
    ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |        /Y . . Y\
| |       // |   | \\
| |      //  | . |  \\
| |     ')   |   |   (`
| |            
| |           
| |             
| |            
| |             
""""""""""""""""""""|_        |""""""|
|""|""""""""""""""\ \       '""|""|
| |        \ \        | |
: :         \ \       : :  
. .          `'       . .
                ";
            }
            else if (AmountLives == 1)
            {
                Hangman = @"
    ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |        /Y . . Y\
| |       // |   | \\
| |      //  | . |  \\
| |     ')   |   |   (`
| |          ||'  
| |          ||   
| |          ||   
| |          ||   
| |         / |    
""""""""""""""""""""|_`-'     |""""""|
|""|""""""""""""""\ \       '""|""|
| |        \ \        | |
: :         \ \       : :  
. .          `'       . .
                ";

            }
            else if (AmountLives == 0)
            {
                Hangman = @"
    ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |        /Y . . Y\
| |       // |   | \\
| |      //  | . |  \\
| |     ')   |   |   (`
| |          ||'||
| |          || ||
| |          || ||
| |          || ||
| |         / | | \
""""""""""""""""""""|_`-' `-' |""""""|
|""|""""""""""""""\ \       '""|""|
| |        \ \        | |
: :         \ \       : :  
. .          `'       . .
                ";

            }





            Console.WriteLine(@"

Lives Left: {0}

Secret word: {1}

Used letters: {2}

Used letters:
        ", AmountLives, CharArrayPrinter(EncodedWord), CharArrayPrinter(UsedLetters)

            );

            Console.WriteLine(Hangman + "\n\n");
            Console.WriteLine(CharArrayPrinter(EncodedWord) + "\n\n");

            if (AmountLives == 0 && GameOver)
            {
                Console.WriteLine("You lost!\n");
            }
            else if (AmountLives != 0 && GameOver)
            {
                Console.WriteLine("You won!\n");
            }

        }


        static char[] WordEncoder(char[] WordToEncode)
        {
            char[] EncodedWord = WordToEncode;

            for (int i = 0; i < WordToEncode.Length; i++)
            {
                EncodedWord[i] = '_';
            }
            return EncodedWord;
        }


        static string CharArrayPrinter(char[] ToPrint)
        {
            string Result = "";
            for (int i = 0; i < ToPrint.Length; i++)
            {
                Result = Result + ToPrint[i];
            }
            return Result;
        }

    }
}

