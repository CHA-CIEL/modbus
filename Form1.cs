using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace modbus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void buttonConnexion_Click(object sender, EventArgs e)
        {
            string ip = textBoxAdresseIP.Text.Trim();
            if (string.IsNullOrEmpty(ip)) ip = "(vide)";
            AppendStatus($"Connexion au serveur {ip}");
        }

        private void AppendStatus(string message)
        {
            textBoxStatut.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
        }

    }
}
