using System.Text.Json;
using AIExtension;
using AIExtension.Tools;

class Program
{
    static async Task work()
    {
        Console.WriteLine("Testing AIClient...");

        // 创建带有工具定义的历史记录
        var historyList = new HistoryList();
        historyList.AddUser("Hello world! test every tool");

        Console.WriteLine(historyList.ToJsonString());

        // 定义工具
        var toolsList = new ToolsList();
        toolsList.Add(new getTimeTool());
        toolsList.Add(new getCurrentWeatherTool());
        toolsList.Add(new RecommendOutfit());

        // 创建支持工具调用的客户端
        var client = new AIClient(
            "qwen-plus",
            "sk-6be272f0721e44198bbf99e38e8b8386",
            "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions",
            toolsList.serialize()
        );

        Console.WriteLine("Calling CompleteChat method with tools...");
        ChatResult result = await client.CompleteChat(historyList.ToJsonString());

        Console.WriteLine($"{JsonSerializer.Serialize(result)}");

        Console.WriteLine(result.choices[0].message.content ?? "No content found");

        if (result.choices[0].message.tool_calls?.Length > 0)
        {
            Console.WriteLine("\n检测到工具调用:");
            var toolResults = toolsList.CallTools(result.choices[0].message);
            foreach (var toolResult in toolResults)
            {
                Console.WriteLine($"工具 {toolResult.Key} 返回: {toolResult.Value}");
            }
        }
        else
        {
            Console.WriteLine("\n未检测到工具调用");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }


    static void Main()
    {
        work().Wait();
        return;
    }
}