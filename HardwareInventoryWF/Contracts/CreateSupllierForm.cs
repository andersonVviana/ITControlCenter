using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardwareInventoryWF.Contracts
{
    public partial class CreateSupllierForm : Form
    {
        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_ITCONTROLCENTER;User Id=gateway;Password=2rD&2Bn?F4;";


        public CreateSupllierForm()
        {
            InitializeComponent();
            iconButton1.Click += iconButton1_Click;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            iconButton1.Enabled = false; // Desabilita o botão após o clique

            if (ValidarCampos())
            {
                if (FornecedorJaExiste(textBox2.Text))
                {
                    MessageBox.Show("Fornecedor com este CNPJ já está cadastrado.");
                    iconButton1.Enabled = true;
                }
                else
                {
                    InserirFornecedor();
                    MessageBox.Show("Fornecedor cadastrado com sucesso!");
                    this.Close(); // Fecha o formulário após salvar
                }
            }
            else
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                iconButton1.Enabled = true;
            }
        }

        private bool ValidarCampos()
        {
            return !string.IsNullOrWhiteSpace(textBox1.Text) &&
                   !string.IsNullOrWhiteSpace(textBox2.Text) &&
                   !string.IsNullOrWhiteSpace(textBox3.Text) &&
                   !string.IsNullOrWhiteSpace(textBox4.Text) &&
                   !string.IsNullOrWhiteSpace(textBox5.Text) &&
                   !string.IsNullOrWhiteSpace(textBox6.Text) &&
                   !string.IsNullOrWhiteSpace(textBox7.Text) &&
                   comboBox1.SelectedItem != null &&
                   comboBox3.SelectedItem != null;
        }

        private void InserirFornecedor()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                INSERT INTO CONTRACTSMASTER 
                (Supplier, CNPJ, Contact1, Contact2, Phone, Phone2, Email, Type, RelationType) 
                VALUES (@Supplier, @CNPJ, @Contact1, @Contact2, @Phone, @Phone2, @Email, @Type, @RelationType)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Supplier", textBox1.Text);
                cmd.Parameters.AddWithValue("@CNPJ", textBox2.Text);
                cmd.Parameters.AddWithValue("@Contact1", textBox3.Text);
                cmd.Parameters.AddWithValue("@Contact2", textBox4.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox5.Text);
                cmd.Parameters.AddWithValue("@Phone2", textBox6.Text);
                cmd.Parameters.AddWithValue("@Email", textBox7.Text);
                cmd.Parameters.AddWithValue("@Type", comboBox1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@RelationType", comboBox3.SelectedItem.ToString());

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private bool FornecedorJaExiste(string cnpj)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM CONTRACTSMASTER WHERE CNPJ = @CNPJ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CNPJ", cnpj);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();

                return count > 0; // Retorna true se o fornecedor já existir
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
