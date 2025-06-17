# VeighNa - By Traders, For Traders, AI-Powered.

<p align="center">
  <img src ="https://vnpy.oss-cn-shanghai.aliyuncs.com/veighna-logo.png"/>
</p>

<p align="center">
    <img src ="https://img.shields.io/badge/version-4.0.0-blueviolet.svg"/>
    <img src ="https://img.shields.io/badge/platform-windows|linux|macos-yellow.svg"/>
    <img src ="https://img.shields.io/badge/python-3.10|3.11|3.12|3.13-blue.svg" />
    <img src ="https://img.shields.io/github/actions/workflow/status/vnpy/vnpy/pythonapp.yml?branch=master"/>
    <img src ="https://img.shields.io/github/license/vnpy/vnpy.svg?color=orange"/>
</p>


## Why vnpy Plugins?
VeighNa is a Python-based open source quantitative trading system development framework that has grown step by step into a fully-featured quantitative trading platform with continuous contributions from the open source community. It currently has many users from domestic and international financial institutions, including hedge funds, investment banks, futures brokers, university research institutions, proprietary trading companies, etc.

:rocket: :rocket: :rocket: **The VeighNa Elite Quantitative Terminal for professional traders has been officially released, providing comprehensive support for professional traders' needs in areas such as massive strategy concurrency, intelligent position rolling, algorithmic order execution, multi-account trading support, and more. For more detailed information, please scan the QR code below and follow the account, then click on the menu bar's [Community Exchange -> Elite Member Services]**:

<p align="center">
  <img src ="https://vnpy.oss-cn-shanghai.aliyuncs.com/alpha_demo.jpg"/, width=500>
</p>



They are:
- 🔌 Modular & reusable components
- 📦 Environment-agnostic (backtest, sandbox, live)
- 🧩 Easy to plug into any strategy or workflow
- 🌐 Exchange-agnostic with unified interfaces

Create, share, or combine plugins for indicators, strategies, risk controls, and more — all while keeping your code clean and scalable.

![Hydra500](https://github.com/user-attachments/assets/b4799778-948a-4d16-a628-25da31a9ebb4)


## AI-Powered

On the tenth anniversary of VeighNa's release, version 4.0 officially introduces the module targeting AI quantitative strategies, providing professional quantitative traders with **an all-in-one multi-factor machine learning (ML) strategy development, research, and live trading solution**:

<p align="center">
  <img src ="https://vnpy.oss-cn-shanghai.aliyuncs.com/alpha_demo.jpg"/, width=500>
</p>

## vnpy Workspace

While the vnpy Platform is all about an integration to dozens of different data vendors, the interface is either Python or a CLI.

If you want an enterprise UI to visualize this datasets and use AI agents on top, you can find vnpy Workspace at.

![image](https://gitee.com/pythonvnpy/vnpy/raw/master/frontend/public/vnpy-002.png)

* :bar_chart: **dataset**: Factor Feature Engineering

    * Designed specifically for ML algorithm training optimization, supporting efficient batch feature calculation and processing
    * Built-in rich factor feature expression calculation engine, enabling rapid one-click generation of training data
    * Alpha 158: A collection of stock market features from Microsoft's Qlib project, covering multiple dimensions of quantitative factors including K-line patterns, price trends, and time-series volatility

* :bulb: **model**: Prediction Model Training

    * Provides standardized ML model development templates, greatly simplifying model building and training processes
    * Unified API interface design, supporting seamless switching between different algorithms for performance comparison testing
    * Integrates multiple mainstream machine learning algorithms:
        * Lasso]: Classic Lasso regression model, implementing feature selection through L1 regularization
        * LightGBM: Efficient gradient boosting decision tree with a training engine optimized for large-scale datasets
        * MLP: Multi-layer perceptron neural network, suitable for modeling complex non-linear relationships



```python
import vnpy
"""
This example shows how backtest over tweets
"""

class TwitterBot(vnpy.Model):
    def main(self, args):
        while self.has_data:
            self.backtester.value_account()
            self.sleep('1h')

    def event(self, type_: str, data: str):
        # Now check if it's a tweet about Tesla
        if 'tsla' in data.lower() or 'gme' in data.lower():
            # Buy, sell or evaluate your portfolio
            pass


if __name__ == "__main__":
    exchange = vnpy.Alpaca()
    model = TwitterBot(exchange)

    # Add the tweets json here
    model.backtester.add_custom_events(vnpy.data.JsonEventReader('./tweets.json'))
    # Now add some underlying prices at 1 month
    model.backtester.add_prices('TSLA', '1h', start_date='3/20/22', stop_date='4/15/22')

    # Backtest or run live
    print(model.backtest(args=None, initial_values={'USD': 10000}))

```

**Shell** - the ready-made graphical framework with the ability to quickly change to your needs and with fully open source code in C#:
  - Complete source code
  - Support for all vnpy platform connections
  - Support for S#.Designer schemas
  - Flexible user interface
  - Strategy testing (statistics, equity, reports)
  - Save and load strategy settings
  - Launch strategies in parallel
  - Detailed information on strategy performance 
  - Launch strategies on schedule

## 🛠️ Installation

In addition to the graphical start-up method based on VeighNa Station, you can also create run.py in any directory and write the following sample code:

```Python
from vnpy.event import EventEngine
from vnpy.trader.engine import MainEngine
from vnpy.trader.ui import MainWindow, create_qapp

from vnpy_ctp import CtpGateway
from vnpy_ctastrategy import CtaStrategyApp
from vnpy_ctabacktester import CtaBacktesterApp


def main():
    """Start VeighNa Trader"""
    qapp = create_qapp()

    event_engine = EventEngine()
    main_engine = MainEngine(event_engine)
    
    main_engine.add_gateway(CtpGateway)
    main_engine.add_app(CtaStrategyApp)
    main_engine.add_app(CtaBacktesterApp)

    main_window = MainWindow(main_engine, event_engine)
    main_window.showMaximized()

    qapp.exec()


if __name__ == "__main__":
    main()
```

```mermaid
erDiagram
    PLUGIN {
        string id
        string name
        string type
    }

    PLUGIN ||--o{ STRATEGY : implements
    STRATEGY ||--o{ INDICATOR : uses
    STRATEGY ||--o{ EXECUTOR : runs
    EXECUTOR ||--o{ MARKET_INTERFACE : interacts
    MARKET_INTERFACE }|--o{ EXCHANGE : connects
    PLUGIN ||--o{ CONFIGURATION : requires
    PLUGIN ||--o{ LOGGING : logs
```



<p align="center">
    <img src="https://minkxx-spotify-readme.vercel.app/api?theme=dark&rainbow=true&scan=true&spin=True" alt="Preview">
</p>

<p align="center">
  <img src="https://github.com/tarikmanoar/tarikmanoar/raw/output/github-snake-dark.svg" alt="snake"></center>
</p>

## Contributors

 wouldn't be  without you. If we are going to disrupt financial industry, every contribution counts. Thank you for being part of this journey.

<a href="https://github.com/vnpy-finance/vnpy/graphs/contributors">
   <img src="https://contributors-img.web.app/image?repo=vnpy-finance/vnpy" width="800"/>
</a>
