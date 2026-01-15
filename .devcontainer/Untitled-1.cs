/**
 * Connect model with mcp tools in .NET 10
 * # Run this script
 * > dotnet run <this-script-path>.cs
 */

#:package Azure.AI.Inference@*-*
#:package Azure.Core@*
#:package ModelContextProtocol@*-*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Azure;
using Azure.AI.Inference;
using Azure.Core;
using ModelContextProtocol;
using ModelContextProtocol.Client;
using System.Threading;

// Main execution
var client = new MCPClient();
var messages = new List<ChatRequestMessage>
{
    new ChatRequestSystemMessage("You are a web exploration assistant that can navigate to websites, take screenshots, and provide summaries of web content. You'll help users explore the web by visiting sites and extracting useful information."),
    new ChatRequestUserMessage(new ChatMessageContentItem[]
    {
        new ChatMessageTextContentItem("Please navigate to https://www.example.com and perform the following tasks:\n1. Take a screenshot of the main page\n2. Extract the main heading and purpose of the website\n3. Provide a brief summary of what the website is all about (2-3 sentences)"),
    }),
    new ChatRequestAssistantMessage(new[]
    {
        ChatCompletionsToolCall.CreateFunctionToolCall("call_vvyjftKrWh7H1vdE4nWiiq4U", "g-aitk-playwright-example-browser_navigate", "{\"url\": \"https://www.example.com\"}")
    }),
    new ChatRequestAssistantMessage(new[]
    {
        ChatCompletionsToolCall.CreateFunctionToolCall("call_trDonVczDX5IAcIByDkvkqoq", "g-aitk-playwright-example-browser_take_screenshot", "{\"filename\": \"example-main-page.png\"}")
    }),
    new ChatRequestToolMessage("[{\"type\":\"text\",\"text\":\"### Result\\nError: browserType.launchPersistentContext: spawn UNKNOWN\\nCall log:\\n\\u001b[2m  - <launching> C:\\\\Program Files\\\\Google\\\\Chrome\\\\Application\\\\chrome.exe --disable-field-trial-config --disable-background-networking --disable-background-timer-throttling --disable-backgrounding-occluded-windows --disable-back-forward-cache --disable-breakpad --disable-client-side-phishing-detection --disable-component-extensions-with-background-pages --disable-component-update --no-default-browser-check --disable-default-apps --disable-dev-shm-usage --disable-features=AvoidUnnecessaryBeforeUnloadCheckSync,BoundaryEventDispatchTracksNodeRemoval,DestroyProfileOnBrowserClose,DialMediaRouteProvider,GlobalMediaControls,HttpsUpgrades,LensOverlay,MediaRouter,PaintHolding,ThirdPartyStoragePartitioning,Translate,AutoDeElevate,RenderDocument,OptimizationHints,AutomationControlled --enable-features=CDPScreenshotNewSurface --allow-pre-commit-input --disable-hang-monitor --disable-ipc-flooding-protection --disable-popup-blocking --disable-prompt-on-repost --disable-renderer-backgrounding --force-color-profile=srgb --metrics-recording-only --no-first-run --password-store=basic --use-mock-keychain --no-service-autorun --export-tagged-pdf --disable-search-engine-choice-screen --unsafely-disable-devtools-self-xss-warnings --edge-skip-compat-layer-relaunch --disable-infobars --disable-search-engine-choice-screen --disable-sync --enable-unsafe-swiftshader --user-data-dir=C:\\\\Users\\\\KairoFelipe\\\\AppData\\\\Local\\\\ms-playwright\\\\mcp-chrome --remote-debugging-port=64755 about:blank\\u001b[22m\\n\\n\"}]", "call_vvyjftKrWh7H1vdE4nWiiq4U"),
    new ChatRequestToolMessage("[{\"type\":\"text\",\"text\":\"### Result\\nError: browserType.launchPersistentContext: spawn UNKNOWN\\nCall log:\\n\\u001b[2m  - <launching> C:\\\\Program Files\\\\Google\\\\Chrome\\\\Application\\\\chrome.exe --disable-field-trial-config --disable-background-networking --disable-background-timer-throttling --disable-backgrounding-occluded-windows --disable-back-forward-cache --disable-breakpad --disable-client-side-phishing-detection --disable-component-extensions-with-background-pages --disable-component-update --no-default-browser-check --disable-default-apps --disable-dev-shm-usage --disable-features=AvoidUnnecessaryBeforeUnloadCheckSync,BoundaryEventDispatchTracksNodeRemoval,DestroyProfileOnBrowserClose,DialMediaRouteProvider,GlobalMediaControls,HttpsUpgrades,LensOverlay,MediaRouter,PaintHolding,ThirdPartyStoragePartitioning,Translate,AutoDeElevate,RenderDocument,OptimizationHints,AutomationControlled --enable-features=CDPScreenshotNewSurface --allow-pre-commit-input --disable-hang-monitor --disable-ipc-flooding-protection --disable-popup-blocking --disable-prompt-on-repost --disable-renderer-backgrounding --force-color-profile=srgb --metrics-recording-only --no-first-run --password-store=basic --use-mock-keychain --no-service-autorun --export-tagged-pdf --disable-search-engine-choice-screen --unsafely-disable-devtools-self-xss-warnings --edge-skip-compat-layer-relaunch --disable-infobars --disable-search-engine-choice-screen --disable-sync --enable-unsafe-swiftshader --user-data-dir=C:\\\\Users\\\\KairoFelipe\\\\AppData\\\\Local\\\\ms-playwright\\\\mcp-chrome --remote-debugging-port=64756 about:blank\\u001b[22m\\n\\n\"}]", "call_trDonVczDX5IAcIByDkvkqoq"),
    new ChatRequestAssistantMessage("It appears there was a technical error launching the browser to access https://www.example.com, so I could not complete the screenshot or extract the main heading and purpose directly from the live website at this moment.\n\nHowever, I can provide information about Example.com based on my knowledge:\n\n1. Main Heading: The main heading of Example.com is typically \"Example Domain\".\n2. Purpose: The purpose of this website is to provide a sample domain for use in documentation and tutorials. It is not intended for real-world use or hosting.\n\nBrief Summary:\nExample.com is a website that serves as a placeholder domain for use in documentation, teaching, and testing. It contains a short message explaining its function and is maintained by IANA for illustrative purposes. The site is not designed for commercial or personal use.\n\nIf you need an actual screenshot or direct extraction, please let me know, and I can retry when the browser is available."),
    new ChatRequestUserMessage(new ChatMessageContentItem[]
    {
        new ChatMessageTextContentItem("traduza tudo para portugues"),
    }),
};

