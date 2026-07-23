# GrayTempest 项目参考手册

## Meta
| | |
|---|---|
| **ModId** | `GrayTempest` |
| **Version** | `0.1.0` |
| **Name** | 灰蛊 (The Gray Tempest) — Stellaris 灰蛊风暴 |
| **Framework** | STS2-RitsuLib v0.4.54 |
| **Game** | Slay the Spire 2 (min v0.108.0) |
| **SDK** | Godot.NET.Sdk/4.5.1 .NET 10.0 |
| **Type** | Character mod (has_pck + has_dll) |
| **Entry** | `Scripts/Entry.cs` `[ModInitializer]` |
| **Bootstrap** | Register Godot scripts → Content discovery → I18N (zhs primary/eng fallback) → 5 LocTableBridges → Harmony PatchAll → ScriptManagerBridge |

## 角色配置

| | |
|---|---|
| **类名** | `GrayTempestCharacter` (GrayTempest.Character) |
| **HP** | 50 |
| **金钱** | 99 |
| **充能球栏位** | 5 |
| **颜色** | `#808080` (灰色) / EnergyColor: `graytempest` |
| **基础视觉** | `CharacterAssetProfiles.Ironclad()` |
| **解锁条件** | Win as Ironclad |
| **代词** | it/it/its |
| **起始遗物** | `GrayGooCore` (纳米机器个体) |
| **起始牌组** | 4× Strike, 4× Defend, 1× NaniteSynthesis, 1× StrikeCraftDirective |

### 卡池
| 池名 | 类型 | 用途 |
|------|------|------|
| `GrayTempestCardPool` | Main | 角色主卡池 (蓝框灰底) |
| `GrayTempestCraftingCardPool` | Shared | 重构纳米军械库 |
| `GrayTempestRelicPool` | Character | 角色遗物池 |
| `GrayTempestSharedRelicPool` | Shared | 全局共享遗物池 |
| `GrayTempestAssociationRelicPool` | Shared | 关联遗物池 |
| `GrayTempestPotionPool` | Character | 药水池 |

---

## 卡牌总览 (31 张)

### Basic (4)

| 卡牌 | 费用 | 类型 | 目标 | 效果 | 升级 |
|------|------|------|------|------|------|
| **Strike** | 1 | Attack | AnyEnemy | 6 dmg | +3 (9) |
| **Defend** | 1 | Skill | Self | 5 Block | +3 (8) |
| **NaniteSynthesis** | 2 | Skill | Self | Reconstruct 5, Exhaust | 移除 Exhaust |
| **StrikeCraftDirective** | 1 | Skill | Self | 6 Block + 触发全部 Orb 被动 | +4 Block + 抽 1 |

### Common (8)

| 卡牌 | 费用 | 类型 | 目标 | 效果 | 关键词 |
|------|------|------|------|------|--------|
| **FieldImprove** | 2(1) | Skill | Self | 改进手牌一张 (花金币) | Exhaust, ImproveAction |
| **SimpleMissile** | 0 | Attack | All | 20(45) 全体伤害 | Exhaust, OneShot, Burst, ImproveCost(10) |
| **SimpleShieldGenerator** | 0 | Skill | Self | 8(20) Block + channel 1(3) ShieldGen | Exhaust, Retain, OneShot, ImproveCost(10) |
| **SimpleRecycle** | 1(0) | Skill | Self | 回收 N 张手牌 | OneShot, Recycle |
| **SimpleImprove** | 1 | Skill | Self | 改进手牌一张 (花金币) | OneShot |
| **Mining** | 1 | Attack | Enemy | 8 dmg + 获得 TinyResource | Energy |
| **GuidedBombardment** | 1 | Skill | Self | 4 Block + channel 1 OBU | — |
| **ComputeAllocation** | 1 | Skill | Self | 激发最右充能球 + 抽 {DrawCount} 张 | — |
| **SmallResources** | 0 | Power | Self | 获得 20(30) 金币 | OneShot, Recyclable(2[3]) |

### Uncommon (7)

| 卡牌 | 费用 | 类型 | 目标 | 效果 | 关键词 |
|------|------|------|------|------|--------|
| **Recycling** | 1(0) | Skill | Self | 回收手牌 1 张 | Exhaust, Recycle |
| **Decomposition** | 1 | Skill | Self | 回收所有状态/诅咒牌 | Exhaust, Recycle |
| **DoubleCast** | 1 | Skill | Self | 给一张 OneShot 牌 Replay 1 | — |
| **FieldUpgrade** | 3(0) | Skill | Self | 永久升级手牌+牌组 (兼容 IMultiUpgradable) | Exhaust, OneShot |
| **SustainedBombardment** | 1 | Power | Self | 每回合开始 channel 1 OBU | Counter |
| **TemporaryAssembly** | 2(1) | Power | Self | 战斗结束添加随机 OneShot 牌入牌组 | — |
| **SomeResources** | 0 | Power | Self | 获得 40(60) 金币 | OneShot, Recyclable(4[6]) |

### Rare (7)

