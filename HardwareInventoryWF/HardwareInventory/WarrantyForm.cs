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
    public partial class WarrantyForm : Form
    {

        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";

        public string SelectedAssetName { get; set; }
        public string SelectedSerial { get; set; }
        public string CurrentStatus { get; set; }

        public WarrantyForm()
        {
            InitializeComponent();
        }

        public void materialButton1_Click(object sender, EventArgs e)
        {
            // Validação: Nome do ativo e Serial não podem ser nulos
            if (!string.IsNullOrEmpty(SelectedAssetName) && !string.IsNullOrEmpty(SelectedSerial))
            {
                try
                {
                    string conn = connectionString;

                    // Query SQL para atualizar as datas
                    string query = @"
                    UPDATE CONSOLIDATED_HARDWARE_INV
                    SET WarrantyDateTo = @WarrantyDateTo,
                        WarrantyDateFrom = @WarrantyDateFrom
                    WHERE Name = @Name AND Serial = @Serial"; // Certifique-se de incluir a condição correta

                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Adiciona os parâmetros de nome e serial
                            command.Parameters.AddWithValue("@Name", SelectedAssetName);
                            command.Parameters.AddWithValue("@Serial", SelectedSerial);

                            // Adiciona as datas dos DateTimePickers como parâmetros
                            command.Parameters.AddWithValue("@WarrantyDateTo", dateTimePicker2.Value);
                            command.Parameters.AddWithValue("@WarrantyDateFrom", dateTimePicker1.Value);

                            // Executa a query
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Status atualizado com sucesso.");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Erro: Não foi possível atualizar o status.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao atualizar: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Nome do ativo ou Serial inválido.");
            }
        }


        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
