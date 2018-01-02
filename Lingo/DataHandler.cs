using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Lingo
{
    /// <summary>
    /// Data Handler deals with all db interactions
    /// </summary>
    class DataHandler
    {
        // Database connection string
        private string conString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mrnew\Documents\GitHub\FinTut\Lingo\lingo.mdf;Integrated Security=True;Connect Timeout=30";


        // Master table insert query
        public string masterInsertQ = "INSERT INTO MASTER (fin, eng, topic) VALUES (@fin, @eng, @topic)";
        // Master verbs insert query
        public string masterVerbsInsertQ = "INSERT INTO MASTERVERBS (veng, vfin, i, you, heShe, it, we, youPl, they) VALUES (@veng, @vfin, @i, @you, @heShe, @it, @we, @youPl, @they)";
        // Topic insert query
        public string topicInsertQ = "INSERT INTO Topics (topic) VALUES (@topic)";

        // General dt view master table read query
        public string masterGeneralReadQ = "SELECT id, eng, fin, topic FROM ";
        // Test master table read query
        public string masterTestReadQ = "SELECT eng, fin FROM ";
        // General dt view master verbs read query
        public string masterGeneralVerbsReadQ = "SELECT * FROM ";
        // Topics search query
        public string topicReadQ = "SELECT * FROM Topics";
        

            
        // Add entry to master table
        public void MasterStore(string fin, string eng, string topic)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand(masterInsertQ);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@fin", fin);
                    cmd.Parameters.AddWithValue("@eng", eng);
                    cmd.Parameters.AddWithValue("@topic", topic);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Master table successfully updated.");
            } catch (Exception e) { Console.WriteLine("Exception caught inserting to master: " + e.Message); }
        }


        // Add verb to master verbs table
        public void MasterVerbStore(string veng, string vfin, string i, string you, string heShe, string it, string we, string youPl, string they)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand(masterVerbsInsertQ);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@veng", veng);
                    cmd.Parameters.AddWithValue("@vfin", vfin);
                    cmd.Parameters.AddWithValue("@i", i);
                    cmd.Parameters.AddWithValue("@you", you);
                    cmd.Parameters.AddWithValue("@heShe", heShe);
                    cmd.Parameters.AddWithValue("@it", it);
                    cmd.Parameters.AddWithValue("@we", we);
                    cmd.Parameters.AddWithValue("@youPl", youPl);
                    cmd.Parameters.AddWithValue("@they", they);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Verb successfully added.");
                }
            } catch (Exception e) { Console.WriteLine("Exception caught inserting to master verbs: " + e.Message); }
        }


        // Add entry to topic table
        public void TopicStore(string topic)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand(topicInsertQ);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@topic", topic);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Topics table successfully updated.");
            } catch (Exception e) { Console.WriteLine("Exception caught inserting into topic table: " + e.Message); }
        }


        // Read given table contents, return data in dt
        public DataTable ReadTable(string table, string query)
        {
            // To be filled with data, mapped to data grid
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query + table + "", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);                   
                    sda.Fill(dt);
                }
                Console.WriteLine(table+" table read successfully");
            } catch (Exception e) { Console.WriteLine("Exception caught reading master: " + e.Message); }
            return dt;
        }

        // Read given table contents, return dt of only chosen topic
        public DataTable TestReadTable(string table, string topic)
        {
            // To be filled with data, mapped to data grid
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT eng, fin FROM " + table + " WHERE topic = '"+ topic +"'", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                Console.WriteLine(table + " table read successfully");
            }
            catch (Exception e) { Console.WriteLine("Exception caught reading master: " + e.Message); }
            return dt;
        }

        // Read all topics, return data as list of strings 
        public List<string> ReadTopics()
        {
            // Resulting list of topic names
            List<string> topics = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(topicReadQ, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        // Add value at 0 to list (only 1 column in this table)
                        topics.Add(row[0].ToString());
                    }
                    Console.WriteLine("Successfully found all topic names in db");
                }
            } catch (Exception e) { Console.WriteLine("Exception caught reading topics: " + e.Message); }
            
            return topics;
        }


        // Remove row of given id in given table
        public void RemoveRow(string table, int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM " + table + " WHERE id = " + id + "", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    Console.WriteLine("Row " + id + " has been successfully removed from " + table + " table.");
                }
            } catch (Exception e) { Console.WriteLine("Exception caught removing row from " + table + " table: " + e.Message); }
        }
      

        // Finds all table names in db
        public List<string> GetTables()
        {
            List<string> tables = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    // Get schema returns db name, table schema name, table name and type in each row
                    DataTable dt = con.GetSchema("Tables");                   
                    foreach(DataRow row in dt.Rows)
                    {
                        // Only want to keep the table name
                        tables.Add(row[2].ToString());
                    }
                    Console.WriteLine("Successfully found all table names in db");           
                }            
            } catch (Exception e) { Console.WriteLine("Exception caught getting table names: " + e.Message); }
            return tables;
        }

      
       


    }
}