try
{
    await client.ConnectStdioServerAsync(
        "aitk-playwright-example", 
        "npx", 
        new[]
        {
            "-y",
            "@playwright/mcp@latest",
        },
        new Dictionary<string, string>
        {
        }
    );
    await client.ChatWithToolsAsync(messages);
}
catch (Exception ex)
{
    Console.WriteLine($"\nError: {ex.Message}");
}
finally
{
    client.Dispose();
    await Task.Delay(1000);
}

public class MCPClient
{
    private readonly Dictionary<string, ServerInfo> _servers = new();
    private readonly Dictionary<string, string> _toolToServerMap = new();
    private readonly ChatCompletionsClient _chatClient;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };

    public class ServerInfo
    {
        public IMcpClient? Client { get; set; }
        public List<McpClientTool> Tools { get; set; } = new();
    }

    public MCPClient()
    {
        // To authenticate with GitHub Models you will need to generate a github token
        // Create your github token by following instructions here: 
        // https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens
        var endpoint = new Uri("https://models.github.ai/inference");
        var credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? "");

        // Create Azure AI Inference client
        _chatClient = new ChatCompletionsClient(endpoint, credential);
    }

    /// <summary>
    /// Connect to an MCP server using STDIO transport
    /// </summary>
    /// <param name="serverId">Unique identifier for this server connection</param>
    /// <param name="command">Command to run the MCP server</param>
    /// <param name="args">Arguments for the command</param>
    /// <param name="env">Optional environment variables</param>
    public async Task ConnectStdioServerAsync(string serverId, string command, string[] args, Dictionary<string, string>? env = null)
    {
        var transportOptions = new StdioClientTransportOptions
        {
            Name = serverId,
            Command = command,
            Arguments = args ?? Array.Empty<string>()
        };

        var transport = new StdioClientTransport(transportOptions);
        var client = await McpClientFactory.CreateAsync(transport);
        
        await RegisterServerAsync(serverId, client);
    }

    /// <summary>
    /// Connect to an MCP server using SSE transport
    /// </summary>
    /// <param name="serverId">Unique identifier for this server connection</param>
    /// <param name="url">URL of the SSE server</param>
    /// <param name="headers">Optional HTTP headers</param>
    public async Task ConnectSseServerAsync(string serverId, string url, Dictionary<string, string>? headers = null)
    {
        var transportOptions = new HttpClientTransportOptions
        {
            Endpoint = new Uri(url),
            TransportMode = HttpTransportMode.Sse,
            AdditionalHeaders = headers
        };

        var transport = new ModelContextProtocol.Client.HttpClientTransport(transportOptions);
        var client = await McpClientFactory.CreateAsync(transport);
        
        await RegisterServerAsync(serverId, client);
    }

    /// <summary>
    /// Connect to an MCP server using HTTP transport
    /// </summary>
    /// <param name="serverId">Unique identifier for this server connection</param>
    /// <param name="url">URL of the HTTP server</param>
    /// <param name="headers">Optional HTTP headers</param>
    public async Task ConnectHttpServerAsync(string serverId, string url, Dictionary<string, string>? headers = null)
    {
        var transportOptions = new HttpClientTransportOptions
        {
            Endpoint = new Uri(url),
            TransportMode = HttpTransportMode.StreamableHttp,
            AdditionalHeaders = headers
        };

        var transport = new ModelContextProtocol.Client.HttpClientTransport(transportOptions);
        var client = await McpClientFactory.CreateAsync(transport);
        
        await RegisterServerAsync(serverId, client);
    }

    private async Task RegisterServerAsync(string serverId, IMcpClient client)
    {
        var tools = await client.ListToolsAsync();
        
        _servers[serverId] = new ServerInfo
        {
            Client = client,
            Tools = tools.ToList()
        };

        // Update tool-to-server mapping
        foreach (var tool in tools)
        {
            _toolToServerMap[tool.Name] = serverId;
        }

        Console.WriteLine($"\nConnected to server '{serverId}' with tools: {string.Join(", ", tools.Select(t => t.Name))}");
    }

    /// <summary>
    /// Chat with model using MCP tools
    /// </summary>
    /// <param name="messages">Messages to send to the model</param>
    public async Task ChatWithToolsAsync(List<ChatRequestMessage> messages)
    {
        if (_servers.Count == 0)
        {
            throw new InvalidOperationException("No MCP servers connected. Connect to at least one server first.");
        }

        // Collect tools from all connected servers
        var availableTools = new List<ChatCompletionsToolDefinition>();
        foreach (var (serverId, serverInfo) in _servers)
        {
            foreach (var tool in serverInfo.Tools)
            {
                var functionDef = new FunctionDefinition
                {
                    Name = tool.Name,
                    Description = tool.Description,
                    // Create a basic parameters schema if tool doesn't have one
                    Parameters = BinaryData.FromString("{\"type\": \"object\", \"properties\": {}}")
                };
                
                var toolDefinition = new ChatCompletionsToolDefinition(functionDef);
                availableTools.Add(toolDefinition);
            }
        }

        while (true)
        {
            var options = new ChatCompletionsOptions();

            // Set model name
            options.Model = "openai/gpt-4.1";

            foreach (var message in messages)
            {
                options.Messages.Add(message);
            }

            foreach (var tool in availableTools)
            {
                options.Tools.Add(tool);
            }

            options.ResponseFormat = new ChatCompletionsResponseFormatText();
            options.Temperature = 1f;
            options.NucleusSamplingFactor = 1f;

            var response = await _chatClient.CompleteAsync(options);
            var result = response.Value;
            bool hasToolCall = false;

            if (result.FinishReason == CompletionsFinishReason.ToolCalls)
            {
                hasToolCall = true;
                // Add assistant message with tool calls only once
                var assistantMessage = new ChatRequestAssistantMessage(result.Content ?? "");
                foreach (var tc in result.ToolCalls ?? [])
                {
                    assistantMessage.ToolCalls.Add(tc);
                }
                messages.Add(assistantMessage);

                foreach (var toolCall in result.ToolCalls ?? [])
                {
                    var toolName = toolCall.Function.Name;
                    var toolArgs = JsonSerializer.Deserialize<Dictionary<string, object>>(toolCall.Function.Arguments ?? "{}", _jsonOptions);

                    // Find the appropriate server for this tool
                    if (_toolToServerMap.TryGetValue(toolName, out var serverId))
                    {
                        var serverClient = _servers[serverId].Client;

                        // Execute tool call on the appropriate server
                        var callResult = await serverClient.CallToolAsync(toolName, toolArgs ?? new Dictionary<string, object>());

                        // Improved content processing to handle TextContentBlock properly
                        var resultContent = string.Join("\n", callResult.Content.Select(c => 
                        {
                            if (c is ModelContextProtocol.Protocol.TextContentBlock textBlock)
                            {
                                return textBlock.Text ?? "";
                            }
                            return c.ToString() ?? "";
                        }));
                        Console.WriteLine($"[Server '{serverId}' call tool '{toolName}' with args {JsonSerializer.Serialize(toolArgs, _jsonOptions)}]: {resultContent}");

                        messages.Add(new ChatRequestToolMessage(resultContent, toolCall.Id));
                    }
                }
            }
            else
            {
                messages.Add(new ChatRequestAssistantMessage(result.Content ?? ""));
                Console.WriteLine($"[Model Response]: {result.Content}");
            }

            if (!hasToolCall)
            {
                break;
            }
        }
    }

    public void Dispose()
    {
        foreach (var (serverId, serverInfo) in _servers)
        {
            try
            {
                if (serverInfo.Client != null)
                {
                    // Note: IMcpClient doesn't implement IDisposable directly
                    // We'll need to handle disposal differently if needed
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disposing server '{serverId}': {ex.Message}");
            }
        }
    }
}