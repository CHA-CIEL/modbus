using System;
using System.Windows.Forms;

namespace modbus
{
    public partial class Form1 : Form
    {
        // Réservation de la socket
        private CModbus modbus;
        public Form1()
        {
            InitializeComponent();
            // Instanciation de l'objet Modbus
            modbus = new CModbus();
        }
        private void buttonConnexion_Click(object sender, EventArgs e)
        {
            string ip = textBoxAdresseIP.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                AppendStatus("Adresse IP vide");
                return;
            }
            string res = modbus.Connexion(ip);
            if (res == "ok")
            {
                AppendStatus("Connexion ok");
            }
            else
            {
                AppendStatus($"**Exception : Impossible de se connecter au serveur** - {res}");
                MessageBox.Show(res, "Connexion échouée", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendStatus(string message)
        {
            textBoxStatut.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
        }

        private void buttonDeconnexion_Click(object sender, EventArgs e)
        {
            string res = modbus.Deconnexion();
            if (res == "ok")
            {
                AppendStatus("déconnexion réussie");
            }
            else
            {
                AppendStatus($"**Exception : Impossible de se déconnecter** - {res}");
                MessageBox.Show(res, "Déconnexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonLireTension_Click(object sender, EventArgs e)
        {
            try
            {
                // Utilise la variante avec message en sortie pour récupérer l'erreur sans lever d'exception
                string res = string.Empty;
                short mot = modbus.LireUnMot(0x0C87, ref res);
                if (res == "ok")
                {
                    // Convertit le mot (dixièmes de volt) en volts
                    ushort raw = unchecked((ushort)mot);
                    double tension = raw / 10.0;
                    AppendStatus($"Tension lue = {tension:F1} V");
                }
                else
                {
                    AppendStatus($"Erreur Modbus: {res}");
                }
                // Affichage du tableau/hex supprimé pour simplifier au début
                // AppendStatus(ToHex(...))
            }
            catch (Exception)
            {
                // Mettre en commentaire l'affichage du message d'exception (consigne)
                // MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendStatus("Erreur lors de la lecture de la tension");
            }
        }

    }
}
