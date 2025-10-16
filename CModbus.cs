using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; // pour IPEndPoint 
using System.Net.Sockets; // pour les sockets 

namespace modbus
{
    internal class CModbus
    {
        private Socket socket;

        public Socket SocketClient => socket;

        // adresseIP est l'adresse du serveur
        // valeur de retour : "ok" ou le message d'erreur détaillé de l'exception
        public string Connexion(string adresseIP)
        {
            try
            {
                // Fermer une éventuelle connexion actuelle
                if (socket != null)
                {
                    try
                    {
                        if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    catch { /* ignorer */ }
                    finally { socket = null; }
                }

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(adresseIP);
                IPEndPoint endPoint = new IPEndPoint(ipAddress, 502);
                socket.Connect(endPoint);
                return "ok";
            }
            catch (Exception ex)
            {
                // En cas d'échec, réinitialiser la socket et renvoyer le message
                try { socket?.Dispose(); } catch { }
                socket = null;
                return ex.Message;
            }
        }

        // Déconnexion du serveur
        // valeur de retour : "ok" ou le message d'erreur détaillé
        public string Deconnexion()
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
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Vérifie que la socket est connectée
        private void VerifConnected()
        {
            if (socket == null || !socket.Connected)
                throw new InvalidOperationException("Socket non connectée");
        }

        // Envoie une requête et retourne la réponse validée
        private byte[] EnvoieReq(ushort startAddress, ushort quantity)
        {
            VerifConnected();

            const byte unitId = 0x01;
            const byte function = 0x03;

            byte[] frameE = new byte[12];
            frameE[0] = 0x00; 
            frameE[1] = 0x00; 
            frameE[2] = 0x00; 
            frameE[3] = 0x00; 
            frameE[4] = 0x00; 
            frameE[5] = 0x06; 
            frameE[6] = unitId;
            frameE[7] = function;
            frameE[8] = (byte)(startAddress >> 8);
            frameE[9] = (byte)(startAddress & 0xFF);
            frameE[10] = (byte)(quantity >> 8);
            frameE[11] = (byte)(quantity & 0xFF);

            socket.Send(frameE);

            byte[] frameR = new byte[256];
            int received = socket.Receive(frameR);
            if (received < 11)
                throw new Exception("Réponse trop courte");
            if (frameR[7] != function)
                throw new Exception($"Code fonction inattendu: {frameR[7]:X2}");

            byte expectedByteCount = (byte)(quantity * 2);
            if (frameR[8] != expectedByteCount)
                throw new Exception($"Taille de données inattendue: {frameR[8]}");

            return frameR;
        }
        
        // Variante avec message d'erreur en sortie via 'ref string'
        // strResultat : "ok" si tout va bien, sinon le message d'erreur
        public short LireUnMot(short adresse, ref string strResultat)
        {
            try
            {
                if (adresse < 0)
                    throw new ArgumentOutOfRangeException(nameof(adresse), "L'adresse doit être positive");

                // vérifie la connexion et lit 1 registre
                ushort start = (ushort)adresse;
                byte[] resp = EnvoieReq(start, 1);

                short value = unchecked((short)((resp[9] << 8) | resp[10]));
                strResultat = "ok";
                return value;
            }
            catch (Exception ex)
            {
                // renvoie 0 et retourne le message dans strResultat
                strResultat = ex.Message;
                return 0;
            }
        }

        // Méthode provisoire simplifiée pour lire la tension
        // Lit le registre 0x0C87 et retourne la tension en volts (arrondie à l'entier)
        public int LireTension()
        {
            // Adresse fixe utilisée au début pour la tension
            const ushort adresseTension = 0x0C87;
            byte[] resp = EnvoieReq(adresseTension, 1);

            int raw = (resp[9] << 8) | resp[10]; // valeur brute en dixièmes de volt
            int volts = (int)Math.Round(raw / 10.0); // 223 V
            return volts;
        }
      
    }
}
