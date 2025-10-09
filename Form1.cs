using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace modbus
{
    public partial class Form1 : Form
    {
        // Réservation de la socket
        private Socket socket;
        public Form1()
        {
            InitializeComponent();
        }
        private void buttonConnexion_Click(object sender, EventArgs e)
        {
            string ip = textBoxAdresseIP.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                AppendStatus("Adresse IP vide");
                return;
            }
            try
            {
                // Instanciation de la socket 
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Création de l'IPEndPoint 
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint endPoint = new IPEndPoint(ipAddress, 502);

                AppendStatus($"Socket prête. EndPoint: {endPoint}");
                socket.Connect(endPoint);
                AppendStatus("Connexion ok");
            }
            catch (SocketException ex)
            {
                AppendStatus($"**Exception : Impossible de se connecter au serveur** - {ex.Message}");
                MessageBox.Show(ex.Message, "Exception Socket", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                AppendStatus($"**Exception : Impossible de se connecter au serveur** - {ex.Message}");
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendStatus(string message)
        {
            textBoxStatut.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
        }

        private void buttonDeconnexion_Click(object sender, EventArgs e)
        {
            try
            {
                if (socket != null)
                {
                    if (socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
                AppendStatus("déconnexion réussie");
            }
            catch (SocketException ex)
            {
                AppendStatus($"**Exception : Impossible de se déconnecter** - {ex.Message}");
                MessageBox.Show(ex.Message, "Exception Socket", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                AppendStatus($"**Exception : Impossible de se déconnecter** - {ex.Message}");
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
