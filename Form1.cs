using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace modbus
{
    public partial class Form1 : Form
    {
        // Réservation de la socket
        private CModbus modbus;
        private int chartIndex = 0;

        public Form1()
        {
            InitializeComponent();
            // Instanciation de l'objet Modbus
            modbus = new CModbus();
            // Configuration légère du chart conçu dans le Designer
            if (chart1.ChartAreas.Count > 0)
            {
                var ca = chart1.ChartAreas[0];
                ca.AxisX.Title = "t";
                ca.AxisY.Title = "V";
            }
            if (chart1.Series.IndexOf("Tension") >= 0)
            {
                var s = chart1.Series["Tension"];
                s.ChartType = SeriesChartType.Line;
                s.BorderWidth = 2;
                s.XValueType = ChartValueType.Int32;
                s.YValueType = ChartValueType.Double;
            }
        }

        private void buttonConnexion_Click(object sender, EventArgs e)
        {
            string ip = textBoxAdresseIP.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                Status("Adresse IP vide");
                return;
            }
            string res = modbus.Connexion(ip);
            if (res == "ok")
            {
                Status("Connexion ok");
            }
            else
            {
                Status($"**Exception : Impossible de se connecter au serveur** - {res}");
                MessageBox.Show(res, "Connexion échouée", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Status(string message)
        {
            textBoxStatut.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
        }

        private void buttonDeconnexion_Click(object sender, EventArgs e)
        {
            string res = modbus.Deconnexion();
            if (res == "ok")
            {
                Status("déconnexion réussie");
            }
            else
            {
                Status($"**Exception : Impossible de se déconnecter** - {res}");
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
                    textBoxTension.Text = tension.ToString("F1");
                    Status($"Tension: {tension:F1} V");
                    Chart(tension);
                }
                else
                {
                    Status($"Erreur Modbus: {res}");
                }
                // Affichage du tableau/hex supprimé pour simplifier au début
                // AppendStatus(ToHex(...))
            }
            catch (Exception ex)
            {
                // Mettre en commentaire l'affichage du message d'exception (consigne)
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Status("Erreur lors de la lecture de la tension");
            }
        }

        private void Chart(double tension)
        {
            if (chart1.Series.IndexOf("Tension") < 0)
                chart1.Series.Add("Tension");
            chart1.Series["Tension"].Points.AddXY(chartIndex, tension);
            chartIndex++;
            if (chartIndex > 100)
            {
                chartIndex = 0;
                chart1.Series["Tension"].Points.Clear();
            }
        }

        private void checkBoxAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoRefresh.Checked)
            {
                timerRefresh.Start();
                Status("Rafraîchissement automatique activé");
            }
            else
            {
                timerRefresh.Stop();
                Status("Rafraîchissement automatique désactivé");
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            // Eviter les appels simultanés
            bool prevConnEnabled = buttonConnexion.Enabled;
            bool prevDeconnEnabled = buttonDeconnexion.Enabled;
            bool prevLireEnabled = buttonLireTension.Enabled;
            buttonConnexion.Enabled = false;
            buttonDeconnexion.Enabled = false;
            buttonLireTension.Enabled = false;
            try
            {
                // Appeler la logique des boutons
                buttonLireTension_Click(buttonLireTension, EventArgs.Empty);
            }
            finally
            {
                buttonConnexion.Enabled = prevConnEnabled;
                buttonDeconnexion.Enabled = prevDeconnEnabled;
                buttonLireTension.Enabled = prevLireEnabled;
            }
        }

    }
}

