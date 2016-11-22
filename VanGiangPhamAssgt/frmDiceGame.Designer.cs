namespace VanGiangPhamAssgt
{
    partial class frm_DiceGame
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_DiceGame));
            this.tmr_SlideDownUp = new System.Windows.Forms.Timer(this.components);
            this.tmr_ShowResults = new System.Windows.Forms.Timer(this.components);
            this.tmr_DisableBut = new System.Windows.Forms.Timer(this.components);
            this.tmr_PlayerTurn = new System.Windows.Forms.Timer(this.components);
            this.tmr_CompStart = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmr_SlideDownUp
            // 
            this.tmr_SlideDownUp.Interval = 10;
            this.tmr_SlideDownUp.Tick += new System.EventHandler(this.tmr_SlideDownUp_Tick);
            // 
            // tmr_ShowResults
            // 
            this.tmr_ShowResults.Interval = 3000;
            this.tmr_ShowResults.Tick += new System.EventHandler(this.tmr_ShowResults_Tick);
            // 
            // tmr_DisableBut
            // 
            this.tmr_DisableBut.Interval = 1700;
            this.tmr_DisableBut.Tick += new System.EventHandler(this.tmr_DisableBut_Tick);
            // 
            // tmr_PlayerTurn
            // 
            this.tmr_PlayerTurn.Interval = 1600;
            this.tmr_PlayerTurn.Tick += new System.EventHandler(this.tmr_PlayerTurn_Tick);
            // 
            // tmr_CompStart
            // 
            this.tmr_CompStart.Interval = 2000;
            this.tmr_CompStart.Tick += new System.EventHandler(this.tmr_CompStart_Tick);
            // 
            // frm_DiceGame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            // background image has sourced from: http://www.allmacwallpaper.com/retina-macbook-pro-wallpapers/Abstract/Date/4
            this.BackgroundImage = global::VanGiangPhamAssgt.Properties.Resources.background; 
            this.ClientSize = new System.Drawing.Size(994, 621);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1010, 660);
            this.MinimumSize = new System.Drawing.Size(1010, 660);
            this.Name = "frm_DiceGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Six In One";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmr_SlideDownUp;
        private System.Windows.Forms.Timer tmr_ShowResults;
        private System.Windows.Forms.Timer tmr_DisableBut;
        private System.Windows.Forms.Timer tmr_PlayerTurn;
        private System.Windows.Forms.Timer tmr_CompStart;


    }
}

