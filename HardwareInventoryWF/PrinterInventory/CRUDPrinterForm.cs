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

namespace HardwareInventoryWF.PrinterInventory
{
    public partial class CRUDPrinterForm : Form
    {
        public string CurrentStatus { get; set; }
        public string SelectedSerial { get; set; }
        public string SelectedAssetName { get; set; }

        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";

        private string serialNumber;

        public CRUDPrinterForm(string serialNumber)
        {
            
            this.serialNumber = serialNumber;

            // Carregar os dados da impressora no formulário
           
        }

        public CRUDPrinterForm()
        {

            InitializeComponent();

        }

        private void CRUDPrinterForm_Load(object sender, EventArgs e)
        {

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            comboBox1.Items.AddRange(new string[]
            {
            "Printer Mono",
            "Printer Color",
            "Label Printer"
            });

            comboBox2.Items.AddRange(new string[]
            {
            "DMA",
            "PLC"
            });

            comboBox3.Items.AddRange(new string[]
            {
            "G1 (DMA)",
            "G2 (DMA)",
            "G3 (DMA)",
            "G5 (DMA)",
            "G7 (DMA)",
            "G9 (DMA)",
            "G11 (DMA)",
            "PISO (PLC)",
            "SUPERIOR (PLC)",
            "GALPÃO (PLC)"
            });

            comboBox4.Items.AddRange(new string[]
            {
            "Brother",
            "Cannon",
            "Epson",
            "Zebra"
            });

            comboBox5.Items.AddRange(new string[]
            {
            "Active",
            "In Maintenance",
            "Available",
            "Under Review"
            });

            // Se o status atual estiver disponível, selecione-o no ComboBox
            if (!string.IsNullOrEmpty(CurrentStatus))
            {
                comboBox1.SelectedItem = CurrentStatus;
                comboBox2.SelectedItem = CurrentStatus;
                comboBox3.SelectedItem = CurrentStatus;
                comboBox4.SelectedItem = CurrentStatus;
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

            // String de conexão com o banco (ajuste conforme necessário)
            var conn = connectionString;

            // Query SQL de inserção
            string query = @"INSERT INTO PRINTER_INVENTORY 
                            (Type, Site, Name, Location, Model, IPAdress, SerialNumber, 
                             MacAdressWLan, MacAdressLan, Status, Manufacturer, WarrantyDateTo)
                             VALUES 
                            (@Type, @Site, @Name, @Location, @Model, @IPAdress, @SerialNumber, 
                             @MacAdressWLan, @MacAdressLan, @Status, @Manufacturer, @WarrantyDateTo)";

            try
            {
                // Conectando ao banco
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adicionando parâmetros para a query
                        command.Parameters.AddWithValue("@Type", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Site", comboBox2.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Name", textBox1.Text);
                        command.Parameters.AddWithValue("@Location", comboBox3.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Model", textBox2.Text);
                        command.Parameters.AddWithValue("@IPAdress", textBox3.Text);
                        command.Parameters.AddWithValue("@SerialNumber", textBox4.Text);
                        command.Parameters.AddWithValue("@MacAdressWLan", textBox5.Text);
                        command.Parameters.AddWithValue("@MacAdressLan", textBox6.Text);
                        command.Parameters.AddWithValue("@Status", comboBox5.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Manufacturer", comboBox4.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@WarrantyDateTo", dateTimePicker1.Value);

                        // Abrindo conexão e executando o comando
                        connection.Open();
                        command.ExecuteNonQuery();

                        // Mensagem de confirmação e fechamento do formulário
                        MessageBox.Show("Impressora cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        PrinterInventoryForm printerInventoryForm = new PrinterInventoryForm();
                        printerInventoryForm.CarregarImpressoras();

                        this.Close();


                    }
                }
            }
            catch (Exception ex)
            {
                // Exibindo mensagem de erro, caso ocorra
                MessageBox.Show($"Erro ao cadastrar impressora: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void iconButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
