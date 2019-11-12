using MiddleRPG.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private readonly Dictionary<string, Hero> heros = new Dictionary<string, Hero>();
        private readonly Dictionary<string, Monster> monsters = new Dictionary<string, Monster>();
        private readonly Dictionary<string, List<UnitAgent>> agentHeros = new Dictionary<string, List<UnitAgent>>();
        private readonly Dictionary<string, List<UnitAgent>> agentMonsters = new Dictionary<string, List<UnitAgent>>();

        // 回合循环控制器
        // 由于它实现了IDisposable接口，所以在Dispose方法中执行它的Dispose()方法
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

            foreach (Hero hero in initHeros) {
                heros.Add(hero.Id, hero);
                UnitAgent gameUnit = new UnitAgent(hero);
                gameUnit.AttackMouseDown += GameUnitHero_MouseDown;
                agentHeros.Add(hero.Id, new List<UnitAgent>(new UnitAgent[] { gameUnit }));
                flayoutHeros.Controls.Add(gameUnit);
            }
            foreach (Monster monster in initMonsters) {
                monsters.Add(monster.Id, monster);
                UnitAgent gameUnit = new UnitAgent(monster);
                gameUnit.DragEnter += GameUnitMonster_DragEnter;
                gameUnit.DragDrop += GameUnitMonster_DragDrop;
                gameUnit.AllowDrop = true;
                agentMonsters.Add(monster.Id, new List<UnitAgent>(new UnitAgent[] { gameUnit }));
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
            foreach (List<UnitAgent> lstAgentHero in agentHeros.Values)
            {   // 初始化英雄们的GameUnit的可攻击状态
                foreach (
                    UnitAgent agentHero
                    in lstAgentHero.Where(
                        agentHero => agentHero.Unit != null
                        && agentHero.Unit.IsAlive))
                {
                    agentHero.PermitAttack = !isTurnToMonster;
                }
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
                    List<Monster> livingMonsters = monsters.Values
                        .Where(monster => monster.IsAlive).ToList();
                    if (livingMonsters.Count > 0)
                    {   // 仍有怪物存活
                        foreach (Monster monster in livingMonsters)
                        {   //怪物攻击英雄，按顺序选择怪物
                            List<UnitAgent> allAgentHeros = agentHeros.Values
                                .Aggregate(
                                    new List<UnitAgent>(),
                                    (acc, x) =>
                                        acc.Concat(
                                            x.Where(y => (y.Unit != null && y.Unit.IsAlive))
                                        ).ToList())
                                .ToList();
                            if (allAgentHeros.Count > 0)
                            {   // 还有存活的英雄
                                monster.Attack(allAgentHeros[rand.Next(0, allAgentHeros.Count)].Unit);
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
                        List<UnitAgent> livingAgentHeros = agentHeros.Values
                            .Aggregate(
                                new List<UnitAgent>(),
                                (acc, x) =>
                                    acc.Concat(
                                        x.Where(y => (y.Unit != null && y.Unit.IsAlive))
                                    ).ToList())
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
                    List<Monster> livingMonsters = monsters.Values
                        .Where(monster => monster.IsAlive).ToList();

                    if (livingMonsters.Count > 0)
                    {
                        //查询当前所有英雄的GameUnit，是否已经全部攻击过
                        bool found = false;
                        foreach (List<UnitAgent> lstAgentHeros in agentHeros.Values)
                        {   //检查所有活着的英雄GameUnit是否存在本回合未攻击过的
                            foreach (
                                UnitAgent lstHero in lstAgentHeros
                                .Where(lstHero =>
                                       lstHero.Unit != null && lstHero.Unit.IsAlive))
                            {
                                if (lstHero.PermitAttack)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
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
            foreach (List<UnitAgent> lstAgentMonsters in agentMonsters.Values)
            {   //刷新怪物们的头像
                foreach (
                    UnitAgent agentMonster
                    in lstAgentMonsters.Where(
                        agentMonster => agentMonster.Unit != null))
                {
                    agentMonster.RedrawAvatar();
                }
            }
        }

        public void RefreshAvatarHeros()
        {
            foreach (List<UnitAgent> lstAgentHeros in agentHeros.Values)
            {   //刷新英雄们的头像
                foreach (
                    UnitAgent agentHero in lstAgentHeros
                    .Where(agentHero => agentHero.Unit != null))
                {
                    agentHero.RedrawAvatar();
                }
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
            if (heros.ContainsKey(id) && heros[id].IsAlive && monster != null && monster.IsAlive)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void GameUnitMonster_DragDrop(object sender, DragEventArgs e)
        {
            string id = (string)e.Data.GetData(DataFormats.Text);
            if (heros.ContainsKey(id))
            {
                Hero hero = heros[id];
                Monster monster = (Monster)((UnitAgent)sender).Unit;
                if (hero.IsAlive && monster != null && monster.IsAlive)
                {
                    hero.Attack(monster);
                    if (!monster.IsAlive)
                    {
                        ((UnitAgent)sender).AllowDrop = false;
                    }
                    if (agentHeros.ContainsKey(id))
                    {   // 关闭该Hero对应的控件的攻击许可
                        foreach (UnitAgent gameUnit in agentHeros[id])
                        {
                            gameUnit.PermitAttack = false;
                        }
                    }
                    ReadyForRound();
                }
            }
        }

        private void TimerRound_Tick(object sender, EventArgs e)
        {
            ReadyForRound();
        }
    }
}
