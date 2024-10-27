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

namespace HardwareInventoryWF.HardwareInventory
{
    public partial class CreateHIForm : Form
    {

        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";

        public CreateHIForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // String de conexão com o banco (ajuste conforme necessário)
            var conn = connectionString;

            // Query SQL de inserção
            string query = @"INSERT INTO CONSOLIDATED_HARDWARE_INV 
                            (Name, OperatingSystem, Manufacturer, Model, Serial, ipv4Address, Status, RAM, Size)
                             VALUES 
                            (@Name, @OperatingSystem, @Manufacturer, @Model, @Serial, @ipv4Address, @Status, @RAM, @Size);";

            try
            {
                // Conectando ao banco
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adicionando parâmetros para a query
                        command.Parameters.AddWithValue("@OperatingSystem", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Manufacturer", comboBox2.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Name", textBox1.Text);
                        command.Parameters.AddWithValue("@Model", comboBox3.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Serial", textBox2.Text);
                        command.Parameters.AddWithValue("@ipv4Address", textBox3.Text);
                        command.Parameters.AddWithValue("@Status", comboBox4.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@RAM", textBox4.Text);
                        command.Parameters.AddWithValue("@Size", textBox5.Text);


                        // Abrindo conexão e executando o comando
                        connection.Open();
                        command.ExecuteNonQuery();

                        // Mensagem de confirmação e fechamento do formulário
                        MessageBox.Show("Equipment / VM successfully registered", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Exibindo mensagem de erro, caso ocorra
                MessageBox.Show($"Error registering equipment / VM: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
