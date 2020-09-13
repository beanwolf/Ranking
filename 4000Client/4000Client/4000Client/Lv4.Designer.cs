namespace _4000Client
{
    partial class Lv4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_ExitGame = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.exitBin = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ExitGame
            // 
            this.button_ExitGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ExitGame.Location = new System.Drawing.Point(976, 4);
            this.button_ExitGame.Name = "button_ExitGame";
            this.button_ExitGame.Size = new System.Drawing.Size(29, 24);
            this.button_ExitGame.TabIndex = 0;
            this.button_ExitGame.Text = "X";
            this.button_ExitGame.UseVisualStyleBackColor = true;
            this.button_ExitGame.Click += new System.EventHandler(this.button_ExitGame_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.Transparent;
            this.panTitle.BackgroundImage = global::_4000Client.Properties.Resources.TitleBar1;
            this.panTitle.Controls.Add(this.button_ExitGame);
            this.panTitle.Controls.Add(this.exitBin);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1008, 28);
            this.panTitle.TabIndex = 22;
            this.panTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panTitle_MouseDown);
            this.panTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panTitle_MouseMove);
            // 
            // exitBin
            // 
            this.exitBin.BackColor = System.Drawing.Color.CadetBlue;
            this.exitBin.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.exitBin.ForeColor = System.Drawing.Color.Black;
            this.exitBin.Location = new System.Drawing.Point(1188, 0);
            this.exitBin.Name = "exitBin";
            this.exitBin.Size = new System.Drawing.Size(76, 25);
            this.exitBin.TabIndex = 12;
            this.exitBin.Text = "종료";
            this.exitBin.UseVisualStyleBackColor = false;
            // 
            // Lv4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 589);
            this.ControlBox = false;
            this.Controls.Add(this.panTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Lv4";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lv4";
            this.panTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ExitGame;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button exitBin;
    }
}