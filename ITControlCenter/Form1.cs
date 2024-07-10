using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Security.Principal;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ITControlCenter
{
    public partial class MenuFrm : Form
    {

        string linkSophosCentral = "https://login.sophos.com/login.sophos.com/oauth2/v2.0/authorize?p=B2C_1A_signup_signin&client_id=d8ce821f-a1da-4b03-b7e2-1d1a9cc028f3&redirect_uri=https%3A%2F%2Fcentral.sophos.com%2Fmanage%2Flogin%2Fazureb2c&scope=openid&response_type=id_token&prompt=login&state=";
        string vSpere = "https://arcade-vc01.mappel.local/websso/SAML2/SSO/mappel.virtual?SAMLRequest=zVTfT9swEH7fXxH5PbHjZsAsUtS1oFWC0ZEOTXuZTHKAJcfOfE5S%2Fvs5aTsqtCH2NilPzt1334%2Bz%0AT882tY46cKisyUmaMBKBKW2lzENOvq4v4hNyNn13irLWjZi1%2FtHcwM8W0EczRHA%2BtM2twbYGV4Dr%0AVAlLU8EmJwFoEcqUkX6EfvS%2BQUHp7GY%2BW5zHt3OWJrVsGtCJtqXUtIc7REuL2dUlp0VxTXd%2FO%2BV8%0AKzWJLqwrYaSQk3upEUi0XOTkR8VSdgwyzarqKE15OmFQMZlVfMIn7OgkEFniSiKqDp4bEdvAFL00%0APiec8Sxmx%2BFbpxORccGPkuzD%2B%2B8kWjnrbWn1R2W2lrTOCCtRoTCyBhS%2BFANjwRMm7rZFKD6t16t4%0AdV2sR4BOVeA%2Bh%2BqcSFfKCuKufCFeZNmERLf7GPgQQwjGoBiNf31ss%2BNIpruYRnHu7QBynySZ7lP6%0AG1HaKjog7tOqwctKenlKD0dvifBGDKqXi5XVqnyKZlrbfu5A%2BuCEdy2MkdbSv85uOFFVfD%2BWimbw%0ACD0YT6JiNeB%2FCcuh7hW45x37V%2FaHZvO3uk13KkXY%2F0oN9uEhzJstf4myA%2BlCy1ZRENTVvXSQlLam%0AWD5CLZFK7108AlPOUk5ZRs83wZdhg3AvaIPqN0bf90k%2FSax7CA0spd%2BuLosRK1bjPShDHqFe%2BKcm%0A5DOMFzdgoJd3Gtbh7A%2BC%2FyOqC9DwcEiVvgxnut%2FRw1ds%2Bgs%3D&SigAlg=http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmldsig-more%23rsa-sha256&Signature=moWl7kzJp4DD0hcVDETfaAocFJ3YkyOnGOR4OgGDpNharsSPXDKRzIjKsZA74urZj99bfCQNgQlw%0AlyzrwTzSJLbAmYMeEGz6X8%2FrioUNNnSr2yO4Y9S7nFLTmqZFO6haiGgjn7Ni%2F%2Fm8lhfwKTfjp35m%0AK4F7MITo9lIkKAe%2FRoO35Sq1pg%2Fhi7vv%2BmPJWZFsAAbp2koak2VUBCAnu5Te2DMYdksCZIAhjhK%2B%0AjaeqFKi8N3%2FBRv%2FNvY3UIpO8qpf0mwrWKIYQMKchXXHajYtViZiR%2BIKlgh5w6eG34jxGDLwwRGr5%0A%2BNohCxALEfwzQH76kpZckU%2Buothx5nvvNN4YzA%3D%3D";
        string faq = "http://arcade-faq01.mappel.local/faq/";

        private Image userImage;

        private const string ApiKey = "09761fd8f3af97d8ec38b6204baf689e"; // Substitua com sua chave de API
        private const string BaseUrl = "http://api.openweathermap.org/data/2.5/weather";
        private const string LocationUrl = "http://ip-api.com/json";

        public MenuFrm()
        {
            InitializeComponent();
            Load += new EventHandler(MenuFrm_Load);
            pictureBox3.Paint += PictureBox_Paint;
        }

        private void btnAD_Click(object sender, EventArgs e)
        {
            ADForm aDForm = new ADForm();
            aDForm.FormClosed += aDForm_FormClosed;
            aDForm.Show();
            this.Hide();
        }

        private void aDForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void MenuFrm_Load(object sender, EventArgs e)
        {
            await GetLocationAndWeatherData();
            DisplayCurrentDate();

            try
            {
                // Obtém o primeiro nome do usuário logado no AD
                string firstName = GetFirstNameFromAD();

                // Exibe o primeiro nome na label
                label5.Text = firstName;

                // Obtém o nome de usuário atual
                string username = Environment.UserName;

                // Obtém a foto do perfil do usuário
                userImage = GetUserProfileImage(username);

                // Força o PictureBox a repintar
                pictureBox3.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar a imagem do perfil: {ex.Message}");
            }
        }

        private void DisplayCurrentDate()
        {
            DateTime currentDate = DateTime.Now;
            label11.Text = currentDate.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"));
        }

        private async Task GetLocationAndWeatherData()
        {
            string city = await GetLocation();
            if (!string.IsNullOrWhiteSpace(city))
            {
                label10.Text = $"{city}";
                await GetWeatherData(city);
            }
            else
            {
                MessageBox.Show("Could not retrieve location.");
            }
        }

        private async Task<string> GetLocation()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(LocationUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    LocationResponse locationResponse = JsonConvert.DeserializeObject<LocationResponse>(responseBody);

                    return locationResponse.City;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching location data: {ex.Message}");
                    return null;
                }
            }
        }

        private string GetFirstNameFromAD()
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, Environment.UserName))
            {
                if (user != null)
                {
                    // Obtém o primeiro nome do usuário
                    return user.GivenName;
                }
                else
                {
                    throw new Exception("Usuário não encontrado no Active Directory.");
                }
            }
        }


        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (userImage != null)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Define o retângulo onde a imagem será desenhada
                Rectangle rect = new Rectangle(0, 0, pictureBox3.Width, pictureBox3.Height);

                // Cria um caminho elíptico (círculo) dentro do retângulo
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(rect);
                    g.SetClip(path);

                    // Desenha a imagem dentro do círculo
                    g.DrawImage(userImage, rect);

                    // Remove o recorte para futuros desenhos
                    g.ResetClip();
                }
            }
        }

        private string GetFullNameFromAD(string username)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username))
            {
                return user?.DisplayName ?? username;
            }
        }

        private Image GetUserProfileImage(string username)
        {
            try
            {
                // Caminho para o diretório de imagens de perfil
                string profileImagePath = $@"C:\Users\{username}\AppData\Roaming\Microsoft\Windows\AccountPictures";

                // Obtém o arquivo mais recente no diretório
                var directoryInfo = new System.IO.DirectoryInfo(profileImagePath);
                var fileInfo = directoryInfo.GetFiles()
                                           .OrderByDescending(f => f.LastWriteTime)
                                           .FirstOrDefault();

                if (fileInfo != null && System.IO.File.Exists(fileInfo.FullName))
                {
                    // Carrega e retorna a imagem do perfil
                    return Image.FromFile(fileInfo.FullName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter a imagem do perfil: {ex.Message}");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = linkSophosCentral;

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir o navegador: " + ex.Message);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = vSpere;

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir o navegador: " + ex.Message);
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = faq;

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir o navegador: " + ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private async Task GetWeatherData(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"{BaseUrl}?q={city}&appid={ApiKey}&units=metric";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(responseBody);

                    float temperature = MathF.Round(weatherResponse.Main.Temp);
                    label9.Text = $"{GetEmojiForTemperature(temperature)} {temperature} °C";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching weather data: {ex.Message}");
                }
            }
        }

        private string GetEmojiForTemperature(float temp)
        {
            if (temp <= 0)
            {
                return "❄️"; // Snowflake for temperatures <= 0°C
            }
            else if (temp > 0 && temp <= 10)
            {
                return "🧥"; // Coat for temperatures between 1°C and 10°C
            }
            else if (temp > 10 && temp <= 20)
            {
                return "🌤️"; // Sun behind small cloud for temperatures between 11°C and 20°C
            }
            else if (temp > 20 && temp <= 30)
            {
                return "☀️"; // Sun for temperatures between 21°C and 30°C
            }
            else
            {
                return "🔥"; // Fire for temperatures > 30°C
            }
        }

        public class WeatherResponse
        {
            public WeatherInfo[] Weather { get; set; }
            public MainInfo Main { get; set; }
        }

        public class WeatherInfo
        {
            public string Description { get; set; }
        }

        public class MainInfo
        {
            public float Temp { get; set; }
        }

        public class LocationResponse
        {
            public string City { get; set; }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}



