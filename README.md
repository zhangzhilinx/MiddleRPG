# MiddleRPG

> 一个简易的基于WinForm实现的RPG Demo
>
> 作为.Net课程的作业





## 安装要求

* .Net Framework 4.0
* Visual Studio





## 项目介绍

### 支持的特性

* 简单且愚蠢的人机对战
* 使用DragDrop进行游戏
* 单位支持附加静态特效：被攻击时、死亡时
* 使用了事件与委托，更好的代码设计
* 别出心裁地利用yield迭代实现更好的回合循环控制
  * 完全重构SimpleRPG版本的代码
    * 支持更复杂的情况，具有一定的扩展性、复用性：比如更多单位
  * 免于设计状态机时过于繁杂的代码
* 尝试应用Linq查询表达式与匿名函数
* 高DPI显示支持
* 禁用视觉样式以支持修改进度条颜色



### 已解决的错误

* 错误使用控件造成的潜在内存泄漏

  **简述**：错误使用控件造成潜在的内存泄漏

  **时机**：当多次重复读取游戏存档时

  **描述**：自定义控件UnitAgent添加到FlowLayoutPanel，当Panel清空Controls时，无法正常释放UnitAgent的内存

  **解决**：

  刚开始查找了很多对象，分析了可能的依赖关系，但仍然无法定位到具体的错误原因
  甚至怀疑是因为FlowLayoutPanel导致内存泄漏，确实发现存在有些做法会导致内存泄漏，
  比如说FlowLayoutPanel.Controls.Clear()并不会同时释放控件的资源，但问题并未得到解决
  探查器也无法确认是何处存在引用，导致这些UnitAgent的内存始终可达，无法被GC回收

  最终重审代码，怀疑UnitAgent中ToolTip控件的使用是不正确的，忽略了该控件的Dispose()
  建议的做法是通过控件设计器，它将把ToolTip控件加入到components(IContainer)中
  在控件执行Dispose()时，设计器提供的的Dispose()实现会自动同时清理components中的所有控件的资源
  修复后重新运行程序，印证了之前的怀疑是正确的

  在这些控件销毁前，注意取消订阅控件的所有事件，否则也可能因相互引用而使GC无法回收

  另一方面，ControlRound()在整个游戏运行过程中是始终具有状态的，
  当ControlRound()通过yield return暂时退出“函数”的前一刻所持有的所有变量/实例均会被保留
  （尤其是在一些局部块中yield return，局部变量则在yield return退出MoveNext()后仍然有效）
  那么如果此时恰好有对外部对象的引用，同样会被保留。那么这些外部对象仍然会被GC标记为可达
  从而无法在需要的时候及时释放掉这些对象的内存
  所以在yield return之前务必对某些需要及时释放的对象变量置null以解除引用
  （注：ControlRound()实际上会被编译成一个具有状态的类）

  **总结**：
  	不可盲目依赖GC，尽管GC极大地减轻了开发者的麻烦，但仍需要开发者能够应对内存问题
  	尤其是非托管资源，更应该小心，尽管此处的泄露不是因为非托管内存导致的
  	另一方面，内存管理需要十分谨慎，即使在具有GC的语言上，发生意外时亦如此棘手，
  	在手动管理内存的语言上，更应谨慎



### 计划的特性

* 更完善的事件机制
  * 战场事件
    * 回合切换
    * 战败 / 战胜
  * 单位事件
    * 选中
    * 攻击
    * 死亡
    * 生命值改变
    * 生命垂危
    * 遭受攻击

* 游戏音效
  * 可结合事件机制实现
  * 背景音乐
  * 情报语音
  * 单位语音



## License

C# Code: GPL v3

All StarCraft 2 design resources are copyrighted by Blizzard. These resources are not used for any commercial purposes here.

