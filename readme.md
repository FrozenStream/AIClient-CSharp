# AI Client Library for .NET / AI客户端库.NET版

一个用于与AI大语言模型API交互的.NET库，支持函数调用（工具调用）功能。
An AI client library for .NET that interacts with large language model APIs and supports function calling (tool calling).

## 功能特点 / Features

- 与OpenAI兼容的API接口 / OpenAI-compatible API interface
- 支持函数调用（Tools/Functions Calling） / Supports function calling (Tools/Functions Calling)
- 简单易用的聊天历史管理 / Easy-to-use chat history management
- 支持多种工具定义和执行 / Supports multiple tool definitions and executions
- 异步API调用 / Asynchronous API calls

## 项目结构 / Project Structure

```
.
├── AIClient.cs              # 核心AI客户端类 / Core AI client class
├── ChatResult.cs            # API响应结果数据结构 / API response data structure
├── History.cs               # 聊天历史管理类 / Chat history management class
├── MainExample.cs           # 使用示例 / Usage example
├── TestAIClient.csproj      # 项目配置文件 / Project configuration file
└── Tools/                   # 工具实现目录 / Tools implementation directory
    ├── tools.cs             # 工具基础类和接口 / Tool base classes and interfaces
    ├── getTime.cs           # 获取时间工具示例 / Get time tool example
    ├── getCurrentWeather.cs # 获取天气工具示例 / Get weather tool example
    └── RecommendOutfitTool.cs # 推荐着装工具示例 / Recommend outfit tool example
```

## 核心组件 / Core Components

### AIClient 类 / AIClient Class

主要的AI客户端类，负责与AI服务进行通信。
The main AI client class responsible for communicating with AI services.

关键方法：/ Key Methods:
- `CompleteChat(string history)`: 发送聊天历史并获取AI回复 / Send chat history and get AI response

构造参数：/ Constructor Parameters:
- `model`: 模型名称 / Model name
- `apiKey`: API密钥 / API key
- `apiUrl`: API端点URL / API endpoint URL
- `tools`: 可选的工具定义JSON字符串 / Optional tool definition JSON string

### HistoryList 类 / HistoryList Class

用于构建和管理聊天历史记录。
Used to build and manage chat history records.

主要方法：/ Main Methods:
- `AddSystem(string content)`: 添加系统消息 / Add system message
- `AddUser(string content)`: 添加用户消息 / Add user message
- `AddAssistant(string content)`: 添加助手消息 / Add assistant message
- `AddToolResult(string tool_call_id, string tool_call_name, string content)`: 添加工具执行结果 / Add tool execution result
- `ToJsonString()`: 将历史记录序列化为JSON字符串 / Serialize history records to JSON string

### Tools 工具系统 / Tools System

提供了一套完整的工具定义和执行框架。
Provides a complete framework for tool definition and execution.

核心组件：/ Core Components:
- `IFunction`: 工具接口 / Tool interface
- `ToolsList`: 工具列表管理器 / Tool list manager
- `Parameter`: 工具参数定义 / Tool parameter definition

预定义工具示例：/ Predefined Tool Examples:
1. `getTimeTool`: 获取当前时间 / Get current time
2. `getCurrentWeatherTool`: 获取指定城市的天气 / Get weather for a specified city
3. `RecommendOutfit`: 根据天气、温度和场合推荐着装 / Recommend outfit based on weather, temperature, and occasion

## 使用示例 / Usage Example

```csharp
// 创建聊天历史 / Create chat history
var historyList = new HistoryList();
historyList.AddUser("请告诉我现在的时间和北京的天气");

// 定义工具 / Define tools
var toolsList = new ToolsList();
toolsList.Add(new getTimeTool());
toolsList.Add(new getCurrentWeatherTool());

// 创建AI客户端 / Create AI client
var client = new AIClient(
    "qwen-plus",                                    // 模型名 / Model name
    "your-api-key",                                 // API密钥 / API key
    "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions", // API地址 / API URL
    toolsList.serialize()                           // 工具定义 / Tool definitions
);

// 发起请求 / Make request
ChatResult result = await client.CompleteChat(historyList.ToJsonString());

// 处理可能的工具调用 / Handle possible tool calls
if (result.choices[0].message.tool_calls?.Length > 0)
{
    var toolResults = toolsList.CallTools(result.choices[0].message);
    // 处理工具调用结果... / Process tool call results...
}
```

## 依赖项 / Dependencies

- .NET 8.0
- System.Text.Json (7.0.0)

## 安装和运行 / Installation and Running

1. 确保已安装 .NET 8.0 SDK / Ensure .NET 8.0 SDK is installed
2. 克隆或下载此项目 / Clone or download this project
3. 在项目根目录运行以下命令：/ Run the following commands in the project root directory:

```bash
dotnet restore
dotnet build
dotnet run
```

## 自定义工具开发 / Custom Tool Development

要创建自定义工具，需要：/ To create a custom tool, you need:

1. 实现 [IFunction](file:///D:/SteamLibrary/steamapps/common/Chill%20with%20You%20Lo-Fi%20Story/test/Tools/tools.cs#L22-L42) 接口 / Implement the IFunction interface
2. 定义工具参数 / Define tool parameters
3. 实现 Call 方法处理工具逻辑 / Implement the Call method to handle tool logic

示例：/ Example:
```csharp
class MyCustomTool : IFunction
{
    public Dictionary<string, Parameter> parameters { get; set; }
    public string description { get; }
    public bool? strict { get; }
    public string functionJson { get; }

    public MyCustomTool()
    {
        parameters = new Dictionary<string, Parameter>();
        description = "工具描述";
        strict = true;
        functionJson = IFunction.BuildFunctionJson(this);
    }

    public string Call(string args)
    {
        // 工具逻辑实现
        return "工具执行结果";
    }
}
```

## 许可证 / License

本项目仅供学习和参考使用。
This project is for learning and reference purposes only.