namespace modbus
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        private void InitializeComponent()
        {
            this.labelAdresseIP = new System.Windows.Forms.Label();
            this.textBoxAdresseIP = new System.Windows.Forms.TextBox();
            this.buttonConnexion = new System.Windows.Forms.Button();
            this.buttonDeconnexion = new System.Windows.Forms.Button();
            this.textBoxStatut = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelAdresseIP
            // 
            this.labelAdresseIP.AutoSize = true;
            this.labelAdresseIP.Location = new System.Drawing.Point(12, 15);
            this.labelAdresseIP.Name = "labelAdresseIP";
            this.labelAdresseIP.Size = new System.Drawing.Size(54, 13);
            this.labelAdresseIP.TabIndex = 0;
            this.labelAdresseIP.Text = "Ip serveur";
            // 
            // textBoxAdresseIP
            // 
            this.textBoxAdresseIP.Location = new System.Drawing.Point(78, 12);
            this.textBoxAdresseIP.Name = "textBoxAdresseIP";
            this.textBoxAdresseIP.Size = new System.Drawing.Size(100, 20);
            this.textBoxAdresseIP.TabIndex = 1;
            this.textBoxAdresseIP.Text = "172.17.50.180";
            // 
            // buttonConnexion
            // 
            this.buttonConnexion.Location = new System.Drawing.Point(184, 10);
            this.buttonConnexion.Name = "buttonConnexion";
            this.buttonConnexion.TabIndex = 2;
            this.buttonConnexion.Text = "Connexion";
            this.buttonConnexion.UseVisualStyleBackColor = true;
            this.buttonConnexion.Click += new System.EventHandler(this.buttonConnexion_Click);
            // 
            // buttonDeconnexion
            // 
            this.buttonDeconnexion.Location = new System.Drawing.Point(265, 10);
            this.buttonDeconnexion.Name = "buttonDeconnexion";
            this.buttonDeconnexion.Size = new System.Drawing.Size(85, 23);
            this.buttonDeconnexion.TabIndex = 3;
            this.buttonDeconnexion.Text = "Deconnexion";
            this.buttonDeconnexion.UseVisualStyleBackColor = true;
            this.buttonDeconnexion.Click += new System.EventHandler(this.buttonDeconnexion_Click);
            // 
            // textBoxStatut
            // 
            this.textBoxStatut.Location = new System.Drawing.Point(356, 12);
            this.textBoxStatut.Multiline = true;

            this.textBoxStatut.ReadOnly = true;
            this.textBoxStatut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatut.Size = new System.Drawing.Size(216, 377);
            this.textBoxStatut.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.textBoxStatut);
            this.Controls.Add(this.buttonDeconnexion);
            this.Controls.Add(this.buttonConnexion);
            this.Controls.Add(this.textBoxAdresseIP);
            this.Controls.Add(this.labelAdresseIP);
            this.Name = "Form1";
            this.Text = "Barrière Modbus";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Déclarations
        private System.Windows.Forms.Label labelAdresseIP;
        private System.Windows.Forms.TextBox textBoxAdresseIP;
        private System.Windows.Forms.Button buttonConnexion;
        private System.Windows.Forms.Button buttonDeconnexion;
        private System.Windows.Forms.TextBox textBoxStatut;
    }
}