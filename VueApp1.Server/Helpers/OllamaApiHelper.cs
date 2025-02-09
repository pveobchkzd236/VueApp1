using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VueApp1.Server.Helpers
{
    /// <summary>
    /// Ollama API 辅助类，用于封装 /api/generate /api/chat /api/embed /api/tags 等常用接口。
    /// </summary>
    public static class OllamaApiHelper
    {
        #region 基础配置 (静态字段)

        /// <summary>
        /// Ollama 服务的基础 URL（示例）。请根据实际情况修改。
        /// </summary>
        private static readonly string API_BASE_URL = "http://127.0.0.1:11434";

        /// <summary>
        /// 如果需要鉴权，可以在这里存储你的 API Key。若无需鉴权可留空。
        /// </summary>
        private static readonly string API_KEY = "YOUR_API_KEY";

        #endregion

        #region 1. Generate a completion (/api/generate)

        /// <summary>
        /// 请求体示例：生成文本
        /// （适用于一次性返回：stream=false）
        /// </summary>
        public class GenerateRequest
        {
            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("prompt")]
            public string Prompt { get; set; }

            // 可选
            [JsonProperty("suffix", NullValueHandling = NullValueHandling.Ignore)]
            public string Suffix { get; set; }

            // 一般在文档里还可指定 "options", "format", "stream", "raw", "keep_alive" 等
            // 这里仅示例化：设置一次性返回
            [JsonProperty("stream")]
            public bool Stream { get; set; } = false;
        }

        /// <summary>
        /// 响应体示例（当 stream=false 时，Ollama 会一次性返回最终响应）
        /// </summary>
        public class GenerateResponse
        {
            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }

            /// <summary>
            /// 当 stream=false 时，这里会是整段完整回复
            /// </summary>
            [JsonProperty("response")]
            public string Response { get; set; }

            [JsonProperty("done")]
            public bool Done { get; set; }

            [JsonProperty("done_reason")]
            public string DoneReason { get; set; }

            // 下面是一些统计或上下文信息，可按需取用
            [JsonProperty("context")]
            public List<int> Context { get; set; }

            [JsonProperty("total_duration")]
            public long TotalDuration { get; set; }

            [JsonProperty("load_duration")]
            public long LoadDuration { get; set; }

            [JsonProperty("prompt_eval_count")]
            public int PromptEvalCount { get; set; }

            [JsonProperty("prompt_eval_duration")]
            public long PromptEvalDuration { get; set; }

            [JsonProperty("eval_count")]
            public int EvalCount { get; set; }

            [JsonProperty("eval_duration")]
            public long EvalDuration { get; set; }
        }

        /// <summary>
        /// 调用 /api/generate，生成文本（一次性返回，不做流式）
        /// </summary>
        /// <param name="request">要发送的请求体</param>
        /// <returns>生成的完整响应</returns>
        public static async Task<GenerateResponse> GenerateTextAsync(GenerateRequest request)
        {
            string url = $"{API_BASE_URL}/api/generate";

            using (HttpClient client = new HttpClient())
            {
                // 若需要鉴权，可在这里加请求头
                // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

                // 序列化请求体
                string json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    string err = await response.Content.ReadAsStringAsync();
                    throw new Exception($"调用 /api/generate 失败: {err}");
                }

                // 反序列化响应
                string resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GenerateResponse>(resultJson);
                return result;
            }
        }

        #endregion

        #region 2. Generate a chat completion (/api/chat)

        /// <summary>
        /// /api/chat 的请求体示例
        /// </summary>
        public class ChatMessage
        {
            [JsonProperty("role")]
            public string Role { get; set; } // user / assistant / system / tool

            [JsonProperty("content")]
            public string Content { get; set; }

            // 如果有多模态模型，还可以放 images 等
        }

        public class ChatRequest
        {
            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("messages")]
            public List<ChatMessage> Messages { get; set; }

            // 同理可以加 stream、options 等
            [JsonProperty("stream")]
            public bool Stream { get; set; } = false;
        }

        /// <summary>
        /// /api/chat 的一次性返回响应
        /// </summary>
        public class ChatResponse
        {
            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("message")]
            public ChatMessage Message { get; set; }

            [JsonProperty("done")]
            public bool Done { get; set; }

            [JsonProperty("done_reason")]
            public string DoneReason { get; set; }

            // 同样可选的一些统计字段
            [JsonProperty("total_duration")]
            public long TotalDuration { get; set; }

            [JsonProperty("load_duration")]
            public long LoadDuration { get; set; }

            [JsonProperty("prompt_eval_count")]
            public int PromptEvalCount { get; set; }

            [JsonProperty("prompt_eval_duration")]
            public long PromptEvalDuration { get; set; }

            [JsonProperty("eval_count")]
            public int EvalCount { get; set; }

            [JsonProperty("eval_duration")]
            public long EvalDuration { get; set; }
        }

        /// <summary>
        /// 调用 /api/chat，进行一次性返回的聊天调用
        /// </summary>
        /// <param name="request">Chat请求体</param>
        /// <returns>聊天生成的完整响应</returns>
        public static async Task<ChatResponse> ChatAsync(ChatRequest request)
        {
            string url = $"{API_BASE_URL}/api/chat";

            using (HttpClient client = new HttpClient())
            {
                // 若需要鉴权，在此添加
                // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

                string json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    string err = await response.Content.ReadAsStringAsync();
                    throw new Exception($"调用 /api/chat 失败: {err}");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ChatResponse>(resultJson);
                return result;
            }
        }

        // 这里是最重要的：流式读取 /api/chat
        public static async Task ChatStreamAsync(
            ChatRequest request,
            Action<ChatResponse> onPartial)
        {
            request.Stream = true;

            // 1) 自定义 HttpClientHandler
            var handler = new HttpClientHandler();
            // 关闭自动解压
            handler.AutomaticDecompression = DecompressionMethods.None;

            using (var client = new HttpClient(handler))
            {
                // 如果 SSE 需要:
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

                // 2) 序列化请求
                var requestJson = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                // 3) 发送请求
                using (var response = await client.PostAsync(API_BASE_URL + "/api/chat", content))
                {
                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var reader = new StreamReader(stream, Encoding.UTF8, false))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            // 可能服务端采用 SSE: line 形如 "data: {...}"
                            // 你要根据实际格式提取 JSON 部分
                            if (line.StartsWith("data:"))
                                line = line.Substring("data:".Length).Trim();

                            // 然后再反序列化
                            var partial = JsonConvert.DeserializeObject<ChatResponse>(line);
                            onPartial?.Invoke(partial);
                            if (partial != null && partial.Done) break;
                        }
                    }
                }
            }
        }


        #endregion

        #region 3. Generate Embeddings (/api/embed)

        /// <summary>
        /// /api/embed 的请求体
        /// </summary>
        public class EmbeddingsRequest
        {
            [JsonProperty("model")]
            public string Model { get; set; }

            /// <summary>
            /// 可以是单条字符串，也可以是字符串数组
            /// </summary>
            [JsonProperty("input")]
            public object Input { get; set; }

            // 是否截断过长输入等其它可选字段
            [JsonProperty("truncate", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Truncate { get; set; }

            // options 也可加在这里
        }

        /// <summary>
        /// /api/embed 的一次性返回响应
        /// </summary>
        public class EmbeddingsResponse
        {
            [JsonProperty("model")]
            public string Model { get; set; }

            /// <summary>
            /// 当 input 是多个文本时，这里会返回多维数组
            /// </summary>
            [JsonProperty("embeddings")]
            public List<List<float>> Embeddings { get; set; }

            [JsonProperty("total_duration")]
            public long TotalDuration { get; set; }

            [JsonProperty("load_duration")]
            public long LoadDuration { get; set; }

            [JsonProperty("prompt_eval_count")]
            public int PromptEvalCount { get; set; }
        }

        /// <summary>
        /// 调用 /api/embed，生成 Embeddings
        /// </summary>
        /// <param name="request">请求体</param>
        /// <returns>嵌入向量结果</returns>
        public static async Task<EmbeddingsResponse> EmbedAsync(EmbeddingsRequest request)
        {
            string url = $"{API_BASE_URL}/api/embed";

            using (HttpClient client = new HttpClient())
            {
                // 如需要鉴权
                // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

                string json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    string err = await response.Content.ReadAsStringAsync();
                    throw new Exception($"调用 /api/embed 失败: {err}");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<EmbeddingsResponse>(resultJson);
                return result;
            }
        }

        #endregion

        #region 4. List Local Models (/api/tags)

        /// <summary>
        /// /api/tags 响应示例
        /// </summary>
        public class ModelInfo
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("modified_at")]
            public DateTime ModifiedAt { get; set; }

            [JsonProperty("size")]
            public long Size { get; set; }

            [JsonProperty("digest")]
            public string Digest { get; set; }

            [JsonProperty("details")]
            public ModelDetails Details { get; set; }
        }

        public class ModelDetails
        {
            [JsonProperty("format")]
            public string Format { get; set; }

            [JsonProperty("family")]
            public string Family { get; set; }

            [JsonProperty("families")]
            public List<string> Families { get; set; }

            [JsonProperty("parameter_size")]
            public string ParameterSize { get; set; }

            [JsonProperty("quantization_level")]
            public string QuantizationLevel { get; set; }
        }

        public class TagsResponse
        {
            [JsonProperty("models")]
            public List<ModelInfo> Models { get; set; }
        }

        /// <summary>
        /// 获取本地已存在的模型列表
        /// </summary>
        public static async Task<List<ModelInfo>> ListLocalModelsAsync()
        {
            string url = $"{API_BASE_URL}/api/tags";

            using (HttpClient client = new HttpClient())
            {
                // 如果需要鉴权
                // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string err = await response.Content.ReadAsStringAsync();
                    throw new Exception($"调用 /api/tags 失败: {err}");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TagsResponse>(resultJson);
                return result?.Models;
            }
        }

        #endregion
    }
}
