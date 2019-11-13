using MiddleRPG.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiddleRPG
{
    //[ToolboxItem(true)]
    //[ToolboxItemFilter("自定义控件")]
    //[DisplayName("GameRole")]
    [Serializable]
    public partial class UnitAgent : UserControl
    {
        private ToolTip tooltip = new ToolTip();
        private BattleUnit unit;
        public BattleUnit Unit {
            get { return unit; }
            private set {
                if(unit != null)
                {
                    unit.HealthChanged -= OnUnitHealthChanged;
                    unit.UnderAttack -= OnUnitUnderAttack;
                }
                unit = value;
                if(unit != null)
                {
                    unit.HealthChanged += new BattleUnit.OnHealthChangedHandler(OnUnitHealthChanged);
                    unit.UnderAttack += new BattleUnit.OnUnderAttackHandler(OnUnitUnderAttack);
                    RefreshCanvas();
                    tooltip.SetToolTip(
                        picAvatar,
                        string.Format(
                            @"力量：{0:D}{1}敏捷：{2:D}{3}智力：{4:D}{5}防御：{6:D}{7}生命：{8:D}",
                            unit.Power, Environment.NewLine,
                            unit.Agility, Environment.NewLine,
                            unit.Intelligence, Environment.NewLine,
                            unit.Defense, Environment.NewLine,
                            unit.Life));
                }
            }
        }

        public bool PermitAttack { get; set; } = false;

        public event MouseEventHandler AttackMouseDown;

        public UnitAgent()
        {
            InitializeComponent();
        }

        public UnitAgent(BattleUnit unit)
        {
            InitializeComponent();
            Unit = unit;
        }

        //private const uint PBM_SETSTATE = 0x0410;
        //private enum ProgressBarState { PBST_NORMAL = 1, PBST_ERROR, PBST_PAUSED }
        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        //private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        //private static void SetProgressBarState(ProgressBar progressBar, int state)
        //{
        //    SendMessage(progressBar.Handle, PBM_SETSTATE, (IntPtr)state, IntPtr.Zero);
        //}

        private Bitmap OverlapImage(Bitmap background, Bitmap foreground)
        {
            if (foreground.Size != background.Size)
            {
                throw new ArgumentException("The two images entered are inconsistent in size.");
            }

            Bitmap output = new Bitmap(background);
            using (Graphics g = Graphics.FromImage(output))
            {
                g.DrawImage(foreground, new Rectangle(new Point(0, 0), background.Size));
            }

            return output;
        }

        private Bitmap DrawEffect(Bitmap effect, Size size)
        {
            int width = size.Width, height = size.Height;
            Bitmap output = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(output))
            {
                g.DrawImage(
                    effect,
                    new Rectangle(width >> 2, height >> 2, width >> 1, height >> 1));
            }
            return output;
        }

        public void RedrawAvatar()
        {
            if(Unit != null)
            {
                picAvatar.Image = Unit.IsAlive && Unit.Effect.Dead != null
                    ? Unit.Avatar
                    : OverlapImage(Unit.Avatar, Unit.Effect.Dead);
            }
        }

        public void RedrawHP()
        {
            if(Unit != null)
            {
                int percentHealth = Convert.ToInt32(
                    Convert.ToDouble(Unit.Health) / Convert.ToDouble(Unit.Life) * 100.0);
                barHP.Value = percentHealth;
                barHP.ForeColor = (percentHealth > 75)
                    ? Color.Green
                    : ((percentHealth > 25) ? Color.Orange : Color.Red);
                //UtilProgressBar.SetProgressBarState(barHP, (percentHealth > 75) ? 1 : ((percentHealth > 25) ? 3 : 2));
                //SetProgressBarState(barHP, (percentHealth > 75) ? 1 : ((percentHealth > 25) ? 3 : 2));
            }
        }

        public void RefreshCanvas()
        {
            if(Unit != null)
            {
                lblName.Text = unit.Name;
                lblName.ForeColor = unit.IsAlive ? Color.Green : Color.Red;
                RedrawAvatar();
                RedrawHP();
            }
        }

        private void OnUnitHealthChanged(int life)
        {
            lblName.ForeColor = Unit.IsAlive ? Color.Green : Color.Red;
            RedrawHP();
        }

        private void OnUnitUnderAttack(BattleUnit attacker, int power)
        {
            picAvatar.Image = OverlapImage(
                Unit.Avatar,
                DrawEffect(attacker.Effect.Attack, Unit.Avatar.Size));
        }

        private void PicAvatar_MouseDown(object sender, MouseEventArgs e)
        {
            if(PermitAttack && AttackMouseDown != null)
            {
                AttackMouseDown(this, e);
            }
        }

        //public static class UtilProgressBar
        //{
        //    private const uint PBM_SETSTATE = 0x0410;
        //    public enum ProgressBarColor { GREEN = 1, RED, YELLOW }
        //    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        //    private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        //    public static void SetProgressBarState(this ProgressBar progressBar, int state)
        //    {
        //        SendMessage(progressBar.Handle, PBM_SETSTATE, (IntPtr)state, IntPtr.Zero);
        //    }
        //}
    }
}
