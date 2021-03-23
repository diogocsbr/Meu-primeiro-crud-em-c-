using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection("Data Source=localhost;Initial Catalog=TesteTwitch;Persist Security Info = True;User ID=sa;Password=SqlM@st3r");

            comando.Connection = conexao;
            comando.Connection.Open();

            Random rdn = new Random();
            comando.CommandText = "insert into contatos ( id, nome, sobrenome ) values (@id, @nome, @sobrenome)";
            comando.CommandType = CommandType.Text;
            
            for (int i = 0; i < 1000; i++)
            {
                comando.Parameters.AddWithValue("@id", Guid.NewGuid());
                comando.Parameters.AddWithValue("@nome", $"Nome {rdn.Next()}");
                comando.Parameters.AddWithValue("@sobrenome", $"Sobreome {rdn.Next()}");
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
            }

            comando.Connection.Close();
            MessageBox.Show("Terminamos");
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection("Data Source=localhost;Initial Catalog=TesteTwitch;Persist Security Info = True;User ID=sa;Password=SqlM@st3r");

            comando.Connection = conexao;
            comando.Connection.Open();

            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "proc_leitura";

            SqlDataReader datareader = comando.ExecuteReader(CommandBehavior.CloseConnection);

            List<Contatos> lista = new List<Contatos>();
            Contatos contato;

            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    contato = new Contatos();

                    contato.Id = datareader.GetGuid(0);
                    contato.Nome = datareader.GetString(1);
                    contato.Sobrenome = datareader.GetString(2);

                    lista.Add(contato);
                }
            }

            datareader.NextResult();

            while (datareader.Read())
            {
                int quantidade = datareader.GetInt32(0);
                lblQuantidade.Text = quantidade.ToString();
            }

            //comando.Connection.Close();
            dataGridView1.DataSource = lista;
        }

        class Contatos
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            public string Sobrenome { get; set; }
        }
    }
}
