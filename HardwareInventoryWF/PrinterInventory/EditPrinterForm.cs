using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HardwareInventoryWF.PrinterInventory
{
    public partial class EditPrinterForm : Form
    {
        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";
        private int printerID;

        public EditPrinterForm()
        {
        }

        public EditPrinterForm(int id)
        {
            InitializeComponent();
            printerID = id;
            CarregarDadosImpressoras();
        }

        public void CarregarDadosImpressoras()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT Type, Name, Site, Location, IPAdress, SerialNumber, 
                                MacAdressWLan, MacAdressLan, Status, WarrantyDateTo, 
                                Model, Manufacturer 
                         FROM PRINTER_INVENTORY WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = printerID; // Adicionar parâmetro corretamente

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    comboBox1.Text = reader["Type"].ToString();
                    textBox1.Text = reader["Name"].ToString();
                    comboBox2.Text = reader["Site"].ToString();
                    comboBox3.Text = reader["Location"].ToString();
                    textBox3.Text = reader["IPAdress"].ToString();
                    textBox4.Text = reader["SerialNumber"].ToString();
                    textBox5.Text = reader["MacAdressWLan"].ToString();
                    textBox6.Text = reader["MacAdressLan"].ToString();
                    comboBox5.Text = reader["Status"].ToString();
                    textBox2.Text = reader["Model"].ToString();
                    dateTimePicker1.Value = DateTime.Parse(reader["WarrantyDateTo"].ToString());
                    comboBox4.Text = reader["Manufacturer"].ToString();
                }
                else
                {
                    MessageBox.Show("Nenhuma impressora encontrada com o ID fornecido.");
                }
                reader.Close();
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE PRINTER_INVENTORY 
                             SET Type = @Type, Name = @Name, Site = @Site,
                                 Location = @Location, IPAdress = @IPAdress, SerialNumber = @SerialNumber, MacAdressWLan = @MacAdressWLan, 
                                 MacAdressLan = @MacAdressLan, Status = @Status, Model = @Model, Manufacturer = @Manufacturer
                             WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", printerID);
                cmd.Parameters.AddWithValue("@Type", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Site", comboBox2.Text);
                cmd.Parameters.AddWithValue("@Location", comboBox3.Text);
                cmd.Parameters.AddWithValue("@Manufacturer", comboBox4.Text);
                cmd.Parameters.AddWithValue("@IPAdress", textBox3.Text);
                cmd.Parameters.AddWithValue("@SerialNumber", textBox4.Text);
                cmd.Parameters.AddWithValue("@MacAdressWLan", textBox5.Text);
                cmd.Parameters.AddWithValue("@MacAdressLan", textBox6.Text);
                cmd.Parameters.AddWithValue("@Status", comboBox5.Text);
                //cmd.Parameters.AddWithValue("@WarrantyDateTo", dateTimePicker1.ToString());
                cmd.Parameters.AddWithValue("@Model", textBox2.Text);

                conn.Open();
                cmd.ExecuteNonQuery();

                
            }

            MessageBox.Show("Fornecedor atualizado com sucesso!");
            DialogResult = DialogResult.OK; // Fecha o formulário com sucesso

            this.Close();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
