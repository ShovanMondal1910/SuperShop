namespace SuperShop.View.Auth
{
    partial class LogInGUI
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
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            groupBox = new GroupBox();
            PasswordErrorlabel = new Label();
            UserNameErrorlabel = new Label();
            Cancelbutton = new Button();
            Loginbutton = new Button();
            ShowPasswordcheckBox = new CheckBox();
            PasswordtextBox = new TextBox();
            Passwordlabel = new Label();
            UsernametextBox = new TextBox();
            UserNamelabel = new Label();
            ForgerPasswordlabel = new Label();
            groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // groupBox
            // 
            groupBox.Controls.Add(PasswordErrorlabel);
            groupBox.Controls.Add(UserNameErrorlabel);
            groupBox.Controls.Add(Cancelbutton);
            groupBox.Controls.Add(Loginbutton);
            groupBox.Controls.Add(ShowPasswordcheckBox);
            groupBox.Controls.Add(PasswordtextBox);
            groupBox.Controls.Add(Passwordlabel);
            groupBox.Controls.Add(UsernametextBox);
            groupBox.Controls.Add(UserNamelabel);
            groupBox.Font = new Font("Times New Roman", 12F);
            groupBox.Location = new Point(63, 66);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(497, 232);
            groupBox.TabIndex = 1;
            groupBox.TabStop = false;
            groupBox.Text = "Login by UserName /Phone Number";
            // 
            // PasswordErrorlabel
            // 
            PasswordErrorlabel.AutoSize = true;
            PasswordErrorlabel.Location = new Point(135, 147);
            PasswordErrorlabel.Name = "PasswordErrorlabel";
            PasswordErrorlabel.Size = new Size(0, 19);
            PasswordErrorlabel.TabIndex = 8;
            // 
            // UserNameErrorlabel
            // 
            UserNameErrorlabel.AutoSize = true;
            UserNameErrorlabel.Location = new Point(135, 81);
            UserNameErrorlabel.Name = "UserNameErrorlabel";
            UserNameErrorlabel.Size = new Size(0, 19);
            UserNameErrorlabel.TabIndex = 7;
            // 
            // Cancelbutton
            // 
            Cancelbutton.Location = new Point(305, 180);
            Cancelbutton.Name = "Cancelbutton";
            Cancelbutton.Size = new Size(99, 26);
            Cancelbutton.TabIndex = 6;
            Cancelbutton.Text = "Cancel";
            Cancelbutton.UseVisualStyleBackColor = true;
            // 
            // Loginbutton
            // 
            Loginbutton.Location = new Point(132, 180);
            Loginbutton.Name = "Loginbutton";
            Loginbutton.Size = new Size(99, 26);
            Loginbutton.TabIndex = 5;
            Loginbutton.Text = "Login";
            Loginbutton.UseVisualStyleBackColor = true;
            Loginbutton.Click += Loginbutton_Click;
            // 
            // ShowPasswordcheckBox
            // 
            ShowPasswordcheckBox.AutoSize = true;
            ShowPasswordcheckBox.Location = new Point(414, 111);
            ShowPasswordcheckBox.Name = "ShowPasswordcheckBox";
            ShowPasswordcheckBox.Size = new Size(63, 23);
            ShowPasswordcheckBox.TabIndex = 4;
            ShowPasswordcheckBox.Text = "Show";
            ShowPasswordcheckBox.UseVisualStyleBackColor = true;
            // 
            // PasswordtextBox
            // 
            PasswordtextBox.Location = new Point(132, 108);
            PasswordtextBox.Name = "PasswordtextBox";
            PasswordtextBox.PasswordChar = '*';
            PasswordtextBox.Size = new Size(272, 26);
            PasswordtextBox.TabIndex = 3;
            // 
            // Passwordlabel
            // 
            Passwordlabel.AutoSize = true;
            Passwordlabel.Location = new Point(32, 111);
            Passwordlabel.Name = "Passwordlabel";
            Passwordlabel.Size = new Size(76, 19);
            Passwordlabel.TabIndex = 2;
            Passwordlabel.Text = "Password :";
            // 
            // UsernametextBox
            // 
            UsernametextBox.Location = new Point(132, 42);
            UsernametextBox.Name = "UsernametextBox";
            UsernametextBox.Size = new Size(272, 26);
            UsernametextBox.TabIndex = 1;
            // 
            // UserNamelabel
            // 
            UserNamelabel.AutoSize = true;
            UserNamelabel.Location = new Point(32, 45);
            UserNamelabel.Name = "UserNamelabel";
            UserNamelabel.Size = new Size(77, 19);
            UserNamelabel.TabIndex = 0;
            UserNamelabel.Text = "Username :";
            // 
            // ForgerPasswordlabel
            // 
            ForgerPasswordlabel.AutoSize = true;
            ForgerPasswordlabel.Font = new Font("Times New Roman", 12F);
            ForgerPasswordlabel.Location = new Point(477, 279);
            ForgerPasswordlabel.Name = "ForgerPasswordlabel";
            ForgerPasswordlabel.Size = new Size(121, 19);
            ForgerPasswordlabel.TabIndex = 2;
            ForgerPasswordlabel.Text = "Forger Password?";
            ForgerPasswordlabel.Click += ForgerPasswordlabel_Click;
            // 
            // LogInGUI
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(633, 330);
            Controls.Add(ForgerPasswordlabel);
            Controls.Add(groupBox);
            Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "LogInGUI";
            Text = "LogInGUI";
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private GroupBox groupBox;
        private Button Loginbutton;
        private CheckBox ShowPasswordcheckBox;
        private TextBox PasswordtextBox;
        private Label Passwordlabel;
        private TextBox UsernametextBox;
        private Label UserNamelabel;
        private Label PasswordErrorlabel;
        private Label UserNameErrorlabel;
        private Button Cancelbutton;
        private Label ForgerPasswordlabel;
    }
}