| 卡牌 | 费用 | 类型 | 目标 | 效果 | 关键词 |
|------|------|------|------|------|--------|
| **Assimilate** | 2(1) | Attack | Enemy | 4(8) dmg, Kinetic 125%, 斩杀=获得等量 MaxHP | Exhaust, Kinetic |
| **EfficientReconstruct** | 2(1) | Power | Self | 获得 {Amount} 层: 重组时 gain 等量 MaxHP | — |
| **TargetedSearch** | 1 | Skill | Self | 抽 1 + 选 1 抽牌堆 + 1 弃牌堆入手 | Exhaust |
| **LargeResources** | 0 | Power | Self | 获得 60(90) 金币 | OneShot, Recyclable(6[9]) |
| **OrbitalOverlord** | 1 | Skill | Self | channel N 个 OBU (N=本场已 channel 总数) | — |
| **Armageddon** | 2(1) | Power | Self | OBU 造成 {Amount}% 额外伤害 | — |
| **GrayTempestStorm** | 2 | Attack | Enemy | 造成当前生命值等量伤害 | Kinetic |

### 纳米军械库 - 重构获得 (Pool, 4)

| 卡牌 | 费用 | 类型 | 目标 | 效果 | 关键词 |
|------|------|------|------|------|--------|
| **Laser** | 1 | Attack | Enemy | 9 dmg, Energy 125% vs unblocked | Recyclable(5), ImproveCost(8), MultiUpgrade×4 |
| **KineticCannon** | 1 | Attack | Enemy | 4×2 hits, Kinetic 125% vs blocked | Recyclable(5), ImproveCost(8), MultiUpgrade×4 |
| **Missile** | 1 | Attack | All | 8 全体伤害 | Recyclable(5), ImproveCost(8), MultiUpgrade×4 |
| **ShieldI** | 1 | Skill | Self | 3 Block + channel 1 ShieldGen + 1 TempFocus | Retain, Recyclable(5), ImproveCost(8), MultiUpgrade×4, Association |

### 状态牌

| 卡牌 | 费用 | 类型 | 效果 |
|------|------|------|------|
| **TinyResource** | 0 | Skill(Status) | Exhaust, Recyclable(2), 获得 5 金币 (Mining 产生) |

---

## 充能球 (3)

| 充能球 | 被动 | 激发 | 颜色 |
|--------|------|------|------|
| **BasicStrikeCraftOrb** | 回合结束对 ALL 敌人造成 {Passive} dmg | ALL 敌人 +1 Vulnerable + {Evoke} dmg | (0.2,0.2,0.2) |
| **ShieldGeneratorNode** | 回合结束获得 {Passive} Block | 获得 {Evoke} Block | (0.5,0.7,0.9) |
| **OrbitalBombardmentUnit** | 本单元激发伤害 +{Passive} | ALL 敌人 {Evoke} dmg + 另一个 OBU 获得等量加成 | — |

---

## 遗物 (2)

| 遗物 | 稀有度 | 效果 | 注册 |
|------|--------|------|------|
| **GrayGooCore** (纳米机器个体) | Starter | 每场战斗回合 1: +1 MaxHP + channel 1 StrikeCraft | `[RegisterCharacterStarterRelic]`, GrayTempestRelicPool, SharedRelicPool |
| **ShieldRelic** (护盾发生器) | Starter | 战斗回合 1: +1 Focus + channel {Counter} 个 ShieldGen | `[RegisterRelic(GrayTempestAssociationRelicPool)]`, 关联遗物 |

---

## 能力 (4)

| 能力 | 类型 | 效果 |
|------|------|------|
| **EfficientReconstructPower** | Buff, Counter | 使用重组卡时获得 Amount MaxHP |
| **TemporaryAssemblyPower** | Buff, Counter | 战斗结束添加随机 OneShot 牌入牌组 |
| **SustainedBombardmentPower** | Buff, Counter | 每回合开始 channel 1 OBU |
| **ArmageddonPower** | Buff, Counter | OBU 造成 {Amount}% 额外伤害 |
| **DoomsdayPower** (GrayTempestStorm) | Buff, Counter | — |

---

## 关键词注册 (12)

| 关键词 ID | 类型 | 说明 |
|-----------|------|------|
| `Reconstruct` | 玩法 | 消耗 MaxHP, 从纳米军械库选牌 |
| `Recyclable` | 修饰 | 回收时提供 MaxHP |
| `Recycle` | 玩法 | 消耗+移出牌组+获得 MaxHP |
| `OneShot` | 玩法 | 打出后消耗+移出牌组 (必须与 Exhaust 绑定) |
| `ImproveAction` | 玩法 | 花金币永久升级牌 |
| `ImproveCost` | 数值 | 改进花费的金币数 |
| `MultiUpgrade` | 修饰 | 此牌可被多次改进 |
| `Nanite` | 修饰 | 叠加在敌人上的计数 |
| `Energy` | 伤害 | 对无格挡敌人 +25% dmg |
| `Kinetic` | 伤害 | 对有格挡敌人 +25% dmg |
| `Burst` | 伤害 | 对 ALL 敌人造成伤害 |
| `Association` | 关联 | 进牌组获得关联遗物, 升级强化遗物 |

