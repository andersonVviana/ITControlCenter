using HardwareInventoryWF.HardwareInventory;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using HardwareInventoryWF.PrinterInventory;
using HardwareInventoryWF.Contracts;

namespace HardwareInventoryWF
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        //Zabbix DMA
        private const string zabbixUrlDma = "http://10.1.0.55/api_jsonrpc.php"; // Altere para o URL do seu Zabbix
        private const string usernameDma = "Admin"; // Altere para seu usuário
        private const string passwordDma = "2rD&2Bn?F4";
        private const string hostId = "10946"; // LINK NEOVIA PLC
        private const string hostId2 = "10947"; // LINK VIVO PLC
        private const string itemKeyDma = "icmpping"; // Chave do item ICMP Ping

        private static readonly HttpClient client = new HttpClient();
        private string apiUrl = "http://10.2.0.107/zabbix//api_jsonrpc.php";
        private string authToken;

        private Timer updateTimer;

        public Form1()
        {
            InitializeComponent();

            // Inicializa o Timer
            updateTimer = new Timer();
            updateTimer.Interval = 60000; // 1 minuto
            updateTimer.Tick += UpdateTimer_Tick; // Adiciona o manipulador de eventos
            updateTimer.Start(); // Inicia o Timer

            // Defina o tamanho dos painéis
            panel13.Size = new Size(20, 20);
            panel14.Size = new Size(20, 20);
            panel15.Size = new Size(20, 20);
            panel16.Size = new Size(20, 20);

            _ = InitializeHostStatus();

            //label9 = new Label { Location = new System.Drawing.Point(10, 10), AutoSize = true };
            //this.Controls.Add(label9);
            this.Load += Form1_Load;

            panel2.Paint += new PaintEventHandler(RoundPanel2Corners);
            panel3.Paint += new PaintEventHandler(RoundPanel3Corners);
            panel4.Paint += new PaintEventHandler(RoundPanel4Corners);
            panel5.Paint += new PaintEventHandler(RoundPanel5Corners);
            panel6.Paint += new PaintEventHandler(RoundPanel6Corners);
            panel7.Paint += new PaintEventHandler(RoundPanel7Corners);
            panel8.Paint += new PaintEventHandler(RoundPanel8Corners);
            //panel9.Paint += new PaintEventHandler(RoundPanel9Corners);

            LoadDataOperatingSystem();
            LoadDataManufacter();
            LoadDataType();
            LoadDataServers();
            LoadDataAvailable();
            LoadDataWarranty();
            LoadDataPrinters();
        }

        #region RoundPanel
        private void RoundPanel2Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel2.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel2.Width - radius, panel2.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel2.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel2.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void RoundPanel3Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel3.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel3.Width - radius, panel3.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel3.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel3.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void RoundPanel4Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel4.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel4.Width - radius, panel4.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel4.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel4.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void RoundPanel5Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel5.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel5.Width - radius, panel5.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel5.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel5.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void RoundPanel6Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel6.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel6.Width - radius, panel6.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel6.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel6.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }
        private void RoundPanel7Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel7.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel7.Width - radius, panel7.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel7.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel7.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void RoundPanel8Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel8.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel8.Width - radius, panel8.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel8.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel8.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void RoundPanel9Corners(object sender, PaintEventArgs e)
        {
            int radius = 30; // Defina o raio para os cantos arredondados

            // Cria um caminho gráfico com os cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(panel9.Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(panel9.Width - radius, panel9.Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, panel9.Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com os cantos arredondados ao Panel2
            panel9.Region = new Region(path);

            // Opcional: Desenhar uma borda ao redor do painel
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        #endregion


        private void LoadDataOperatingSystem()
        {
            var conn = connectionString;
            

            // SQL para obter a quantidade de equipamentos por sistema operacional
            string query = @"
            SELECT 
                OperatingSystem, 
                COUNT(*) AS Qty
            FROM CONSOLIDATED_HARDWARE_INV
            WHERE (Name LIKE 'ABNB%' OR Name LIKE 'ABWS%' OR Name LIKE 'ABTC%')
              AND (Status = 'Ativo' OR Status = 'Disponível no TI' OR Status = 'Na Assistência Técnica' OR Status = 'Atestado / Licença' OR Status = 'Em Análise')
            GROUP BY OperatingSystem
            ORDER BY Qty DESC;";

            // Criar a conexão e o comando SQL
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    // Abrir a conexão
                    connection.Open();

                    // Executar o comando e obter os dados
                    SqlDataReader reader = command.ExecuteReader();

                    // Limpar séries do gráfico antes de adicionar novos dados
                    chart2.Series.Clear();
                    Series series = new Series
                    {
                        Name = "",
                        ChartType = SeriesChartType.Column,
                        IsValueShownAsLabel = true,
                        LabelForeColor = Color.Black,
                        Color = Color.FromArgb(82, 37, 131)

                    };

                    // Adicionar dados ao gráfico
                    while (reader.Read())
                    {
                        string sistemaOperacional = reader["OperatingSystem"].ToString();
                        int quantidade = Convert.ToInt32(reader["Qty"]);
                        series.Points.AddXY(sistemaOperacional, quantidade);
                    }

                    // Adicionar a série ao gráfico
                    chart2.Series.Add(series);

                    ChartArea chartArea = chart2.ChartAreas[0];
                    chartArea.AxisX.MajorGrid.LineWidth = 0; // Remove as linhas verticais
                    chartArea.AxisY.MajorGrid.LineColor = Color.Gray; // Cor das linhas horizontais
                    chartArea.AxisY.MajorGrid.LineWidth = 1; // Largura das linhas horizontais
                    chartArea.AxisX.MajorGrid.Enabled = false; // Desativa as linhas verticais (grade principal)
                    chartArea.AxisY.MajorGrid.Enabled = true;

                    chart2.Legends.Clear();

                    // Fechar o leitor de dados
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                }
            }
        }

        private void LoadDataPrinters()
        {
            var conn = connectionString;



            // SQL para obter a quantidade de equipamentos por sistema operacional
            string query = @"
            select
	            status,
	            COUNT(*) as Qty
            from 
            PRINTER_INVENTORY
            GROUP BY Status";

            // Criar a conexão e o comando SQL
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    // Abrir a conexão
                    connection.Open();

                    // Executar o comando e obter os dados
                    SqlDataReader reader = command.ExecuteReader();

                    // Limpar séries do gráfico antes de adicionar novos dados
                    chart6.Series.Clear();
                    Series series = new Series
                    {
                        Name = "",
                        ChartType = SeriesChartType.Column,
                        IsValueShownAsLabel = true,
                        LabelForeColor = Color.Black,
                        Color = Color.FromArgb(82, 37, 131)

                    };

                    int totalEquipamentos = 0;

                    // Adicionar dados ao gráfico
                    while (reader.Read())
                    {
                        string sistemaOperacional = reader["status"].ToString();
                        int quantidade = Convert.ToInt32(reader["Qty"]);
                        series.Points.AddXY(sistemaOperacional, quantidade);

                        totalEquipamentos += quantidade;
                    }

                    // Adicionar a série ao gráfico
                    chart6.Series.Add(series);

                    ChartArea chartArea = chart6.ChartAreas[0];
                    chartArea.AxisX.MajorGrid.LineWidth = 0; // Remove as linhas verticais
                    chartArea.AxisY.MajorGrid.LineColor = Color.Gray; // Cor das linhas horizontais
                    chartArea.AxisY.MajorGrid.LineWidth = 1; // Largura das linhas horizontais
                    chartArea.AxisX.MajorGrid.Enabled = false; // Desativa as linhas verticais (grade principal)
                    chartArea.AxisY.MajorGrid.Enabled = true;

                    chart6.Legends.Clear();

                    label18.Text = "Total: " + totalEquipamentos;

                    // Fechar o leitor de dados
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                }
            }
        }

        private void LoadDataManufacter()
        {
            var conn = connectionString;


            // SQL para obter a quantidade de equipamentos por sistema operacional
            string query = @"
            SELECT 
                Manufacturer, 
                COUNT(DISTINCT Name) AS Qty
            FROM CONSOLIDATED_HARDWARE_INV
            WHERE (Name LIKE 'ABNB%' OR Name LIKE 'ABWS%' OR Name LIKE 'ABTC%')
              AND (Status = 'Ativo' OR Status = 'Disponível no TI' OR Status = 'Na Assistência Técnica' OR Status = 'Atestado / Licença' OR Status = 'Em Análise')
            GROUP BY Manufacturer
            ORDER BY Qty DESC;
            ";

            // Criar a conexão e o comando SQL
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    // Abrir a conexão
                    connection.Open();

                    // Executar o comando e obter os dados
                    SqlDataReader reader = command.ExecuteReader();

                    // Limpar séries do gráfico antes de adicionar novos dados
                    chart1.Series.Clear();
                    Series series = new Series
                    {
                        Name = "",
                        ChartType = SeriesChartType.Column,
                        IsValueShownAsLabel = true,
                        LabelForeColor = Color.Black,
                        Color = Color.FromArgb(82, 37, 131)

                    };

                    // Adicionar dados ao gráfico
                    while (reader.Read())
                    {
                        string sistemaOperacional = reader["Manufacturer"].ToString();
                        int quantidade = Convert.ToInt32(reader["Qty"]);
                        series.Points.AddXY(sistemaOperacional, quantidade);
                    }

                    // Adicionar a série ao gráfico
                    chart1.Series.Add(series);

                    ChartArea chartArea = chart1.ChartAreas[0];
                    chartArea.AxisX.MajorGrid.LineWidth = 0; // Remove as linhas verticais
                    chartArea.AxisY.MajorGrid.LineColor = Color.Gray; // Cor das linhas horizontais
                    chartArea.AxisY.MajorGrid.LineWidth = 1; // Largura das linhas horizontais
                    chartArea.AxisX.MajorGrid.Enabled = false; // Desativa as linhas verticais (grade principal)
                    chartArea.AxisY.MajorGrid.Enabled = true;

                    chart1.Legends.Clear();

                    // Fechar o leitor de dados
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                }
            }
        }

        private void LoadDataType()
        {
            var conn = connectionString;


            // SQL para obter a quantidade de equipamentos por sistema operacional
            string query = @"
            SELECT 
                CASE
                    WHEN Name LIKE 'ABNB%' THEN 'Laptop'
                    WHEN Name LIKE 'ABWS%' THEN 'Desktop'
                    WHEN Name LIKE 'ABTC%' THEN 'ThinClient'
                END AS Category,
                COUNT(DISTINCT Name) AS Qty
            FROM CONSOLIDATED_HARDWARE_INV
            WHERE (Name LIKE 'ABNB%' OR Name LIKE 'ABWS%' OR Name LIKE 'ABTC%')
              AND (Status = 'Ativo' OR Status = 'Disponível no TI' OR Status = 'Na Assistência Técnica' OR Status = 'Atestado / Licença' OR Status = 'Em Análise')
            GROUP BY 
                CASE
                    WHEN Name LIKE 'ABNB%' THEN 'Laptop'
                    WHEN Name LIKE 'ABWS%' THEN 'Desktop'
                    WHEN Name LIKE 'ABTC%' THEN 'ThinClient'
                END
            ORDER BY Qty DESC;
            ";

            // Criar a conexão e o comando SQL
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    // Abrir a conexão
                    connection.Open();

                    // Executar o comando e obter os dados
                    SqlDataReader reader = command.ExecuteReader();

                    // Limpar séries do gráfico antes de adicionar novos dados
                    chart4.Series.Clear();
                    Series series = new Series
                    {
                        Name = "",
                        ChartType = SeriesChartType.Column,
                        IsValueShownAsLabel = true,
                        LabelForeColor = Color.Black,
                        Color = Color.FromArgb(82, 37, 131)

                    };

                    // Adicionar dados ao gráfico
                    while (reader.Read())
                    {
                        string sistemaOperacional = reader["Category"].ToString();
                        int quantidade = Convert.ToInt32(reader["Qty"]);
                        series.Points.AddXY(sistemaOperacional, quantidade);
                    }

                    // Adicionar a série ao gráfico
                    chart4.Series.Add(series);

                    ChartArea chartArea = chart4.ChartAreas[0];
                    chartArea.AxisX.MajorGrid.LineWidth = 0; // Remove as linhas verticais
                    chartArea.AxisY.MajorGrid.LineColor = Color.Gray; // Cor das linhas horizontais
                    chartArea.AxisY.MajorGrid.LineWidth = 1; // Largura das linhas horizontais
                    chartArea.AxisX.MajorGrid.Enabled = false; // Desativa as linhas verticais (grade principal)
                    chartArea.AxisY.MajorGrid.Enabled = true;

                    chart4.Legends.Clear();

                    // Fechar o leitor de dados
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                }
            }
        }

        private void LoadDataServers()
        {
            var conn = connectionString;

            // SQL para obter a quantidade de equipamentos por sistema operacional
            string query = @"
            SELECT 
                OperatingSystem, 
                COUNT(DISTINCT Name) AS Qty
            FROM CONSOLIDATED_HARDWARE_INV
            WHERE Name LIKE 'ARCADE%'
              AND (Status = 'Ativo' OR Status = 'Disponível no TI' OR Status = 'Na Assistência Técnica' OR Status = 'Atestado / Licença' OR Status = 'Em Análise')
            GROUP BY OperatingSystem
            ORDER BY Qty DESC;
            ";

            // Criar a conexão e o comando SQL
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    // Abrir a conexão
                    connection.Open();

                    // Executar o comando e obter os dados
                    SqlDataReader reader = command.ExecuteReader();

                    // Limpar séries do gráfico antes de adicionar novos dados
                    chart3.Series.Clear();
                    Series series = new Series
                    {
                        Name = "",
                        ChartType = SeriesChartType.Column,
                        IsValueShownAsLabel = true,
                        LabelForeColor = Color.Black,
                        Color = Color.FromArgb(82, 37, 131)
                    };

                    int totalEquipamentos = 0; // Variável para armazenar o total de equipamentos

                    // Adicionar dados ao gráfico
                    while (reader.Read())
                    {
                        string sistemaOperacional = reader["OperatingSystem"].ToString();
                        int quantidade = Convert.ToInt32(reader["Qty"]);
                        series.Points.AddXY(sistemaOperacional, quantidade);

                        totalEquipamentos += quantidade; // Acumular a quantidade total
                    }

                    // Adicionar a série ao gráfico
                    chart3.Series.Add(series);

                    ChartArea chartArea = chart3.ChartAreas[0];
                    chartArea.AxisX.MajorGrid.LineWidth = 0; // Remove as linhas verticais
                    chartArea.AxisY.MajorGrid.LineColor = Color.Gray; // Cor das linhas horizontais
                    chartArea.AxisY.MajorGrid.LineWidth = 1; // Largura das linhas horizontais
                    chartArea.AxisX.MajorGrid.Enabled = false; // Desativa as linhas verticais (grade principal)
                    chartArea.AxisY.MajorGrid.Enabled = true;

                    chart3.Legends.Clear();

                    // Atualizar o label com o total de equipamentos
                    label17.Text = "Total: " + totalEquipamentos;

                    // Fechar o leitor de dados
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                }
            }
        }


        private void LoadDataWarranty()
        {
            var conn = connectionString;

            string query = @"
            SELECT 
                WarrantyDateTo, 
                COUNT(DISTINCT Name) AS Qty
            FROM CONSOLIDATED_HARDWARE_INV
            WHERE (Name LIKE 'ABNB%' OR Name LIKE 'ABWS%' OR Name LIKE 'ABTC%')
              AND WarrantyDateTo IS NOT NULL
              AND (Status = 'Ativo' OR Status = 'Disponível no TI' OR Status = 'Na Assistência Técnica' OR Status = 'Atestado / Licença' OR Status = 'Em Análise')
            GROUP BY WarrantyDateTo
            ORDER BY Qty DESC;
    ";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                int novos = 0, meiaVida = 0, fimDeVida = 0, obsoleto = 0;

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    chart5.Series.Clear();
                    Series series = new Series
                    {
                        Name = "Equipment Warranty",
                        ChartType = SeriesChartType.Pie,
                        IsValueShownAsLabel = true,  // Mostrar rótulos nas fatias
                        LabelForeColor = Color.White
                    };

                    while (reader.Read())
                    {
                        DateTime warrantyDate = reader.GetDateTime(0);
                        int anosDeUso = DateTime.Now.Year - warrantyDate.Year;

                        if (anosDeUso <= 2)
                            novos += reader.GetInt32(1);
                        else if (anosDeUso <= 4)
                            meiaVida += reader.GetInt32(1);
                        else if (anosDeUso <= 6)
                            fimDeVida += reader.GetInt32(1);
                        else
                            obsoleto += reader.GetInt32(1);
                    }

                    int total = novos + meiaVida + fimDeVida + obsoleto;

                    // Função para criar rótulo com número e porcentagem
                    string FormatLabel(int qty) =>
                        $"{qty} ({(qty / (double)total * 100):0.0}%)";

                    // Adicionar pontos com rótulos personalizados e nomes na legenda
                    if (novos > 0)
                    {
                        var point = series.Points.Add(novos);
                        point.AxisLabel = "New Equipment";
                        point.Label = FormatLabel(novos);
                        point.LegendText = "New Equipment";  // Definir nome na legenda
                        point.Color = Color.FromArgb(129, 41, 144);
                    }

                    if (meiaVida > 0)
                    {
                        var point = series.Points.Add(meiaVida);
                        point.AxisLabel = "Mid-life Equipment";
                        point.Label = FormatLabel(meiaVida);
                        point.LegendText = "Mid-life Equipment";  // Definir nome na legenda
                        point.Color = Color.FromArgb(153, 50, 204);
                    }

                    if (fimDeVida > 0)
                    {
                        var point = series.Points.Add(fimDeVida);
                        point.AxisLabel = "End of Life";
                        point.Label = FormatLabel(fimDeVida);
                        point.LegendText = "End of Life";  // Definir nome na legenda
                        point.Color = Color.FromArgb(178, 99, 255);
                    }

                    if (obsoleto > 0)
                    {
                        var point = series.Points.Add(obsoleto);
                        point.AxisLabel = "Obsolete";
                        point.Label = FormatLabel(obsoleto);
                        point.LegendText = "Obsolete";  // Definir nome na legenda
                        point.Color = Color.FromArgb(137, 62, 255);
                    }

                    // Adicionar a série ao gráfico
                    chart5.Series.Add(series);

                    label13.Text = $"Total: {total}";
                    label14.Text = $"Total: {total}";
                    label15.Text = $"Total: {total}";
                    label16.Text = $"Total: {total}";

                    // Configuração da área do gráfico
                    ChartArea chartArea = chart5.ChartAreas[0];
                    chartArea.AxisX.MajorGrid.LineWidth = 0;
                    chartArea.AxisY.MajorGrid.LineColor = Color.Gray;
                    chartArea.AxisY.MajorGrid.LineWidth = 1;
                    chartArea.AxisX.MajorGrid.Enabled = false;
                    chartArea.AxisY.MajorGrid.Enabled = true;

                    // Ativar a legenda do gráfico
                    chart5.Legends.Clear();
                    Legend legend = new Legend
                    {
                        Docking = Docking.Right,
                        Alignment = StringAlignment.Center,
                        LegendStyle = LegendStyle.Table,
                        TableStyle = LegendTableStyle.Tall,
                        IsTextAutoFit = true
                    };
                    chart5.Legends.Add(legend);

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                }
            }
        }


        private void LoadDataAvailable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Substitua sua consulta SQL abaixo
                    string query = @"
                    WITH LatestInventory AS
                    (
                        SELECT 
                            Name,
							Manufacturer,
                            Model,
                            ROW_NUMBER() OVER (PARTITION BY Name, Serial ORDER BY TraceDateTime DESC) AS RowNum
                        FROM CONSOLIDATED_HARDWARE_INV
                        WHERE (Name IS NOT NULL AND Name <> '') AND (Status = 'Ativo' OR Status = 'Disponível no TI' OR Status = 'Na Assistência Técnica' OR Status = 'Atestado / Licença' OR Status = 'Em Análise')
                    )
                    SELECT 
                        Name,
						Manufacturer,
                        Model
                    FROM LatestInventory
                    WHERE RowNum = 1
                    ORDER BY Name DESC
					";  // Insira sua consulta SQL aqui

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Define a fonte de dados do DataGridView
                        //dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        

        // Obtem todos os hosts
        private async Task<JToken> GetAllHosts(string authToken)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "host.get",
                @params = new
                {
                    output = new[] { "hostid", "name" }
                },
                auth = authToken,
                id = 2
            };

            var response = await SendPostRequest(payload);
            return response?["result"];
        }


        // Inicializa o status do host específico
        private async Task InitializeHostStatus()
        {
            try
            {
                var authToken = await GetAuthToken();
                if (string.IsNullOrEmpty(authToken))
                {
                    MessageBox.Show("Erro: Não foi possível obter o token de autenticação.");
                    return;
                }

                // Obtém o status e o IP do host 1
                var (status1, ip1) = await GetHostStatus(authToken, hostId);
                label11.Text = $"Secondary Link PLC : {ip1}";

                // Define a cor do painel de acordo com o status
                panel13.BackColor = status1 == "1" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                MakeCircular(panel13); // Se desejar que o painel tenha formato circular

                // Obtém o status e o IP do host 2
                var (status2, ip2) = await GetHostStatus(authToken, hostId2);
                label10.Text = $"Primary Link PLC : {ip2}";

                // Define a cor do painel de acordo com o status
                panel14.BackColor = status2 == "1" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                MakeCircular(panel14); // Se desejar que o painel tenha formato circular
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao obter status dos hosts: {ex.Message}");
            }
        }

        // Consulta o status do host específico
        private async Task<(string Status, string IP)> GetHostStatus(string authToken, string hostId)
        {
            var itemId = await GetItemId(authToken, hostId);
            if (string.IsNullOrEmpty(itemId))
            {
                MessageBox.Show($"Erro: O item com chave '{itemKeyDma}' não foi encontrado para o host ID {hostId}.");
                return (null, null);
            }

            var latestValue = await GetLatestItemValue(authToken, itemId);
            var ip = await GetHostIP(authToken, hostId); // Obtém o IP do host
            return (latestValue, ip);
        }

        // Obtem o ID do item usando a chave
        private async Task<string> GetItemId(string authToken, string hostId)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "item.get",
                @params = new
                {
                    output = new[] { "itemid" },
                    hostids = hostId,
                    search = new { key_ = itemKeyDma }
                },
                auth = authToken,
                id = 3
            };

            var response = await SendPostRequest(payload);
            var item = response?["result"]?.FirstOrDefault();

            return item?["itemid"]?.ToString();
        }

        // Obtem o último valor do item
        private async Task<string> GetLatestItemValue(string authToken, string itemId)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "item.get",
                @params = new
                {
                    output = new[] { "lastvalue" },
                    itemids = itemId
                },
                auth = authToken,
                id = 4
            };

            var response = await SendPostRequest(payload);
            var item = response?["result"]?.FirstOrDefault();

            return item?["lastvalue"]?.ToString();
        }

        private async Task<string> GetHostIP(string authToken, string hostId)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "host.get",
                @params = new
                {
                    output = new[] { "hostid", "name", "interfaces" },
                    hostids = hostId,
                    selectInterfaces = new[] { "ip" }
                },
                auth = authToken,
                id = 5
            };

            var response = await SendPostRequest(payload);
            var host = response?["result"]?.FirstOrDefault();
            return host?["interfaces"]?[0]?["ip"]?.ToString(); // Obtém o primeiro IP do host
        }

        public async Task<string> GetHostIPZabbixPLC(string hostId)
        {
            var requestData = new
            {
                jsonrpc = "2.0",
                method = "hostinterface.get",
                @params = new
                {
                    hostids = hostId,
                    output = new[] { "ip" }
                },
                auth = authToken,
                id = 1
            };

            var content = new StringContent(JObject.FromObject(requestData).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            if (jsonResponse["result"] != null && jsonResponse["result"].Any())
            {
                var interfaceInfo = jsonResponse["result"].First;
                return interfaceInfo["ip"]?.ToString();
            }
            else if (jsonResponse["error"] != null)
            {
                throw new Exception("Erro ao obter o IP do host: " + jsonResponse["error"]["data"]);
            }
            else
            {
                throw new Exception("Resposta inesperada da API.");
            }
        }


        // Método para obter o token de autenticação
        private async Task<string> GetAuthToken()
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "user.login",
                @params = new { user = usernameDma, password = passwordDma },
                id = 1
            };

            var response = await SendPostRequest(payload);

            if (response?["error"] != null)
            {
                MessageBox.Show($"Erro de autenticação: {response["error"]["message"]} - {response["error"]["data"]}");
                return null;
            }

            return response["result"]?.ToString();
        }

        // Envia uma requisição POST para a API Zabbix
        private async Task<JObject> SendPostRequest(object payload)
        {
            using (var client = new HttpClient())
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(zabbixUrlDma, content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Erro: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseContent);
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            InitializeHostStatus();
            UpdatePingStatusAndIP();
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        // Método para obter o token de autenticação do segundo Zabbix
        private async Task<string> GetAuthTokenFromOtherZabbix()
        {
            var request = new
            {
                jsonrpc = "2.0",
                method = "user.login",
                @params = new
                {
                    user = "WFApp", // Altere se necessário
                    password = "2rD&2Bn?F4" // Altere para a senha correta
                },
                id = 1
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://10.2.0.107/zabbix/api_jsonrpc.php"); // Ajuste a URL

                try
                {
                    var jsonRequest = JsonConvert.SerializeObject(request);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("", content);
                    response.EnsureSuccessStatusCode(); // Verifica se houve erro na requisição

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ZabbixResponse<string>>(jsonResponse);

                    if (string.IsNullOrEmpty(result.Result))
                        throw new Exception("Token de autenticação está vazio.");

                    return result.Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao obter token: {ex.Message}");
                    return null;
                }
            }
        }

        // Estrutura da classe Host
        public class Host
        {
            public string HostId { get; set; }
            public string Name { get; set; }
        }

        // Estrutura da resposta da API
        public class ZabbixResponse<T>
        {
            public T Result { get; set; }
        }



        private void materialButton2_Click(object sender, EventArgs e)
        {
            HardwareInventoryForm hardwareInventoryForm = new HardwareInventoryForm();

            this.Hide();

            hardwareInventoryForm.FormClosed += (s, args) => this.Show();

            hardwareInventoryForm.Show();


        }

        public async Task AuthenticateZabbixPLC(string username, string password)
        {
            var authData = new
            {
                jsonrpc = "2.0",
                method = "user.login",
                @params = new
                {
                    username = username,
                    password = password
                },
                id = 1
            };

            var content = new StringContent(JObject.FromObject(authData).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            if (jsonResponse["result"] != null)
            {
                authToken = jsonResponse["result"].ToString();
            }
            else if (jsonResponse["error"] != null)
            {
                throw new Exception("Erro de autenticação: " + jsonResponse["error"]["data"]);
            }
            else
            {
                throw new Exception("Resposta inesperada da API.");
            }
        }

        public async Task<JArray> GetHosts()
        {
            var requestData = new
            {
                jsonrpc = "2.0",
                method = "host.get",
                @params = new
                {
                    output = new[] { "hostid", "name" }
                },
                auth = authToken,
                id = 1
            };

            var content = new StringContent(JObject.FromObject(requestData).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            if (jsonResponse["result"] != null)
            {
                return (JArray)jsonResponse["result"];
            }
            else if (jsonResponse["error"] != null)
            {
                throw new Exception("Erro ao obter os hosts: " + jsonResponse["error"]["data"]);
            }
            else
            {
                throw new Exception("Resposta inesperada da API.");
            }
        }

        public async Task<JArray> GetHostsZabbixDma()
        {
            var requestData = new
            {
                jsonrpc = "2.0",
                method = "host.get",
                @params = new
                {
                    output = new[] { "hostid", "name" }
                },
                auth = authToken,
                id = 1
            };

            var content = new StringContent(JObject.FromObject(requestData).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            if (jsonResponse["result"] != null)
            {
                return (JArray)jsonResponse["result"];
            }
            else if (jsonResponse["error"] != null)
            {
                throw new Exception("Erro ao obter os hosts: " + jsonResponse["error"]["data"]);
            }
            else
            {
                throw new Exception("Resposta inesperada da API.");
            }
        }

        public async Task<string> GetICMPPingStatus(string hostId)
        {
            var requestData = new
            {
                jsonrpc = "2.0",
                method = "item.get",
                @params = new
                {
                    hostids = hostId,
                    search = new { key_ = "icmpping" },
                    output = new[] { "lastvalue" }
                },
                auth = authToken,
                id = 1
            };

            var content = new StringContent(JObject.FromObject(requestData).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            if (jsonResponse["result"] != null && jsonResponse["result"].Any())
            {
                var item = jsonResponse["result"].First;
                return item["lastvalue"]?.ToString();
            }
            else if (jsonResponse["error"] != null)
            {
                throw new Exception("Erro ao obter o status do ICMP Ping: " + jsonResponse["error"]["data"]);
            }
            else
            {
                throw new Exception("Resposta inesperada da API.");
            }
        }



        private void MakeCircular(Panel panel)
        {
            // Define a região do painel como circular
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, panel.Width - 1, panel.Height - 1); // Subtrai 1 para evitar que a borda fique cortada
            panel.Region = new Region(path);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await UpdatePingStatusAndIP();
        }

        private async Task UpdatePingStatusAndIP()
        {
            try
            {
                await AuthenticateZabbixPLC("Admin", "2rD&2Bn?F4");

                string status10647 = await GetICMPPingStatus("10647");
                string status10648 = await GetICMPPingStatus("10648");

                string ip10647 = await GetHostIPZabbixPLC("10647");
                string ip10648 = await GetHostIPZabbixPLC("10648");

                label9.Text = $"Primary Link DMA: {ip10647}";
                label12.Text = $"Secundary Link DMA: {ip10648}";

                panel15.BackColor = status10647 == "1" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                MakeCircular(panel15); // Se desejar que o painel tenha formato circular

                panel16.BackColor = status10648 == "1" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                MakeCircular(panel16); // Se desejar que o painel tenha formato circular


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void materialButton12_Click(object sender, EventArgs e)
        {
            LoadDataOperatingSystem();
            LoadDataManufacter();
            LoadDataType();
            LoadDataServers();
            LoadDataAvailable();
            LoadDataWarranty();
            LoadDataPrinters();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            PrinterInventoryForm printerInventoryForm = new PrinterInventoryForm();

            this.Hide();

            printerInventoryForm.FormClosed += (s, args) => this.Show();

            printerInventoryForm.Show();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            ContractsForm contractsForm = new ContractsForm();

            this.Hide();

            contractsForm.FormClosed += (s, args) => this.Show();

            contractsForm.Show();
        }
    }
}
