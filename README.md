# BuckshotRoulette.Simplified



[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Version](https://img.shields.io/badge/Version-0.1.0--beta-blue)](https://github.com/Hoshi-Inori/BuckshotRoulette/releases)

一个基于 .NET 9 开发的《恶魔轮盘》命令行简化版。本项目采用状态机架构实现，支持多语言动态切换，并针对控制台 UI 对齐进行了深度优化。

---

## 游戏特性

* **核心玩法高度还原**：包含实弹/虚弹机制、完整的道具系统（放大镜、手锯、香烟等）。
* **状态机架构**：基于 State Pattern 驱动游戏逻辑，确保状态切换清晰、稳定。
* **多语言支持**：内置中文、英文、日文三语配置文件，支持根据本地环境动态加载。
* **控制台优化**：自适应全角/半角字符显示宽度，解决 CJK 环境下的 UI 错位痛点。
* **高性能分发**：支持 win-x64 / linux-x64 单文件发布，无需安装运行时即可运行。

---

## 项目结构

本工程共包含 48 个类/接口/枚举，核心架构如下：

* **Contexts**: 数据上下文，管理游戏实时状态与配置。
* **States**: 核心逻辑状态机（Splash / Game / Config 三大状态组）。
* **Items**: 策略模式实现的道具系统，易于扩展新道具。
* **Renderers**: 独立的渲染引擎，负责控制台界面的缓冲与输出。
* **Utilities**: 包含随机化工具、显示宽度校准等辅助功能。

---

## 快速开始

### 运行环境
* Windows 10+ / Linux
* 终端建议使用：**Cascadia Code**, **新宋体**, 或 **Lucida Console** 以获得最佳对齐效果。

### 构建与发布
使用以下命令生成单文件版本（包含可执行文件、locales 本地化文件夹以及运行时生成的配置文件）：

```bash
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -p:DebugType=none -p:IncludeSourceRevisionInInformationalVersion=false
```