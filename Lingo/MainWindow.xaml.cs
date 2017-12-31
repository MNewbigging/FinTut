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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace Lingo
{
    /// <summary>
    /// TODO: 
    /// - Topics sub section; create new db tables (same format as master/masterV) by adding
    /// new vocab and existing 
    /// - Revision section; flash cards, shows individual words with translation
    /// - Statistics section; user profile, test history
    /// - Grammar section; rules page? 
    /// If vocab is added to another table, should be added to master as well
    /// Check just added vocab; only add if it doesn't exist in table (use threads to do this in case the master table is massive)
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Load all currently existing topics in all display boxes
            LoadTopics();
        }

        // Initialise data handler obj
        DataHandler dataHandler = new DataHandler();
        // Debug aid
        Debugger debug = new Debugger();

        // ADD single word vocab to master table
        private void btnVGAdd_Click(object sender, RoutedEventArgs e)
        {
            // Invoke data handler to store data taken from text boxes
            dataHandler.MasterStore(txtBxVGFin.Text.ToLower(), txtBxVGEng.Text.ToLower());
            // Refresh list
            dataGridVocabGeneral.ItemsSource = dataHandler.ReadTable("MASTER").DefaultView;
            // Clear textboxes
            txtBxVGEng.Text = "";
            txtBxVGFin.Text = "";
            // Auto focus on eng text box
            txtBxVEng.Focusable = true;
            Keyboard.Focus(txtBxVEng);
        }

        // REFRESH general data grid
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dataGridVocabGeneral.ItemsSource = dataHandler.ReadTable("MASTER").DefaultView;
        }

        // REFRESH verbs data grid
        private void btnVerbRefresh_Click(object sender, RoutedEventArgs e)
        {
            dataGridVocabVerbs.ItemsSource = dataHandler.ReadTable("MASTERVERBS").DefaultView;
        }

        // ADD verb to master verb table
        private void btnVerbAdd_Click(object sender, RoutedEventArgs e)
        {
            dataHandler.MasterVerbStore(txtBxVEng.Text.ToLower(), txtBxVFin.Text.ToLower(), txtBxI.Text.ToLower(), txtBxYou.Text.ToLower(), txtBxHeShe.Text.ToLower(), txtBxIt.Text.ToLower(), txtBxWe.Text.ToLower(), txtBxYouPl.Text.ToLower(), txtBxThey.Text.ToLower());
            // Refresh list
            dataGridVocabVerbs.ItemsSource = dataHandler.ReadTable("MASTERVERBS").DefaultView;
            // Clear textboxes
            txtBxVEng.Text = ""; txtBxVFin.Text = ""; txtBxI.Text = ""; txtBxYou.Text = "";
            txtBxHeShe.Text = ""; txtBxIt.Text = ""; txtBxWe.Text = ""; txtBxYouPl.Text = ""; txtBxThey.Text = "";
        }

        // DELETE selected row from master verbs table
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            // Grab selected row in data grid
            DataRowView dataRow = (DataRowView)dataGridVocabVerbs.SelectedItem;
            // Grab id number of row
            int id = Convert.ToInt32(dataRow.Row.ItemArray[0]);
            // Send row id to data handler to be removed from master verbs table
            dataHandler.RemoveRow("MASTERVERBS", id);
        }

        // DELETE selected row from master table
        private void btnDelGeneral_Click(object sender, RoutedEventArgs e)
        {
            // Grab selected row in data grid
            DataRowView dataRow = (DataRowView)dataGridVocabGeneral.SelectedItem;
            // Grab id number of row
            int id = Convert.ToInt32(dataRow.Row.ItemArray[0]);
            Console.WriteLine("Row id: " + id);
            // Send row id to data handler to remove from master table
            dataHandler.RemoveRow("MASTER", id);
        }
          
        // START TEST - fetches & preps test data, displays in new window
        private void btnGenTest_Click(object sender, RoutedEventArgs e)
        {
            // Get all contents of chosen table
            // TODO - check that a topic is selected
            DataTable dt = dataHandler.ReadTable(cmboBxTopics.SelectedItem.ToString());           
            // Convert given size to int
            int size = Convert.ToInt32(txtBxSize.Text);
            // Pass these to test obj to handle test prep/execution
            Test test = new Test(dt, size);     
        }


        // 
        private void btnAddNewTopic_Click(object sender, RoutedEventArgs e)
        {

        }


        // Searches databse for all tables/topics, displays in combo/list boxes throughout app
        private void LoadTopics()
        {
            // Clear current boxes first
            cmboBxTopics.Items.Clear();
            lstBxAllTopics.Items.Clear();

            // Retrieve list of table names currently in db
            List<string> names = dataHandler.GetTables();

            // Add each name found to all display boxes
            foreach (string table in names)
            {
                cmboBxTopics.Items.Add(table);
                lstBxAllTopics.Items.Add(table);
            }

        }

    }
}
