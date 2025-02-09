<template>
  <div class="chatterMain">
    <div class="chat-window">
      <div v-for="(message, index) in messages" :key="index" class="message">
        <span class="user">{{ message.role }}:</span> {{ message.content }}
      </div>
    </div>
    <div class="input-area">
      <input v-model="newMessage" @keyup.enter="sendMessage" placeholder="输入对话..." />
      <button @click="sendMessage">发送</button>
    </div>
  </div>
</template>

<script>
export default {
 data() {
    return {
      // 例如：模型名称
      model: 'deepseek-r1:8b',
      stream: false,
      newMessage: '',
      // 初始化消息数组，所有消息都按照 ChatMessage 格式（role 和 content）保存
      messages: []
    };
  },


  methods: {
    async sendMessage() {
      if (this.newMessage.trim() === '') return;

      // 构造用户消息对象，属性名称要和后端一致：role 和 content
      const userMessage = {
        role: 'user',
        content: this.newMessage
      };

      // 添加用户消息到消息列表
      this.messages.push(userMessage);

      // 构造请求体，注意这里的 messages 键必须和后端实体一致
      const payload = {
        model: this.model,
        messages: this.messages,
        stream: this.stream
      };

      try {
        // 调用后端 API
        const response = await fetch('/api/OllamaService', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(payload)
        });
        const data = await response.json();

        // 假设后端返回 data.response 是 AI 回复的文本
        const aiMessage =  data.message

        // 添加 AI 响应到消息列表
        this.messages.push(aiMessage);
      } catch (error) {
        console.error('发送消息失败:', error);
      }

      // 清空输入框
      this.newMessage = '';
    }
  }
};
</script>

<style scoped>
.chatter {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.chat-window {
  flex: 1;
  overflow-y: auto;
  padding: 10px;
  border: 1px solid #ccc;
  margin-bottom: 10px;
}

.message {
  margin-bottom: 10px;
}

.user {
  font-weight: bold;
}

.input-area {
  display: flex;
}

input {
  flex: 1;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

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
>
