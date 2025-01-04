using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kebaberia
{
    /// <summary>
    /// Emma Duprey
    /// Purpose: Record a players score in a text file along with their name and sort/display it according to rank
    /// Restrictions: naur
    /// </summary>
    internal class HighScore
    {
        //FIELDS
        private int score;

        private string name;

        private List<int> scoreList;

        private string fileName;

        private int iterates;

        //no properties at the moment because everything can stay in this class

        /// <summary>
        /// Paramaterized Constructor
        /// </summary>
        /// <param name="score">final score from end game state</param>
        public HighScore(int score)
        {
            this.score = score;
            scoreList = new List<int>();
            fileName = "HighScore";
            iterates = 0;
        }


        /// <summary>
        /// Reads the HighScore text file to get a list and dictionary of scores
        /// </summary>
        public void GetScoreList()
        {
            // set up the StreamReader
             StreamReader input = null;
             try
             {
                 // open the file for reading
                 string path = "..\\..\\..\\" + fileName;
                 input = new StreamReader(path);

                 // loop to read in and process each line
                 string line = null;
                 while ((line = input.ReadLine()) != null)
                 {
                     //  add the key to the array of score for sorting
                     scoreList.Add(Int32.Parse(line));
                 }

                 //  Clears out the entire file so that it can be rewritten in order
                 File.WriteAllText(path, String.Empty);
             }
             catch (Exception e)
             {
                 Console.WriteLine("Exception: " + e.Message);
             }
             finally
             {
                 // close the file
                 if (input != null)
                 {
                     input.Close();
                 }
             }
        }

        /// <summary>
        /// Iterates through the list to add where the new score should go, then writes in correct order to the text file
        /// </summary>
        public void AddNewScore()
        {
            //  Add in the new score
            scoreList.Add(score);

            // Sort the list from high to low
            scoreList.Sort();
            scoreList.Reverse();

            // declare a StreamWriter
            StreamWriter output = null;
            try
            {
                // try to open a file
                string path = "..\\..\\..\\" + fileName;
                output = new StreamWriter(path);
            
                // write it out to the file
                for (int i = 0; i < scoreList.Count; i++)
                {
                    output.WriteLine(scoreList[i]);
                }
            
            
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                // close the file
                if (output != null)
                {
                    output.Close();
                }
            }
            
            
        }

        /// <summary>
        /// Displays the scores of the top 10
        /// </summary>
        public List<int> DisplayScores()
        {

            List<int> topScores = new List<int>();

            for (int i = 0;i < scoreList.Count && i <= 10;i++)
            {
                topScores.Add(scoreList[i]);
            }

            return topScores;
        }

        public List<int> RunHighScore()
        {
            GetScoreList();
            AddNewScore();
            List<int> topScores = DisplayScores();

            return topScores;
        }
    }
}
