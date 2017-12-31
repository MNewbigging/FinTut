﻿using System;
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
        private string conString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=c:\users\mrnew\documents\visual studio 2015\Projects\Lingo\Lingo\lingo.mdf;Integrated Security=True;Connect Timeout=30";

        

        // Master table insert query
        private string masterInsertQ = "INSERT INTO MASTER (fin, eng) VALUES (@fin, @eng)";
        // Master verbs insert query
        private string masterVerbsInsertQ = "INSERT INTO MASTERVERBS (veng, vfin, i, you, heShe, it, we, youPl, they) VALUES (@veng, @vfin, @i, @you, @heShe, @it, @we, @youPl, @they)";

       

            
        // Add entry to master table
        public void MasterStore(string fin, string eng)
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


        // Read given table contents, return data in dt
        public DataTable ReadTable(string table)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM "+ table +"", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);                   
                    sda.Fill(dt);
                }
                Console.WriteLine(table+" table read successfully");
            } catch (Exception e) { Console.WriteLine("Exception caught reading master: " + e.Message); }
            return dt;
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