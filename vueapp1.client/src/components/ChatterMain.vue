<template>
  <div class="chatterMain">
    <div class="chat-window">
      <div v-for="(message, index) in messages" :key="index" class="message">
        <span class="user">{{ message.role }}:</span>
        <!-- 如果消息角色是 ai，则用 v-html 渲染格式化后的回复 -->
        <div v-if="message.role == 'assistant'" v-html="formatAiMessage(message.content)"></div>
        <!-- 其他角色直接以纯文本显示 -->
        <div v-else>{{ message.content }}</div>
      </div>
    </div>
    <div class="input-area">
      <textarea v-model="newMessage" @keyup.enter="sendMessage" placeholder="输入对话..." rows="1" ref="messageInput"></textarea>
      <button @click="sendMessage">发送</button>
    </div>
  </div>
</template>

<script>
  import { marked } from 'marked';

  export default {
    data() {
      return {
        model: 'deepseek-r1:8b',
        stream: false,
        newMessage: '',
        messages: []
      };
    },
    methods: {
      async sendMessage() {
        if (this.newMessage.trim() === '') return;

        const userMessage = {
          role: 'user',
          content: this.newMessage
        };

        this.messages.push(userMessage);

        const payload = {
          model: this.model,
          messages: this.messages,
          stream: this.stream
        };

        try {
          const response = await fetch('/api/OllamaService', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
          });
          const data = await response.json();

          const aiMessage = data.message;

          this.messages.push(aiMessage);
        } catch (error) {
          console.error('发送消息失败:', error);
        }

        this.newMessage = '';
        this.$refs.messageInput.style.height = 'auto';
      },
      // 格式化 AI 回复，提取 <think> 部分并渲染 Markdown 内容
      formatAiMessage(content) {
        console.log("原始内容：", content);
        // 匹配 <think> 标签里的内容（忽略大小写）
        const thinkRegex = /<think>([\s\S]*?)<\/think>/i;
        let thinkContent = '';
        let markdownContent = content;
        const match = thinkRegex.exec(content);
        if (match) {
          thinkContent = match[1];
          // 移除 <think> 标签部分
          markdownContent = content.replace(thinkRegex, '');
        }
        // 去除前后空白（有时有助于解析）
        markdownContent = markdownContent.trim();
        // 使用 marked 将 Markdown 转换为 HTML
        const renderedMarkdown = marked.parse(markdownContent + "\n");
        console.log("解析后的 HTML：", renderedMarkdown);

        let finalHtml = '';
        if (thinkContent) {
          finalHtml += `<div class="ai-think"><think>${thinkContent}</think></div>`;
        }
        finalHtml += `<div class="ai-markdown">${renderedMarkdown}</div>`;
        return finalHtml;
      }
    },
    watch: {
      newMessage() {
        this.$nextTick(() => {
          const textarea = this.$refs.messageInput;
          textarea.style.height = 'auto';
          textarea.style.height = `${Math.min(textarea.scrollHeight, 5 * 24)}px`;
        });
      }
    }
  };
</script>

<style scoped>
  /* 清除默认全局边距，确保 100% 高度正常工作 */
  html, body {
    margin: 0;
    padding: 0;
    height: 100%;
  }

  /* 统一 box-sizing，保证内边距和边框包含在宽高内 */
  *,
  *::before,
  *::after {
    box-sizing: border-box;
  }

  /* 整体容器：使用固定定位，始终铺满可视区域 */
  .chatterMain {
    position: fixed;
    top: 0;
    bottom: 0;
    left: 50%;
    transform: translateX(-50%);
    width: 100%;
    max-width: 600px; /* 可选：限定最大宽度 */
    display: flex;
    flex-direction: column;
    border: 1px solid #ccc;
    border-radius: 4px;
    overflow: hidden;
  }

  /* 聊天记录区域：占满剩余空间，内容超出时滚动 */
  .chat-window {
    flex: 1;
    overflow-y: auto;
    padding: 10px;
  }

  /* 单条消息样式 */
  .message {
    margin-bottom: 10px;
    color: #fff;
  }

  .user {
    font-weight: bold;
    color:blue;
  }

  /* AI 回复中的 think 部分样式 */
  /deep/ .ai-think think {
    color: gray;
    display: block;
    margin-bottom: 0.5em;
    font-style: italic;
  }

  /* AI 回复中的 Markdown 内容样式 */
  .ai-markdown h1,
  .ai-markdown h2,
  .ai-markdown h3,
  .ai-markdown h4,
  .ai-markdown h5,
  .ai-markdown h6 {
    margin-top: 1.2em;
    margin-bottom: 0.6em;
  }

  .ai-markdown pre {
    background: #f0f0f0;
    padding: 0.8em;
    overflow: auto;
    border-radius: 4px;
  }

  .ai-markdown code {
    background: #f7f7f7;
    padding: 2px 4px;
    border-radius: 3px;
  }

  .ai-markdown img {
    max-width: 100%;
    height: auto;
  }

  /* 输入区域：固定在容器底部 */
  .input-area {
    display: flex;
    align-items: center;
    padding: 10px;
    border-top: 1px solid #ccc;
    flex-shrink: 0;
  }

  /* 文本框：宽度自适应，风格统一 */
  textarea {
    flex: 1;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
    resize: none;
    overflow: hidden;
    max-height: 120px; /* 限制为最多 5 行 */
  }

  /* 按钮样式 */
  button {
    padding: 10px 20px;
    margin-left: 10px;
    border: none;
    background-color: #007bff;
    color: white;
    border-radius: 4px;
    cursor: pointer;
  }

    button:hover {
      background-color: #0056b3;
    }
</style>
