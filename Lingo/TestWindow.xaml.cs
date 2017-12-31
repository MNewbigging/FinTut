using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lingo
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        // To store generated textboxes
        public List<TextBox> answerFields = new List<TextBox>();
        // Set as instance of test passed into window constructor
        Test test;
        public TestWindow(Test testInstance)
        {
            InitializeComponent();
            // Assign instance of test 
            test = testInstance;
           
            // Generate test controls
            for(int i = 0; i < test.questions + 1; i++)
            {
                // Add a row per each question
                // +1 for last row of button controls
                testGrid.RowDefinitions.Add(new RowDefinition());
                // Check if on last iteration
                if (i == test.questions)
                {
                    // Add button controls
                    Button btnEndTest = new Button();
                    btnEndTest.Content = "Done";
                    btnEndTest.Click += (object sender, RoutedEventArgs e) =>
                    {
                        // CHECK ANSWERS
                        CheckAnswers();
                    };
                    Grid.SetColumn(btnEndTest, 1);
                    Grid.SetRow(btnEndTest, i);
                    testGrid.Children.Add(btnEndTest);
                    // Add label to show overall result
                }
                // Otherwise populate test as normal
                else
                {
                    // Label to mark question number
                    Label lblNumber = new Label();
                    lblNumber.Content = i + 1 + ".";
                    Grid.SetColumn(lblNumber, 0);
                    Grid.SetRow(lblNumber, i);
                    testGrid.Children.Add(lblNumber);
                    // Label to show eng question word
                    Label lblEngQ = new Label();
                    lblEngQ.Content = test.engQuestions[i];
                    Grid.SetColumn(lblEngQ, 1);
                    Grid.SetRow(lblEngQ, i);
                    testGrid.Children.Add(lblEngQ);
                    // Textbox to allow for fin answer
                    TextBox txtBx = new TextBox();
                    txtBx.MinWidth = 50;
                    Grid.SetColumn(txtBx, 2);
                    Grid.SetRow(txtBx, i);
                    answerFields.Add(txtBx); // Add to list for easy retrieval later
                    testGrid.Children.Add(txtBx);                  
                }
               
            }

        }

        // Compares given asnwers with actuals
        private void CheckAnswers()
        {
            // First get list of answers
            List<string> userAnswers = new List<string>();
            foreach(TextBox tb in answerFields)
            {
                userAnswers.Add(tb.Text);
            }

            // Now compare given/actual lists
            for (int i = 0; i < test.questions; i++)
            {
                // If answer is the same; correct
                if (userAnswers[i] == test.finAnswers[i])
                {
                    // Show feedback/icon (label for now)
                    Label lblCorrect = new Label();
                    lblCorrect.Content = "Correct!";
                    lblCorrect.Foreground = Brushes.Green;
                    Grid.SetColumn(lblCorrect, 3);
                    Grid.SetRow(lblCorrect, i);
                    testGrid.Children.Add(lblCorrect);
                }
                else
                {
                    // Show correct answer
                    Label lblWrong = new Label();
                    lblWrong.Content = test.finAnswers[i];
                    lblWrong.Foreground = Brushes.Red;
                    Grid.SetColumn(lblWrong, 3);
                    Grid.SetRow(lblWrong, i);
                    testGrid.Children.Add(lblWrong);
                }
            }
        }

        // Created from inline declaration in constructor 
        private void BtnEndTest_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
