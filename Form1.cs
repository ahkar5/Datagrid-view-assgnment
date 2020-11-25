using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace gviewassig
{
    public partial class Form1 : Form
    {
        static string conString = "Server=localhost;Database=Moviedb;Uid=root;Pwd=;";
        MySqlConnection con = new MySqlConnection(conString);
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Date";
            dataGridView1.Columns[2].Name = "Rating";
            dataGridView1.Columns[3].Name = "Director";
            dataGridView1.Columns[4].Name = "Actor";
            dataGridView1.Columns[5].Name = "Actress";
            dataGridView1.Columns[6].Name = "Price";

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //Selection Mode
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

        }
        //insert into db
        private void add(string Name, string Date, string Rating, string Director, string Actor, string Actress, string Price)
        {
            string sql = "INSERT INTO MovieTB(Name,Date,Rating,Director,Actor,Actress,Price) VALUES(@Name,@Date,@Rating,@Director,@Actor,@Actress,@Price)";
            cmd = new MySqlCommand(sql, con);
            //Open Con and EXEC Insert
            try
            {
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    clearTXTs();
                    MessageBox.Show("success");
                }
                con.Close();
                retrieve();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
       
       //add ro dgview
       private void populate(string Name,string Date,string Rating,string Director,string Actor,string Actress,string Price)
        {
            dataGridView1.Rows.Add(Name, Date, Rating, Director, Actor, Actress, Price);
        }
        
        //retrieve from db
        private void retrieve()
        {
            dataGridView1.Rows.Clear();
            //Sql STMT
            string sql = "SELECT * FROM MovieTB";
            cmd = new MySqlCommand(sql, con);
            //open con,retrieve,fill,dgview
            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                //loop thur dt
                foreach(DataRow row in dt.Rows)
                {
                    populate(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString());
                }
                con.Close();

                //clear dt
                dt.Rows.Clear();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //Update db
        private void update(string Name,string Date,string Rating,string Director,string Actor,string Actress,string Price)
        {
            //sql stmt
            string sql = "Update MovieTB SET Name='" + Name + "',Date='" + Date + "',Rating='" + Rating + "',Director='" + Director + "',Actor'" + Actor + "',Actress'" + Actress + "',Price'" + Price + "'";
            cmd = new MySqlCommand(sql, con);
            //open con,update,retrieve, dgview
            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;
                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    clearTXTs();
                    MessageBox.Show("success");

                }
                con.Close();
                retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
        //delete from db
        private void delete(string Name)
        {
            //sqlstmt
            string sql = "DELETE FROM propleTB Where Name=" + Name + "";
            cmd = new MySqlCommand(sql, con);
            //open con,ececute delete,close con
            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.DeleteCommand = con.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;
                //promt for confimation
                if (MessageBox.Show("sure", "delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        clearTXTs();
                        MessageBox.Show("success");
                    }
                }

                con.Close();
                retrieve();
            }
        }
        //clear txtx
        private void clearTXTs()
        {
            NameTxt.Text = "";
            DateTXT.Text = "";
            RatingTXT.Text = "";
            DirectorTXT.Text = "";
            ActorTXT.Text = "";
            ActressTXT.Text = "";
            PriceTXT.Text = "";

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            NameTxt.Text =dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            DateTXT.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            RatingTXT.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            DirectorTXT.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            ActorTXT.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            ActressTXT.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            PriceTXT.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            add(NameTxt.Text, DateTXT.Text, RatingTxt.Text, DirectorTxt.Text, ActorTxt.Text, ActressTxt.Text, PriceTxt.Text);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            retrieve();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            update(NameTxt.Text, DateTXT.Text, RatingTxt.Text, DirectorTxt.Text, ActorTxt.Text, ActressTxt.Text, PriceTxt.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            delete(id);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
