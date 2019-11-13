namespace MiddleRPG
{
    partial class FormMiddleRPG
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                roundLoop?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMiddleRPG));
            this.flayoutHeros = new System.Windows.Forms.FlowLayoutPanel();
            this.flayoutMonsters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRoundIndicator = new System.Windows.Forms.Label();
            this.timerRound = new System.Windows.Forms.Timer(this.components);
            this.btnSceneLoad = new System.Windows.Forms.Button();
            this.btnSceneSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flayoutHeros
            // 
            this.flayoutHeros.AutoSize = true;
            this.flayoutHeros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flayoutHeros.BackColor = System.Drawing.Color.Transparent;
            this.flayoutHeros.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("flayoutHeros.BackgroundImage")));
            this.flayoutHeros.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flayoutHeros.Dock = System.Windows.Forms.DockStyle.Left;
            this.flayoutHeros.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flayoutHeros.Location = new System.Drawing.Point(0, 0);
            this.flayoutHeros.Name = "flayoutHeros";
            this.flayoutHeros.Padding = new System.Windows.Forms.Padding(10, 25, 10, 0);
            this.flayoutHeros.Size = new System.Drawing.Size(20, 561);
            this.flayoutHeros.TabIndex = 0;
            // 
            // flayoutMonsters
            // 
            this.flayoutMonsters.AutoSize = true;
            this.flayoutMonsters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flayoutMonsters.BackColor = System.Drawing.Color.Transparent;
            this.flayoutMonsters.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("flayoutMonsters.BackgroundImage")));
            this.flayoutMonsters.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flayoutMonsters.Dock = System.Windows.Forms.DockStyle.Right;
            this.flayoutMonsters.Location = new System.Drawing.Point(764, 0);
            this.flayoutMonsters.Name = "flayoutMonsters";
            this.flayoutMonsters.Padding = new System.Windows.Forms.Padding(10, 25, 10, 0);
            this.flayoutMonsters.Size = new System.Drawing.Size(20, 561);
            this.flayoutMonsters.TabIndex = 1;
            // 
            // lblRoundIndicator
            // 
            this.lblRoundIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRoundIndicator.AutoSize = true;
            this.lblRoundIndicator.BackColor = System.Drawing.Color.Transparent;
            this.lblRoundIndicator.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRoundIndicator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblRoundIndicator.Location = new System.Drawing.Point(312, 18);
            this.lblRoundIndicator.Name = "lblRoundIndicator";
            this.lblRoundIndicator.Size = new System.Drawing.Size(160, 46);
            this.lblRoundIndicator.TabIndex = 2;
            this.lblRoundIndicator.Text = "我方回合";
            this.lblRoundIndicator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerRound
            // 
            this.timerRound.Tick += new System.EventHandler(this.TimerRound_Tick);
            // 
            // btnSceneLoad
            // 
            this.btnSceneLoad.BackColor = System.Drawing.SystemColors.Control;
            this.btnSceneLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSceneLoad.Location = new System.Drawing.Point(226, 3);
            this.btnSceneLoad.Name = "btnSceneLoad";
            this.btnSceneLoad.Size = new System.Drawing.Size(142, 23);
            this.btnSceneLoad.TabIndex = 3;
            this.btnSceneLoad.Text = "恢复游戏";
            this.btnSceneLoad.UseVisualStyleBackColor = false;
            // 
            // btnSceneSave
            // 
            this.btnSceneSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSceneSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSceneSave.Location = new System.Drawing.Point(374, 3);
            this.btnSceneSave.Name = "btnSceneSave";
            this.btnSceneSave.Size = new System.Drawing.Size(142, 23);
            this.btnSceneSave.TabIndex = 4;
            this.btnSceneSave.Text = "保存游戏";
            this.btnSceneSave.UseVisualStyleBackColor = false;
            this.btnSceneSave.Click += new System.EventHandler(this.btnSceneSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.btnSceneLoad, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSceneSave, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 532);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(744, 29);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // FormMiddleRPG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblRoundIndicator);
            this.Controls.Add(this.flayoutMonsters);
            this.Controls.Add(this.flayoutHeros);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMiddleRPG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiddleRPG";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flayoutHeros;
        private System.Windows.Forms.FlowLayoutPanel flayoutMonsters;
        private System.Windows.Forms.Label lblRoundIndicator;
        private System.Windows.Forms.Timer timerRound;
        private System.Windows.Forms.Button btnSceneLoad;
        private System.Windows.Forms.Button btnSceneSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

