namespace DeNavify
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            username = new TextBox();
            DbUSER = new Label();
            password = new TextBox();
            DbPass = new Label();
            DbServer = new TextBox();
            label1 = new Label();
            label2 = new Label();
            DeNavify = new Button();
            DbComboBox = new ComboBox();
            label3 = new Label();
            SymbolBox = new TextBox();
            SuspendLayout();
            // 
            // username
            // 
            username.Location = new Point(29, 47);
            username.Name = "username";
            username.Size = new Size(127, 23);
            username.TabIndex = 0;
            username.Text = "sa";
            // 
            // DbUSER
            // 
            DbUSER.AutoSize = true;
            DbUSER.Location = new Point(29, 29);
            DbUSER.Name = "DbUSER";
            DbUSER.Size = new Size(45, 15);
            DbUSER.TabIndex = 1;
            DbUSER.Text = "DbUser";
            // 
            // password
            // 
            password.Location = new Point(233, 47);
            password.Name = "password";
            password.PasswordChar = '*';
            password.Size = new Size(127, 23);
            password.TabIndex = 1;
            // 
            // DbPass
            // 
            DbPass.AutoSize = true;
            DbPass.Location = new Point(233, 29);
            DbPass.Name = "DbPass";
            DbPass.Size = new Size(45, 15);
            DbPass.TabIndex = 3;
            DbPass.Text = "DbPass";
            // 
            // DbServer
            // 
            DbServer.Location = new Point(435, 47);
            DbServer.Name = "DbServer";
            DbServer.Size = new Size(127, 23);
            DbServer.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(435, 29);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 5;
            label1.Text = "Server";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(632, 29);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 7;
            label2.Text = "Database";
            // 
            // DeNavify
            // 
            DeNavify.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            DeNavify.Location = new Point(628, 287);
            DeNavify.Name = "DeNavify";
            DeNavify.Size = new Size(127, 23);
            DeNavify.TabIndex = 5;
            DeNavify.Text = "DENAVIFY";
            DeNavify.UseVisualStyleBackColor = true;
            DeNavify.Click += DeNavifyButton_Click;
            // 
            // DbComboBox
            // 
            DbComboBox.FormattingEnabled = true;
            DbComboBox.Location = new Point(620, 47);
            DbComboBox.Name = "DbComboBox";
            DbComboBox.Size = new Size(127, 23);
            DbComboBox.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 145);
            label3.Name = "label3";
            label3.Size = new Size(327, 15);
            label3.TabIndex = 8;
            label3.Text = "Choose characters you want to remove seperated by comma";
            // 
            // SymbolBox
            // 
            SymbolBox.Location = new Point(29, 163);
            SymbolBox.MaxLength = 5000;
            SymbolBox.Name = "SymbolBox";
            SymbolBox.Size = new Size(327, 23);
            SymbolBox.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(767, 322);
            Controls.Add(SymbolBox);
            Controls.Add(label3);
            Controls.Add(DbComboBox);
            Controls.Add(DeNavify);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(DbServer);
            Controls.Add(DbPass);
            Controls.Add(password);
            Controls.Add(DbUSER);
            Controls.Add(username);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Denav";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox username;
        private Label DbUSER;
        private TextBox password;
        private Label DbPass;
        private TextBox DbServer;
        private Label label1;
        private Label label2;
        private Button button1;
        private ComboBox DbComboBox;
        private Button DeNavify;
        private Label label3;
        private TextBox SymbolBox;
    }
}