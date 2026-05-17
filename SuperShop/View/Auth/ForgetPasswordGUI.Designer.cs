namespace SuperShop.View.Auth
{
    partial class ForgetPasswordGUI
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
            UsernametextBox = new TextBox();
            UserNamelabel = new Label();
            AdminIDtextBox = new TextBox();
            AdminIDlabel = new Label();
            SecurityAnswerlabel = new Label();
            SecurityAnswertextBox = new TextBox();
            SecurityQuestioncomboBox = new ComboBox();
            SecurityQuestionlabel = new Label();
            Validatebutton = new Button();
            ConfirmPasswordtextBox = new TextBox();
            ConfirmPasswordlabel = new Label();
            NewPasswordtextBox = new TextBox();
            NewPasswordlabel = new Label();
            ChangePasswordbutton = new Button();
            SuspendLayout();
            // 
            // UsernametextBox
            // 
            UsernametextBox.Anchor = AnchorStyles.None;
            UsernametextBox.Location = new Point(187, 47);
            UsernametextBox.Name = "UsernametextBox";
            UsernametextBox.Size = new Size(314, 26);
            UsernametextBox.TabIndex = 3;
            // 
            // UserNamelabel
            // 
            UserNamelabel.Anchor = AnchorStyles.None;
            UserNamelabel.AutoSize = true;
            UserNamelabel.Location = new Point(86, 50);
            UserNamelabel.Name = "UserNamelabel";
            UserNamelabel.Size = new Size(77, 19);
            UserNamelabel.TabIndex = 2;
            UserNamelabel.Text = "Username :";
            // 
            // AdminIDtextBox
            // 
            AdminIDtextBox.Anchor = AnchorStyles.None;
            AdminIDtextBox.Font = new Font("Times New Roman", 14.25F);
            AdminIDtextBox.Location = new Point(187, 96);
            AdminIDtextBox.Name = "AdminIDtextBox";
            AdminIDtextBox.Size = new Size(314, 29);
            AdminIDtextBox.TabIndex = 168;
            // 
            // AdminIDlabel
            // 
            AdminIDlabel.Anchor = AnchorStyles.None;
            AdminIDlabel.AutoSize = true;
            AdminIDlabel.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AdminIDlabel.Location = new Point(80, 98);
            AdminIDlabel.Name = "AdminIDlabel";
            AdminIDlabel.Size = new Size(76, 19);
            AdminIDlabel.TabIndex = 167;
            AdminIDlabel.Text = "Admin ID :";
            // 
            // SecurityAnswerlabel
            // 
            SecurityAnswerlabel.Anchor = AnchorStyles.None;
            SecurityAnswerlabel.AutoSize = true;
            SecurityAnswerlabel.Font = new Font("Times New Roman", 12F);
            SecurityAnswerlabel.Location = new Point(37, 202);
            SecurityAnswerlabel.Name = "SecurityAnswerlabel";
            SecurityAnswerlabel.Size = new Size(115, 19);
            SecurityAnswerlabel.TabIndex = 166;
            SecurityAnswerlabel.Text = "Security Answer :";
            // 
            // SecurityAnswertextBox
            // 
            SecurityAnswertextBox.Anchor = AnchorStyles.None;
            SecurityAnswertextBox.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SecurityAnswertextBox.Location = new Point(187, 200);
            SecurityAnswertextBox.Name = "SecurityAnswertextBox";
            SecurityAnswertextBox.Size = new Size(314, 26);
            SecurityAnswertextBox.TabIndex = 165;
            // 
            // SecurityQuestioncomboBox
            // 
            SecurityQuestioncomboBox.Anchor = AnchorStyles.None;
            SecurityQuestioncomboBox.Font = new Font("Times New Roman", 14.25F);
            SecurityQuestioncomboBox.FormattingEnabled = true;
            SecurityQuestioncomboBox.Items.AddRange(new object[] { "What was the name of your first school?", "In which city or village were you born?", "What is the name of your childhood best friend?", "What was your first pet’s name?", "What is your favorite teacher’s name?", "What was the model of your first mobile phone?", "What is your mother’s middle name?", "What is the name of the street you grew up on?", "What was your dream job as a child?" });
            SecurityQuestioncomboBox.Location = new Point(187, 150);
            SecurityQuestioncomboBox.Name = "SecurityQuestioncomboBox";
            SecurityQuestioncomboBox.Size = new Size(314, 29);
            SecurityQuestioncomboBox.TabIndex = 164;
            // 
            // SecurityQuestionlabel
            // 
            SecurityQuestionlabel.Anchor = AnchorStyles.None;
            SecurityQuestionlabel.AutoSize = true;
            SecurityQuestionlabel.Font = new Font("Times New Roman", 12F);
            SecurityQuestionlabel.Location = new Point(27, 151);
            SecurityQuestionlabel.Name = "SecurityQuestionlabel";
            SecurityQuestionlabel.Size = new Size(123, 19);
            SecurityQuestionlabel.TabIndex = 163;
            SecurityQuestionlabel.Text = "Security Question :";
            // 
            // Validatebutton
            // 
            Validatebutton.Anchor = AnchorStyles.None;
            Validatebutton.BackColor = Color.BurlyWood;
            Validatebutton.Location = new Point(261, 244);
            Validatebutton.Name = "Validatebutton";
            Validatebutton.Size = new Size(152, 33);
            Validatebutton.TabIndex = 169;
            Validatebutton.Text = "Validate";
            Validatebutton.UseVisualStyleBackColor = false;
            Validatebutton.Click += Validatebutton_Click;
            // 
            // ConfirmPasswordtextBox
            // 
            ConfirmPasswordtextBox.Anchor = AnchorStyles.None;
            ConfirmPasswordtextBox.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ConfirmPasswordtextBox.Location = new Point(187, 346);
            ConfirmPasswordtextBox.Name = "ConfirmPasswordtextBox";
            ConfirmPasswordtextBox.Size = new Size(314, 26);
            ConfirmPasswordtextBox.TabIndex = 173;
            // 
            // ConfirmPasswordlabel
            // 
            ConfirmPasswordlabel.Anchor = AnchorStyles.None;
            ConfirmPasswordlabel.AutoSize = true;
            ConfirmPasswordlabel.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ConfirmPasswordlabel.Location = new Point(27, 350);
            ConfirmPasswordlabel.Name = "ConfirmPasswordlabel";
            ConfirmPasswordlabel.Size = new Size(129, 19);
            ConfirmPasswordlabel.TabIndex = 172;
            ConfirmPasswordlabel.Text = "Confirm Password :";
            // 
            // NewPasswordtextBox
            // 
            NewPasswordtextBox.Anchor = AnchorStyles.None;
            NewPasswordtextBox.Location = new Point(187, 298);
            NewPasswordtextBox.Name = "NewPasswordtextBox";
            NewPasswordtextBox.Size = new Size(314, 26);
            NewPasswordtextBox.TabIndex = 171;
            // 
            // NewPasswordlabel
            // 
            NewPasswordlabel.Anchor = AnchorStyles.None;
            NewPasswordlabel.AutoSize = true;
            NewPasswordlabel.Location = new Point(51, 300);
            NewPasswordlabel.Name = "NewPasswordlabel";
            NewPasswordlabel.Size = new Size(110, 19);
            NewPasswordlabel.TabIndex = 170;
            NewPasswordlabel.Text = "New Password :";
            // 
            // ChangePasswordbutton
            // 
            ChangePasswordbutton.Anchor = AnchorStyles.None;
            ChangePasswordbutton.BackColor = Color.BurlyWood;
            ChangePasswordbutton.Location = new Point(261, 394);
            ChangePasswordbutton.Name = "ChangePasswordbutton";
            ChangePasswordbutton.Size = new Size(152, 33);
            ChangePasswordbutton.TabIndex = 174;
            ChangePasswordbutton.Text = "Change Password";
            ChangePasswordbutton.UseVisualStyleBackColor = false;
            ChangePasswordbutton.Click += ChangePasswordbutton_Click;
            // 
            // ForgetPasswordGUI
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(602, 455);
            Controls.Add(ChangePasswordbutton);
            Controls.Add(ConfirmPasswordtextBox);
            Controls.Add(ConfirmPasswordlabel);
            Controls.Add(NewPasswordtextBox);
            Controls.Add(NewPasswordlabel);
            Controls.Add(Validatebutton);
            Controls.Add(AdminIDtextBox);
            Controls.Add(AdminIDlabel);
            Controls.Add(SecurityAnswerlabel);
            Controls.Add(SecurityAnswertextBox);
            Controls.Add(SecurityQuestioncomboBox);
            Controls.Add(SecurityQuestionlabel);
            Controls.Add(UsernametextBox);
            Controls.Add(UserNamelabel);
            Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "ForgetPasswordGUI";
            Text = "ForgetPasswordGUI";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox UsernametextBox;
        private Label UserNamelabel;
        private TextBox AdminIDtextBox;
        private Label AdminIDlabel;
        private Label SecurityAnswerlabel;
        private TextBox SecurityAnswertextBox;
        private ComboBox SecurityQuestioncomboBox;
        private Label SecurityQuestionlabel;
        private Button Validatebutton;
        private TextBox ConfirmPasswordtextBox;
        private Label ConfirmPasswordlabel;
        private TextBox NewPasswordtextBox;
        private Label NewPasswordlabel;
        private Button ChangePasswordbutton;
    }
}