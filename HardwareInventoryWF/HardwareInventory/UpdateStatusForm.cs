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
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string newStatus = comboBox1.SelectedItem.ToString();

                // Validação: Nome do ativo e Serial não podem ser nulos
                if (!string.IsNullOrEmpty(SelectedAssetName) && !string.IsNullOrEmpty(SelectedSerial))
                {
                    try
                    {
                        var conn = connectionString;

                        // Query básica sem a data
                        string query = @"
                        UPDATE CONSOLIDATED_HARDWARE_INV
                        SET Status = @Status";

                        // Verifica se o CheckBox foi marcado para incluir a data na query
                        if (checkBox1.Checked)
                        {
                            query += ", WarrantyDateTo = @WarrantyDateTo";
                        }

                        query += " WHERE Name = @Name AND Serial = @Serial";

                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Adiciona os parâmetros obrigatórios
                                command.Parameters.AddWithValue("@Status", newStatus);
                                command.Parameters.AddWithValue("@Name", SelectedAssetName);
                                command.Parameters.AddWithValue("@Serial", SelectedSerial);

                                // Adiciona a data apenas se o CheckBox estiver marcado
                                if (checkBox1.Checked)
                                {
                                    DateTime selectedDate = dateTimePicker1.Value;
                                    command.Parameters.AddWithValue("@WarrantyDateTo", selectedDate);
                                }

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
            else
            {
                MessageBox.Show("Selecione um status.");
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
                    WHERE Name = @Name AND Serial = @Serial";

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
                                dateTimePicker1.Checked = false; // Desmarcar o DateTimePicker
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
