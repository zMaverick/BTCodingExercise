using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTCodeExercise
{
    /// <summary>
    /// To Use:
    /// 
    /// Either Build or Debug creating an .exe file, in the console (cmd) navigate to the location of the file using cd (change directory)
    /// If debugging the file will be located in the \BTCodeExercise\BTCodeExercise\bin\Debug\ folder
    /// Run the file with the names to be checked as arguments separated by spaces (for example: C:\...\filelocation>BTCodeExercise.exe arg1 arg2)
    //  Input the names manually (15 Max) one per line, or use a text file (for example: C:\...\filelocation>BTCodeExercise.exe arg1 arg2 < inputnames.txt)
    /// The matching  names will be output to the console at the bottom
    /// Note: A list of 15 surnames in a text file is located in the Debug folder for use
    /// </summary>
    
    class Program
    {
        /* The following instance variables are used for checking letters against */
        #region Instance Variables
        public string strAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";   //String that holds all alphabetic letter (used to discard numbers, punctuation etc.)

        public string strDiscard = "AEIHOUWY";                      //String that holds the characters to be discarded after the first letter

        public string strVowels = "AEIOU";                          //Equivalent vowel letters
        public string strConstsA = "CGJKQSXYZ";                     //Equivalent consonant letters (Group A)
        public string strConstsB = "BFPVW";                         //Equivalent consonant letters (Group B)
        public string strConstsC = "DT";                            //Equivalent consonant letters (Group C)
        public string strConstsD = "MN";                            //Equivalent consonant letters (Group D)
        #endregion

        static void Main(string[] args)
        {
            Program progInstance = new Program();       //Instance of the Program class for access to instance variables

            int iListSize = 15;                         //Integer to hold the size of the list of names to match against

            string[] strArgCodes = new string[args.Length];     //Array of strings, the same size as the arguments array, used to hold Phonetic Codes for the equivalent argument

            string[] strNames = new string[iListSize];          //Array of strings, the size as the list of names inputted, used to hold the inputted names
            string[] strNameCodes = new string[iListSize];      //Array of strings, the same size as the inputted names array, used to hold Phonetic Codes for the equivalent name
            
            Console.WriteLine("Input names (15), one per line -\n");    //Console output to show the inputted names

            /* The following loop runs for the amount of inputted names */ 
            for (int i = 0; i < strNames.Length; i++)
            {
                strNames[i] = Console.ReadLine();       //Inputs are read from the console and assigned to a space in names array
                strNameCodes[i] = AssignPhoneticCode(strNames[i], progInstance);    //Generate a phonetic code for the inputted name and assign to the array that holds the codes

                Console.WriteLine(strNames[i]);         //Console output to list the inputted names
            }

            Console.WriteLine("\nThe arguments inputted have the following matches -\n");      //Console output for the matching names

            /* The following loop runs for each of the arguments */ 
            for (int i = 0; i < args.Length; i++)
            {
                List<string> strMatches = new List<string>();   //List of strings to hold matching names to the argument
                StringBuilder strOutput = new StringBuilder();  //StringBuilder to construct a formatted string from the matching names

                strArgCodes[i] = AssignPhoneticCode(args[i], progInstance);     //Generate a phonetic code for the inputted argument and assign to the array that holds the arguments codes

                /* The following loop runs for all the input names phonetic codes */ 
                for (int e = 0; e < strNameCodes.Length; e++)
                {
                    /* Under the condition the argument's code matches an inputted name's code*/
                    if (strArgCodes[i] == strNameCodes[e])
                    {
                        strMatches.Add(strNames[e]);    //Add the equivalent name to the list of matching names
                    }
                }

                /* The following loop runs for all the names that match the argument */ 
                for (int e = 0; e < strMatches.Count; e++)
                {
                    /* Is this the first iteration of this loop (The first name in the list) */
                    if (e > 0)
                    {
                        strOutput.Append(", "+strMatches[e]);   //If not, punctuate it and add to the string builder
                    }
                    else
                    {
                        strOutput.Append(strMatches[e]);        //If so, leave unpunctuated and add to the string builder
                    }
                }

                Console.WriteLine(args[i] + ": " + strOutput);  //Output to the console: the argument, followed by the matching names
            }        

        }

        /// <summary>AssignPhoneticCode is a method used to generate a numeric code based on the inputted string
        /// <para>
        /// It is based on the SoundEX matching algorithm, where each letter in a word (name in this case) is assigned a number value or discarded based on the letter.
        /// Letters are checked against groups of equivalents (similar sounding letters: A, E, I, O and U) and assigned a value based on which category they fall into.
        /// Numbers and punctuation are discarded immediately along with certain letters after the initial letter.
        /// Finally reoccurring letters are ignored and treated as a single letter (mm becomes m, and thus assigned a 5)
        /// </para>
        /// </summary> 

        public static string AssignPhoneticCode(string input, Program progInstance)
        {
            string strTempName = input.ToUpper();   //The inputted name is made capital and then assigned to a temporary string variable

            string strCharValue = "";       //String variable used to assign the current character that is being checked a numeric value
            string strLastCharValue = "";   //String variable that is assigned the previous character's value, used for removing reoccurring letters
            string strNameValue = "";       //The total value for the input name, it is added upon each iteration

            /* The following loop runs for the character length of the inputted string */ 
            for (int i = 0; i < strTempName.Length; i++)
            {
                strCharValue = "";  //Reset the current character value for reuse

                /* On the condition the current character is alphabetic */
                if (progInstance.strAlphabet.IndexOf(strTempName[i]) > -1)
                {
                    /* This function checks all letters other than the first letter against the group of letters to discard */
                    if (i > 0 && progInstance.strDiscard.IndexOf(strTempName[i]) > -1)
                    {
                        strCharValue = "";  //If it's not the first and is in the discard group: discard the character
                    }
                    else   // If it is the first letter or isn't in the discard group
                    {
                        /* Check against each of the groups of letters */
                        if (progInstance.strVowels.IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "1";     //If it's in the vowels group, assign 1
                        }
                        else if (progInstance.strConstsA.IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "2";     //If it's in the consonant group A, assign 2
                        }
                        else if (progInstance.strConstsB.IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "3";     //If it's in the consonant group B, assign 3
                        }
                        else if (progInstance.strConstsC.IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "4";     //If it's in the consonant group C, assign 4
                        }
                        else if (progInstance.strConstsD.IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "5";     //If it's in the consonant group D, assign 5
                        }
                        else if ("H".IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "7";     //If it's an H, assign 6
                        }
                        else if ("L".IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "8";     //If it's an L, assign 7
                        }
                        else if ("R".IndexOf(strTempName[i]) > -1)
                        {
                            strCharValue = "9";     //If it's an R, assign 9
                        }
                    }
                }
                else   //If the character is not alphabetic
                {
                    strCharValue = "";  //Discard this character
                }

                /* If this character has the same previous character value (reoccurring letter) */
                if (strCharValue == strLastCharValue)
                {
                    strCharValue = "";  //Discard this character
                }
                else    //If it doesn't
                {
                    strLastCharValue = strCharValue;                //Set the current character to the previous for the next iteration
                    strNameValue = strNameValue + strCharValue;     //Add the character value to the name value
                }
            }

            /* When this function completes, return the phonetic value string generated */
            return strNameValue;
        }

    }
}
