using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Lingo
{
    /// <summary>
    /// Single word test class:
    /// Fetches & prepares test data
    /// Opens test window, populates with controls
    /// Compares given answers with actuals for result
    /// </summary>
    public class Test
    {
        // Debug
        Debugger debug = new Debugger();
        // Table from db this test is based on
        DataTable testTable = new DataTable();
        // List of english question words
        public List<string> engQuestions = new List<string>();
        // List of finnish answer words
        public List<string> finAnswers = new List<string>();
        // List of user input answers
        public List<string> userAnswers = new List<string>();
        // Number of questions as determined by user
        public int questions;

        // Constructor preps test data
        public Test(DataTable dt, int size)
        {
            // Copy over data
            testTable = dt;
            questions = size;

            // Prepare test data
            PrepareTestData();
        }


        // Find random order of questions from table 
        private void PrepareTestData()
        {
            // Find total number of rows in table
            int totalRows = testTable.Rows.Count;

            // Ensure test size is within bounds
            if (questions > totalRows)
            {
                Console.WriteLine("Chosen table has less than " + questions + " rows.");
                // Reduce given size to table max
                questions = totalRows;
            }
            else if (questions <= 0)
            {
                Console.WriteLine("Must have at least 1 question!");
                questions = 1;
            }

            // Get random order of row indices
            int[] questionOrder = RandomRows(totalRows);
            // Debug
            debug.PrintArray(questionOrder, "questionOrder");
            // Pull data from table for each row in the random order chosen
            foreach (int index in questionOrder)
            {
                // Grab row at index
                DataRow row = testTable.Rows[index-1];
                finAnswers.Add(row[1].ToString());
                engQuestions.Add(row[2].ToString());
            }
            // Debug
            debug.PrintList(engQuestions, "engQuestions");
            debug.PrintList(finAnswers, "finAnswers");

            // Once data is prepped, open test window
            TestWindow test = new TestWindow(this);
            test.Show();
        }


        // Returns array of random indices for test questions
        private int[] RandomRows(int totalRows)
        {
            // Hash set won't include duplicates
            HashSet<int> rows = new HashSet<int>();
            // RNG
            Random random = new Random();

            while (rows.Count < questions)
            {
                // Random from inc, to excl
                rows.Add(random.Next(1, totalRows + 1));
            }

            // Convert resulting hash set into array
            int[] testRows = new int[rows.Count];
            rows.CopyTo(testRows);

            return testRows;
        }


        



    }
}
