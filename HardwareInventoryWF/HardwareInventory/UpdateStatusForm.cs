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
    public partial class UpdateStatusForm : Form
    {

        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";

        public string SelectedAssetName { get; set; }
        public string SelectedSerial { get; set; }

        public string CurrentStatus { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }

        public string Name { get; set; }


        public UpdateStatusForm()
        {
            InitializeComponent();
        }

        private void UpdateStatusForm_Load(object sender, EventArgs e)
        {
            // Adicionando as opções ao ComboBox
            comboBox1.Items.Clear(); // Certifique-se de limpar qualquer item antigo

            comboBox1.Items.AddRange(new string[]
            {
            "Atestado / Licença",
            "Ativo",
            "Descarte",
            "Disponível no TI",
            "Equipamento Não Encontrado",
            "Na Assistência Técnica",
            "Em Análise",
            "Hostname Desativado",
            "Servidor Desativado"
            });

            // Se o status atual estiver disponível, selecione-o no ComboBox
            if (!string.IsNullOrEmpty(CurrentStatus))
            {
                comboBox1.SelectedItem = CurrentStatus;
                comboBox2.SelectedItem = Manufacturer;
                comboBox4.SelectedItem = Model;
                textBox2.Text = Serial;

            }


        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            string newStatus = comboBox1.SelectedItem?.ToString();
            string newManufacturer = comboBox2.SelectedItem?.ToString();
            string newModel = comboBox4.SelectedItem?.ToString();

            if (newStatus != null || newManufacturer != null || newModel != null)
            {
                try
                {
                    var conn = connectionString;

                    // Query básica sem a data
                    string query = @"
                    UPDATE CONSOLIDATED_HARDWARE_INV
                    SET Status = @Status, 
                        Manufacturer = @Manufacturer,
                        Model = @Model,
                        Serial = @Serial
                    WHERE Name = @Name";

                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Status", (object)newStatus ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Manufacturer", (object)newManufacturer ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Model", (object)newModel ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Name", Name);
                            command.Parameters.AddWithValue("@Serial", Serial);

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
                MessageBox.Show("Selecione ao menos um campo.");
            }
        }



        private void materialButton2_Click(object sender, EventArgs e)
        {

            this.Close();
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedAssetName) && !string.IsNullOrEmpty(SelectedSerial))
            {
                try
                {
                    var conn = connectionString;

                    // Query para definir a coluna WarrantyDateTo como NULL
                    string query = @"
                    UPDATE CONSOLIDATED_HARDWARE_INV
                    SET WarrantyDateTo = NULL
                    WHERE Name = @Name";

                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Name", SelectedAssetName);
                            command.Parameters.AddWithValue("@Serial", SelectedSerial);

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data removida com sucesso.");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Erro: Não foi possível remover a data.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao remover a data: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Nome do ativo ou Serial inválido.");
            }
        }
    }
}
