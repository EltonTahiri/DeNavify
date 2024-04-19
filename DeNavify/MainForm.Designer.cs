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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            username = new TextBox();
            DbUSER = new Label();
            password = new TextBox();
            DbPass = new Label();
            DbServer = new TextBox();
            label1 = new Label();
            label2 = new Label();
            DbComboBox = new ComboBox();
            label3 = new Label();
            SymbolBox = new TextBox();
            helpProvider1 = new HelpProvider();
            helpProvider2 = new HelpProvider();
            helpProvider = new HelpProvider();
            DeNavify = new MaterialSkin.Controls.MaterialButton();
            imageList1 = new ImageList(components);
            SuspendLayout();
            // 
            // username
            // 
            username.Cursor = Cursors.IBeam;
            username.Location = new Point(29, 125);
            username.Name = "username";
            username.Size = new Size(130, 23);
            username.TabIndex = 0;
            username.Text = "sa";
            // 
            // DbUSER
            // 
            DbUSER.AutoSize = true;
            DbUSER.Location = new Point(29, 107);
            DbUSER.Name = "DbUSER";
            DbUSER.Size = new Size(45, 15);
            DbUSER.TabIndex = 1;
            DbUSER.Text = "DbUser";
            // 
            // password
            // 
            password.Location = new Point(29, 181);
            password.Name = "password";
            password.PasswordChar = '*';
            password.Size = new Size(130, 23);
            password.TabIndex = 2;
            // 
            // DbPass
            // 
            DbPass.AutoSize = true;
            DbPass.Location = new Point(29, 163);
            DbPass.Name = "DbPass";
            DbPass.Size = new Size(45, 15);
            DbPass.TabIndex = 3;
            DbPass.Text = "DbPass";
            DbPass.Click += DbPass_Click;
            // 
            // DbServer
            // 
            DbServer.Location = new Point(204, 125);
            DbServer.Name = "DbServer";
            DbServer.Size = new Size(136, 23);
            DbServer.TabIndex = 1;
            DbServer.TextChanged += DbServer_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(205, 107);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 5;
            label1.Text = "Server";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(205, 163);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 7;
            label2.Text = "Database";
            label2.Click += label2_Click;
            // 
            // DbComboBox
            // 
            DbComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            DbComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            DbComboBox.FormattingEnabled = true;
            DbComboBox.Location = new Point(204, 181);
            DbComboBox.Name = "DbComboBox";
            DbComboBox.Size = new Size(136, 23);
            DbComboBox.TabIndex = 3;
            DbComboBox.SelectedIndexChanged += DbComboBox_SelectedIndexChanged;
            DbComboBox.Enter += DbComboBox_Enter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 235);
            label3.Name = "label3";
            label3.Size = new Size(276, 15);
            label3.TabIndex = 8;
            label3.Text = "Write a character to remove separated by a comma";
            label3.TextAlign = ContentAlignment.TopRight;
            label3.Click += label3_Click_1;
            // 
            // SymbolBox
            // 
            SymbolBox.Location = new Point(50, 253);
            SymbolBox.MaxLength = 5000;
            SymbolBox.Name = "SymbolBox";
            SymbolBox.Size = new Size(278, 23);
            SymbolBox.TabIndex = 4;
            SymbolBox.TextChanged += SymbolBox_TextChanged;
            // 
            // DeNavify
            // 
            DeNavify.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            DeNavify.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            DeNavify.Depth = 0;
            DeNavify.HighEmphasis = true;
            DeNavify.Icon = null;
            DeNavify.Location = new Point(148, 295);
            DeNavify.Margin = new Padding(4, 6, 4, 6);
            DeNavify.MouseState = MaterialSkin.MouseState.HOVER;
            DeNavify.Name = "DeNavify";
            DeNavify.NoAccentTextColor = Color.Empty;
            DeNavify.Size = new Size(91, 36);
            DeNavify.TabIndex = 5;
            DeNavify.Text = "DeNavify";
            DeNavify.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            DeNavify.UseAccentColor = false;
            DeNavify.UseVisualStyleBackColor = true;
            DeNavify.Click += DeNavifyButton_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(370, 370);
            Controls.Add(DeNavify);
            Controls.Add(SymbolBox);
            Controls.Add(label3);
            Controls.Add(DbComboBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(DbServer);
            Controls.Add(DbPass);
            Controls.Add(password);
            Controls.Add(DbUSER);
            Controls.Add(username);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            FormStyle = FormStyles.ActionBar_48;
            helpProvider.SetHelpKeyword(this, "Help");
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(370, 370);
            MinimumSize = new Size(370, 370);
            Name = "MainForm";
            Padding = new Padding(3, 72, 3, 3);
            helpProvider.SetShowHelp(this, true);
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DeNavify";
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
        private Label label3;
        private TextBox SymbolBox;
        private HelpProvider helpProvider1;
        private HelpProvider helpProvider2;
        private HelpProvider helpProvider;
        private MaterialSkin.Controls.MaterialButton DeNavify;
        private ImageList imageList1;
    }
}