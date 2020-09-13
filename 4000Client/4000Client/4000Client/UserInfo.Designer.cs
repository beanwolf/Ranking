namespace _4000Client
{
    partial class UserInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInfo));
            this.uinfo_id = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.webBrowser_UserAVATA = new System.Windows.Forms.WebBrowser();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.webBrowser2 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // uinfo_id
            // 
            this.uinfo_id.AutoSize = true;
            this.uinfo_id.Location = new System.Drawing.Point(12, 9);
            this.uinfo_id.Name = "uinfo_id";
            this.uinfo_id.Size = new System.Drawing.Size(16, 12);
            this.uinfo_id.TabIndex = 0;
            this.uinfo_id.Text = "ID";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.button1.Location = new System.Drawing.Point(12, 235);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "창닫기";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // webBrowser_UserAVATA
            // 
            this.webBrowser_UserAVATA.Location = new System.Drawing.Point(14, 24);
            this.webBrowser_UserAVATA.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_UserAVATA.Name = "webBrowser_UserAVATA";
            this.webBrowser_UserAVATA.ScrollBarsEnabled = false;
            this.webBrowser_UserAVATA.Size = new System.Drawing.Size(84, 160);
            this.webBrowser_UserAVATA.TabIndex = 30;
            this.webBrowser_UserAVATA.Url = new System.Uri("https://www.ranking.co.kr/Account/AvataDisplay.aspx", System.UriKind.Absolute);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(104, 24);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(174, 76);
            this.webBrowser1.TabIndex = 31;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // webBrowser2
            // 
            this.webBrowser2.Location = new System.Drawing.Point(104, 108);
            this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser2.Name = "webBrowser2";
            this.webBrowser2.Size = new System.Drawing.Size(174, 141);
            this.webBrowser2.TabIndex = 32;
            this.webBrowser2.Url = new System.Uri("https://www.ranking.co.kr/Account/AvataDisplay.aspx", System.UriKind.Absolute);
            // 
            // UserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.webBrowser2);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.webBrowser_UserAVATA);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uinfo_id);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserInfo";
            this.Text = "UserInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label uinfo_id;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.WebBrowser webBrowser_UserAVATA;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.WebBrowser webBrowser2;
    }
}