---

## 系统

| 系统 | 函数 | 行为 |
|------|------|------|
| **RecycleSystem** | `RecycleCard(ctx, card, owner)` | +2 MaxHP → 移除 |
| **ReconstructCmd** | `Execute(ctx, srcCard)` | -5 MaxHP → 选 1 (全部可用牌, 不限 3 张) → 入手+入牌组 |
| **ReconstructCardPool** | `GetPoolFactories(5)` | 返回注册的池卡牌工厂: Laser, KineticCannon, Missile, ShieldI |
| **ImproveSystem** | `ImproveCard(ctx, card, owner)` | 扣 ImproveCost 金币 → IMultiUpgradable:FakeUpgrade() / else CardCmd.Upgrade() → 手牌+牌组 |
| **AssociationSystem** | `CheckCardAdded/CheckCardUpgraded` | ShieldI 首次入牌组 → `RelicCmd.Obtain<ShieldRelic>`; 升级 ShieldI → 递增 relic.Counter |

---

## 本地化

| 语种 | 角色 | 路径 |
|------|------|------|
| **中文 (zhs)** | 主语言 | `GrayTempest/localization/zhs/` |
| **英文 (eng)** | 回退语言 | `GrayTempest/localization/eng/` |

### 表 (5 个 LocTableBridge)

| 表 | 文件 |
|----|------|
| cards | `cards.json` |
| card_keywords | `card_keywords.json` |
| characters | `characters.json` |
| relics | `relics.json` |
| powers | `powers.json` |
| orbs | `orbs.json` |
| static_hover_tips | `static_hover_tips.json` |

### 本地化注意事项
- 卡牌升级文本通过 `IntVar` + `{VarName}` 在 `description` 中实现，**不**使用 `description_upgrade`
- 遗物 `ShieldRelic` 描述中的 `{Counter}` 通过 `CanonicalVars` 注册为 `DynamicVar`，描述显示初值 1，实际计数通过 `DisplayAmount` + `ShowCounter` 在遗物图标的角标显示
- 所有 `static_hover_tips.json` 条目为简单占位符 `"{0}"`

---

## 已知问题

| 问题 | 严重度 | 说明 |
|------|--------|------|
| **debug_audio mp3 加载失败** | 非致命 | `NDebugAudioManager` 自动构造 `debug_audio/gray_tempest_orb_*_channel.mp3` 路径，即使 SFX 字符串为空也触发 "No loader found" |
| **SpineSkeleton: Node2D** | 非致命 | 充能球场景保留为 `Node2D`，期望 `SpineSprite` 但无实际影响 |
| **UID 重复** | 警告 | `field_research.png` 和 `field_improve.png` UID 重复 |
| **Missing .sln** | 导出警告 | 无 `.sln` 文件，Godot 导出时打印大量警告 (不影响功能) |

---

## 构建状态

```
Build: 0 errors, >0 warnings (pre-existing: null-ref warnings, .sln missing, UID duplicate, GDExtension spine)
```

---

## 全局 API 速查

```
DamageCmd.Attack(d).FromCard(card,play).Targeting(creature).TargetingAllOpponents().WithHitFx(s).WithHitCount(n).Execute(ctx)
CardPileCmd.Draw(ctx,amount,owner) / Add(cards,pile,pos,src) / AddGeneratedCardToCombat(card,pile,owner)
CardSelectCmd.FromHand(ctx,owner,prefs,filter) / FromCombatPile(ctx,pile,owner,prefs) / FromSimpleGrid(ctx,cards,owner,prefs)
CardCmd.Exhaust(ctx,card) / Upgrade(card,style) / Discard(card) / DiscardAndDraw(ctx,cards,count)
CreatureCmd.GainBlock(c,amt,play) / GainMaxHp(c,amt) / LoseMaxHp(ctx,c,amt) / Damage(ctx,targets,val,prop,src)
OrbCmd.Channel<T>(ctx,owner) / Passive(ctx,orb,target,fromEvoke)
PowerCmd.Apply<T>(ctx,target,amount,src,cardPlay)
PlayerCmd.LoseGold(amount,owner,lossType)
VfxCmd.PlayOnCreature(creature,vfxPath)
```

## 注册模式

```csharp
[RegisterCard(typeof(GrayTempestCardPool))]
[RegisterCharacterStarterCard(typeof(GrayTempestCharacter), count)]
[RegisterRelic(typeof(GrayTempestRelicPool))]
[RegisterCharacterStarterRelic(typeof(GrayTempestCharacter))]
[RegisterOrb]
[RegisterPower]
[RegisterOwnedCardKeyword(nameof(X))]
[RegisterSharedCardPool]    // Reconstruct 军械库
[RegisterSharedRelicPool]   // Shared/Association 遗物池
[RegisterRelic(typeof(...))] // 指定遗物所属池
```
