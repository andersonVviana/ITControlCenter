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
    public partial class EditSupplierForm : Form
    {

        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_ITCONTROLCENTER;User Id=gateway;Password=2rD&2Bn?F4;";
        private int fornecedorID;

        public EditSupplierForm(int id)
        {
            InitializeComponent();
            fornecedorID = id;
            CarregarDadosFornecedor();
        }

        private void CarregarDadosFornecedor()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Supplier, Type, CNPJ, RelationType, Contact1, Contact2, Phone, Phone2, Email FROM CONTRACTSMASTER WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", fornecedorID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader["Supplier"].ToString();
                    textBox2.Text = reader["CNPJ"].ToString();
                    textBox3.Text = reader["Contact1"].ToString();
                    textBox4.Text = reader["Contact2"].ToString();
                    textBox5.Text = reader["Phone"].ToString();
                    textBox6.Text = reader["Phone2"].ToString();
                    textBox7.Text = reader["Email"].ToString();
                    comboBox1.Text = reader["Type"].ToString();
                    comboBox3.Text = reader["RelationType"].ToString();
                }
                reader.Close();
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE CONTRACTSMASTER 
                             SET Supplier = @Supplier, Type = @Type, CNPJ = @CNPJ,
                                 RelationType = @RelationType, Contact1 = @Contact1, Contact2 = @Contact2, Phone = @Phone, Phone2 = @Phone2, Email = @Email
                             WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", fornecedorID);
                cmd.Parameters.AddWithValue("@Supplier", textBox1.Text);
                cmd.Parameters.AddWithValue("@Type", comboBox1.Text);
                cmd.Parameters.AddWithValue("@CNPJ", textBox2.Text);
                cmd.Parameters.AddWithValue("@RelationType", comboBox3.Text);
                cmd.Parameters.AddWithValue("@Contact1", textBox3.Text);
                cmd.Parameters.AddWithValue("@Contact2", textBox4.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox5.Text);
                cmd.Parameters.AddWithValue("@Phone2", textBox6.Text);
                cmd.Parameters.AddWithValue("@Email", textBox7.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Fornecedor atualizado com sucesso!");
            DialogResult = DialogResult.OK; // Fecha o formulário com sucesso
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
