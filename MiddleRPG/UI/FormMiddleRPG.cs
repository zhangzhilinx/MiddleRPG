using MiddleRPG.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace MiddleRPG
{
    public partial class FormMiddleRPG : Form
    {
        private enum Winner
        {
            Waiting,    // 胜负未分
            Heros,      // 英雄/我方
            Monsters    // 怪物/敌方
        };

        private const string pathGameArchive = "./game.sav";

        private Dictionary<string, UnitAgent> agentHeros = new Dictionary<string, UnitAgent>();
        private Dictionary<string, UnitAgent> agentMonsters = new Dictionary<string, UnitAgent>();

        /// <summary>
        /// 回合循环控制器
        /// 由于它实现了IDisposable接口，所以在Dispose方法中执行它的Dispose()方法
        /// </summary>
        private IEnumerator<Winner> roundLoop = null;

        public FormMiddleRPG()
        {
            InitializeComponent();

            Hero[] initHeros = new Hero[] {
                new Archon(Guid.NewGuid().ToString()),
                new DarkTemplar(Guid.NewGuid().ToString()),
                new HighTemplar(Guid.NewGuid().ToString())
            };
            Monster[] initMonsters = new Monster[] {
                new Queen(Guid.NewGuid().ToString()),
                new Hydralisk(Guid.NewGuid().ToString()),
                new Ultralisk(Guid.NewGuid().ToString())
            };

            foreach (Hero hero in initHeros)
            {
                UnitAgent gameUnit = new UnitAgent(hero);
                gameUnit.AttackMouseDown += GameUnitHero_MouseDown;
                agentHeros.Add(hero.Id, gameUnit);
                flayoutHeros.Controls.Add(gameUnit);
            }
            foreach (Monster monster in initMonsters)
            {
                UnitAgent gameUnit = new UnitAgent(monster);
                gameUnit.DragEnter += GameUnitMonster_DragEnter;
                gameUnit.DragDrop += GameUnitMonster_DragDrop;
                gameUnit.AllowDrop = true;
                agentMonsters.Add(monster.Id, gameUnit);
                flayoutMonsters.Controls.Add(gameUnit);
            }

            // 根据目前场上的所有单位，创建回合循环，false代表我方先手
            roundLoop = ControlRound(false).GetEnumerator();
            // 回合循环创建后，必须先迭代一次，执行战场回合初始化
            roundLoop.MoveNext();
            timerRound.Interval = 1000;
        }

        private IEnumerable<Winner> ControlRound(bool isTurnToMonster)
        {
            Winner winner = Winner.Waiting;
            foreach (
                UnitAgent agentHero
                in agentHeros.Values.Where(x => (x.Unit?.IsAlive ?? false)))
            {   // 初始化英雄们的GameUnit的可攻击状态
                agentHero.PermitAttack = !isTurnToMonster;
            }
            lblRoundIndicator.Text = isTurnToMonster ? "敌方回合" : "我方回合";
            if (isTurnToMonster)
                timerRound.Start();
            yield return winner;

            Random rand = new Random();
            while (true)
            {   //进入循环回合判定
                while (winner != Winner.Waiting)
                {   // 已决定胜负之后再不会执行回合任务
                    yield return winner;
                }
                if (isTurnToMonster)
                {
                    RefreshAvatarMonsters();
                    //List<Monster> livingMonsters = agentMonsters.Values
                    //    .Where(x => x?.Unit?.IsAlive ?? false)
                    //    .Select(x => (Monster)x.Unit)
                    //    .ToList();
                    List<Monster> livingMonsters = (from x in agentMonsters.Values
                                                    where (x?.Unit?.IsAlive ?? false)
                                                    select (Monster)x.Unit)
                                                   .ToList();
                    if (livingMonsters.Count > 0)
                    {   // 仍有怪物存活
                        List<UnitAgent> livingAgentHeros;
                        foreach (Monster monster in livingMonsters)
                        {   //怪物攻击英雄，按顺序选择怪物
                            livingAgentHeros = agentHeros.Values
                                .Where(x => x?.Unit?.IsAlive ?? false)
                                .ToList();

                            if (livingAgentHeros.Count > 0)
                            {   // 还有存活的英雄
                                monster.Attack(livingAgentHeros[rand.Next(0, livingAgentHeros.Count)].Unit);
                                yield return winner;
                            }
                            else
                            {   // 没有存活的英雄
                                break;
                            }
                        }
                        RefreshAvatarHeros();
                        //检查是否有英雄存活，若有则切换到我方回合，否则判定敌方获胜
                        timerRound.Stop();
                        livingAgentHeros = agentHeros.Values
                            .Where(x => x?.Unit?.IsAlive ?? false)
                            .ToList();
                        Console.WriteLine(livingAgentHeros.Count);
                        if (livingAgentHeros.Count > 0)
                        {
                            foreach (UnitAgent agentHero in livingAgentHeros)
                            {   //令所有活着的英雄GameUnit处于可攻击状态
                                agentHero.PermitAttack = true;
                            }
                            //随后切换到我方回合
                            isTurnToMonster = !isTurnToMonster;
                            lblRoundIndicator.Text = "我方回合";
                        }
                        else
                        {   //判定敌方胜利
                            winner = Winner.Monsters;
                            lblRoundIndicator.Text = "敌方获胜";
                        }
                    }
                    else
                    {   //没有怪物存活了
                        timerRound.Stop();
                        winner = Winner.Heros;
                        lblRoundIndicator.Text = "玩家获胜";
                    }
                }
                else
                {
                    //查询是否怪物全部死亡
                    List<Monster> livingMonsters = (from x in agentMonsters.Values
                                                    where (x?.Unit?.IsAlive ?? false)
                                                    select (Monster)x.Unit)
                                                   .ToList();
                    if (livingMonsters.Count > 0)
                    {
                        //查询当前所有英雄的GameUnit，是否已经全部攻击过
                        bool found = agentHeros.Values
                            .Where(x => (x?.Unit?.IsAlive ?? false))
                            .Any(x => x.PermitAttack);
                        if (!found)
                        {   //如果全部都攻击过，则切换回合，同时启动计时
                            isTurnToMonster = !isTurnToMonster;
                            lblRoundIndicator.Text = "敌方回合";
                            timerRound.Start();
                        }
                        yield return winner;
                    }
                    else
                    {
                        winner = Winner.Heros;
                        lblRoundIndicator.Text = "玩家获胜";
                        RefreshAvatarMonsters();
                    }
                }
            }
        }

        private void ReadyForRound()
        {
            roundLoop.MoveNext();
        }

        public void RefreshAvatarMonsters()
        {
            foreach (
                UnitAgent agentMonster
                in agentMonsters.Values.Where(x => x.Unit != null))
            {   //刷新怪物们的头像
                agentMonster.RedrawAvatar();
            }
        }

        public void RefreshAvatarHeros()
        {
            foreach (
                UnitAgent agentHero
                in agentHeros.Values.Where(x => x.Unit != null))
            {   //刷新英雄们的头像
                agentHero.RedrawAvatar();
            }
        }

        private void GameUnitHero_MouseDown(object sender, MouseEventArgs e)
        {
            Hero hero = (Hero)((UnitAgent)sender).Unit;
            if (sender is UnitAgent && hero != null && hero is Hero)
            {
                DoDragDrop(hero == null ? "None" : hero.Id, DragDropEffects.Move);
            }
        }

        private void GameUnitMonster_DragEnter(object sender, DragEventArgs e)
        {
            string id = (string)e.Data.GetData(DataFormats.Text);
            Monster monster = (Monster)((UnitAgent)sender).Unit;

            if (agentHeros.ContainsKey(id)
                && (agentHeros[id].Unit?.IsAlive ?? false)
                && (monster?.IsAlive ?? false))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void GameUnitMonster_DragDrop(object sender, DragEventArgs e)
        {
            string id = (string)e.Data.GetData(DataFormats.Text);
            if (agentHeros.ContainsKey(id))
            {
                BattleUnit battleUnitHero = (BattleUnit)agentHeros[id]?.Unit;
                if (battleUnitHero != null)
                {
                    Hero hero = (Hero)battleUnitHero;
                    Monster monster = (Monster)((UnitAgent)sender).Unit;
                    if (hero.IsAlive && (monster?.IsAlive ?? false))
                    {
                        hero.Attack(monster);
                        if (!monster.IsAlive)
                        {
                            ((UnitAgent)sender).AllowDrop = false;
                        }
                        if (agentHeros.ContainsKey(id))
                        {   // 关闭该Hero对应的控件的攻击许可
                            UnitAgent agentHero = agentHeros[id];
                            if (agentHero != null)
                            {
                                agentHero.PermitAttack = false;
                            }
                        }
                        ReadyForRound();
                    }
                }
            }
        }

        private void TimerRound_Tick(object sender, EventArgs e)
        {
            ReadyForRound();
        }

        private void btnSceneSave_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathGameArchive, FileMode.Create))
            {
                formatter.Serialize(stream, agentHeros);
                formatter.Serialize(stream, agentMonsters);
            }
        }
    }
}
