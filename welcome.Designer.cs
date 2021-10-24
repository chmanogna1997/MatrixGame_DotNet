
namespace TermProj
{
    partial class welcome
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
            this.Name_lbl = new System.Windows.Forms.Label();
            this.Welcome_lbl = new System.Windows.Forms.Label();
            this.Name_txt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Name_lbl
            // 
            this.Name_lbl.AutoSize = true;
            this.Name_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Name_lbl.Location = new System.Drawing.Point(59, 132);
            this.Name_lbl.Name = "Name_lbl";
            this.Name_lbl.Size = new System.Drawing.Size(214, 29);
            this.Name_lbl.TabIndex = 0;
            this.Name_lbl.Text = "Enter Your Name:";
            // 
            // Welcome_lbl
            // 
            this.Welcome_lbl.AutoSize = true;
            this.Welcome_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 27F);
            this.Welcome_lbl.Location = new System.Drawing.Point(44, 39);
            this.Welcome_lbl.Name = "Welcome_lbl";
            this.Welcome_lbl.Size = new System.Drawing.Size(510, 52);
            this.Welcome_lbl.TabIndex = 2;
            this.Welcome_lbl.Text = "Welcome to the game ...";
            // 
            // Name_txt
            // 
            this.Name_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Name_txt.Location = new System.Drawing.Point(303, 132);
            this.Name_txt.Name = "Name_txt";
            this.Name_txt.Size = new System.Drawing.Size(275, 36);
            this.Name_txt.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.button1.Location = new System.Drawing.Point(181, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 40);
            this.button1.TabIndex = 4;
            this.button1.Text = "Let\'s Start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Start_btn);
            // 
            // welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(644, 335);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Name_txt);
            this.Controls.Add(this.Welcome_lbl);
            this.Controls.Add(this.Name_lbl);
            this.Name = "welcome";
            this.Text = "welcome";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Name_lbl;
        private System.Windows.Forms.Label Welcome_lbl;
        private System.Windows.Forms.TextBox Name_txt;
        private System.Windows.Forms.Button button1;
    }
}