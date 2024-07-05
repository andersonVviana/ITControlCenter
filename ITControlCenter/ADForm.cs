using System.DirectoryServices;

namespace ITControlCenter
{
    public partial class ADForm : Form
    {
        private string userPrincipalName;

        public ADForm()
        {
            InitializeComponent();

            button2.Click += new EventHandler(button2_Click);
            button2.Enabled = false;
            textBox1.KeyDown += new KeyEventHandler(textBox1_TextChanged);
            button2.Click += new EventHandler(button2_Click);
            button3.Click += new EventHandler(button3_Click);
            button3.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string username = textBox1.Text;
                GetUserDetails(username);
            }
        }

        private string GetUsernameFromEmail(string email)
        {
            try
            {
                // Encontra a posição do "@" no e-mail
                int atIndex = email.IndexOf('@');
                if (atIndex >= 0)
                {
                    // Retorna o texto após o "@" (o nome de usuário)
                    return email.Substring(atIndex + 1);
                }
                else
                {
                    return string.Empty; // Retorna vazio se não encontrar o "@"
                }
            }
            catch (Exception)
            {
                return string.Empty; // Retorna vazio em caso de erro
            }
        }

        private void GetUserDetails(string username)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://mappel.local");
                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = $"(sAMAccountName={username})"
                };

                search.PropertiesToLoad.AddRange(new string[]
                {
                    "givenName", "sn", "displayName", "description",
                    "physicalDeliveryOfficeName", "telephoneNumber", "mail",
                    "userAccountControl", "streetAddress", "l", "st", "postalCode",
                    "co", "sAMAccountName", "userPrincipalName", "title",
                    "department", "company", "manager", "memberOf",
                    "pwdLastSet", "msDS-UserPasswordExpiryTimeComputed",
                    "lockoutTime"
                });

                SearchResult result = search.FindOne();

                if (result != null)
                {
                    nameTxb.Text = result.Properties["givenName"][0]?.ToString() ?? string.Empty;
                    lastNameTxb.Text = result.Properties["sn"][0]?.ToString() ?? string.Empty;
                    displayNameTxb.Text = result.Properties["displayName"][0]?.ToString() ?? string.Empty;
                    descriptionTxb.Text = result.Properties["description"][0]?.ToString() ?? string.Empty;
                    officeTxb.Text = result.Properties["physicalDeliveryOfficeName"][0]?.ToString() ?? string.Empty;
                    phoneTxb.Text = result.Properties["telephoneNumber"][0]?.ToString() ?? string.Empty;
                    emailTxb.Text = result.Properties["mail"][0]?.ToString() ?? string.Empty;
                    textBox2.Text = result.Properties["streetAddress"][0]?.ToString() ?? string.Empty;
                    textBox3.Text = result.Properties["l"][0]?.ToString() ?? string.Empty;
                    textBox4.Text = result.Properties["st"][0]?.ToString() ?? string.Empty;
                    textBox5.Text = result.Properties["postalCode"][0]?.ToString() ?? string.Empty;
                    textBox6.Text = result.Properties["co"][0]?.ToString() ?? string.Empty;
                    textBox7.Text = result.Properties["sAMAccountName"][0]?.ToString() ?? string.Empty;
                    textBox8.Text = GetDomainFromEmail(result.Properties["userPrincipalName"][0]?.ToString() ?? string.Empty);
                    textBox9.Text = result.Properties["title"][0]?.ToString() ?? string.Empty;
                    textBox10.Text = result.Properties["department"][0]?.ToString() ?? string.Empty;
                    textBox11.Text = result.Properties["company"][0]?.ToString() ?? string.Empty;
                    textBox12.Text = GetManagerName(result.Properties["manager"][0]?.ToString());

                    int userAccountControl = result.Properties["userAccountControl"].Count > 0 ? (int)result.Properties["userAccountControl"][0] : 0;
                    bool isLocked = result.Properties["lockoutTime"].Count > 0 && (long)result.Properties["lockoutTime"][0] != 0;
                    string accountStatus = GetAccountStatus(userAccountControl, isLocked);

                    statusTxb.Text = accountStatus;

                    // Definir a cor de fundo com base no estado da conta
                    switch (accountStatus)
                    {
                        case " Ativo":
                            statusTxb.BackColor = Color.Green;
                            statusTxb.ForeColor = Color.White;
                            button2.Enabled = false;
                            button3.Enabled = true;
                            break;
                        case " Desativado":
                            statusTxb.BackColor = Color.Red;
                            statusTxb.ForeColor = Color.White;
                            button2.Enabled = false;
                            button3.Enabled = true;
                            break;
                        case " Bloqueado":
                            statusTxb.BackColor = Color.Orange;
                            statusTxb.ForeColor = Color.White;
                            button2.Enabled = true;
                            break;
                        default:
                            statusTxb.BackColor = SystemColors.Window;
                            button2.Enabled = false;// Cor padrão
                            break;
                    }

                    DisplayUserGroups(result.Properties["memberOf"]);

                    string passwordStatus = GetPasswordStatus(result, userAccountControl);
                    textBox13.Text = passwordStatus;

                    switch (passwordStatus)
                    {
                        case " Senha expirada":
                            textBox13.BackColor = Color.Red;
                            textBox13.ForeColor = Color.DarkBlue;
                            break;
                        case " Senha nunca expira":
                            textBox13.BackColor = Color.Yellow;
                            textBox13.ForeColor = Color.DarkBlue;
                            break;
                        case string s when s.Contains(" Senha prestes a expirar"):
                            textBox13.BackColor = Color.Orange;
                            textBox13.ForeColor = Color.DarkBlue;
                            break;
                        default:
                            textBox13.BackColor = SystemColors.Window; // Cor padrão
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado.");
                    button2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar informações do usuário: " + ex.Message);
                button2.Enabled = false;
            }
        }

        private string GetAccountStatus(int userAccountControl, bool isLocked)
        {
            const int UF_ACCOUNTDISABLE = 0x0002;

            if ((userAccountControl & UF_ACCOUNTDISABLE) == UF_ACCOUNTDISABLE)
            {
                return " Desativado";
            }
            else if (isLocked)
            {
                return " Bloqueado";
            }
            else
            {
                return " Ativo";
            }
        }

        private string GetManagerName(string managerDn)
        {
            if (string.IsNullOrEmpty(managerDn))
            {
                return string.Empty;
            }

            try
            {
                DirectoryEntry managerEntry = new DirectoryEntry("LDAP://mappel.local/" + managerDn);
                return managerEntry.Properties["displayName"][0]?.ToString() ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetDomainFromEmail(string email)
        {
            try
            {
                // Encontra a posição do "@" no e-mail
                int atIndex = email.IndexOf('@');
                if (atIndex >= 0)
                {
                    // Retorna o texto depois do "@" (o domínio)
                    return email.Substring(atIndex);
                }
                else
                {
                    return string.Empty; // Retorna vazio se não encontrar o "@"
                }
            }
            catch (Exception)
            {
                return string.Empty; // Retorna vazio em caso de erro
            }
        }

        private void DisplayUserGroups(ResultPropertyValueCollection memberOf)
        {
            listBox1.Items.Clear();

            foreach (string group in memberOf)
            {
                // Obter o nome do grupo a partir do caminho do LDAP
                int startIndex = group.IndexOf('=') + 1;
                int endIndex = group.IndexOf(',');
                string groupName = group.Substring(startIndex, endIndex - startIndex);

                listBox1.Items.Add(groupName);
            }
        }

        private string GetPasswordStatus(SearchResult result, int userAccountControl)
        {
            const int UF_DONT_EXPIRE_PASSWD = 0x10000;

            if ((userAccountControl & UF_DONT_EXPIRE_PASSWD) == UF_DONT_EXPIRE_PASSWD)
            {
                return " Senha nunca expira";
            }

            if (result.Properties.Contains("msDS-UserPasswordExpiryTimeComputed") && result.Properties["msDS-UserPasswordExpiryTimeComputed"].Count > 0)
            {
                long expiryDateLong = (long)result.Properties["msDS-UserPasswordExpiryTimeComputed"][0];
                DateTime expiryDate = DateTime.FromFileTime(expiryDateLong);

                if (DateTime.Now > expiryDate)
                {
                    return " Senha expirada";
                }
                else if ((expiryDate - DateTime.Now).TotalDays <= 7)
                {
                    return $" {(expiryDate - DateTime.Now).TotalDays:F0} dias";
                }
                else
                {
                    return $" {(expiryDate - DateTime.Now).TotalDays:F0} dias";
                }
            }

            return " Status da senha desconhecido";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://mappel.local");
                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = $"(sAMAccountName={username})"
                };

                SearchResult result = search.FindOne();

                if (result != null)
                {
                    DirectoryEntry userEntry = result.GetDirectoryEntry();
                    const int UF_LOCKOUT = 0x0010;

                    int userAccountControl = (int)userEntry.Properties["userAccountControl"].Value;
                    userAccountControl &= ~UF_LOCKOUT; // Remove o flag de bloqueio
                    userEntry.Properties["userAccountControl"].Value = userAccountControl;
                    userEntry.Properties["lockoutTime"].Value = 0;

                    userEntry.CommitChanges();

                    MessageBox.Show("Usuário desbloqueado", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    button2.Enabled = false;
                    statusTxb.Text = " Ativo";
                    statusTxb.BackColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao desbloquear o usuário: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var changePasswordForm = new ChangePasswordForm())
            {
                if (changePasswordForm.ShowDialog() == DialogResult.OK)
                {
                    // Lógica para alterar a senha
                    string username = textBox1.Text;
                    string newPassword = changePasswordForm.NewPassword;

                    try
                    {
                        DirectoryEntry entry = new DirectoryEntry("LDAP://mappel.local");
                        DirectorySearcher search = new DirectorySearcher(entry)
                        {
                            Filter = $"(sAMAccountName={username})"
                        };

                        SearchResult result = search.FindOne();

                        if (result != null)
                        {
                            DirectoryEntry userEntry = result.GetDirectoryEntry();
                            userEntry.Invoke("SetPassword", new object[] { newPassword });
                            userEntry.CommitChanges();

                            MessageBox.Show("Senha alterada com sucesso", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao alterar a senha do usuário: " + ex.Message);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string fullName = $"{nameTxb.Text} {lastNameTxb.Text}";

            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://mappel.local");
                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = $"(sAMAccountName={username})"
                };

                SearchResult searchResult = search.FindOne();

                if (searchResult != null)
                {
                    DirectoryEntry userEntry = searchResult.GetDirectoryEntry();
                    int userAccountControl = (int)userEntry.Properties["userAccountControl"].Value;
                    const int UF_ACCOUNTDISABLE = 0x0002;

                    // Verificar se o usuário está desativado
                    if ((userAccountControl & UF_ACCOUNTDISABLE) == UF_ACCOUNTDISABLE)
                    {
                        // Usuário está desativado, perguntar se deseja ativar
                        DialogResult result = MessageBox.Show($"Deseja ativar o usuário {fullName}?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            // Ativar o usuário
                            userEntry.Properties["userAccountControl"].Value = userAccountControl & ~UF_ACCOUNTDISABLE; // Remove UF_ACCOUNTDISABLE
                            userEntry.CommitChanges();

                            MessageBox.Show("Usuário ativado com sucesso", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Se clicado em Não, não faz nada
                    }
                    else
                    {
                        // Usuário está ativo, perguntar se deseja desativar
                        DialogResult result = MessageBox.Show($"Deseja desativar o usuário {fullName}?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            // Desativar o usuário
                            userEntry.Properties["userAccountControl"].Value = userAccountControl | UF_ACCOUNTDISABLE; // Adiciona UF_ACCOUNTDISABLE
                            userEntry.CommitChanges();

                            MessageBox.Show("Usuário desativado com sucesso", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Se clicado em Não, não faz nada
                    }
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar o status do usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
