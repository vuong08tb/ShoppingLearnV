// Chatbot Widget JavaScript

class Chatbot {
    constructor() {
        this.apiUrl = '/api/chat';
        this.sessionId = this.getOrCreateSessionId();
        this.isOpen = false;
        this.isTyping = false;

        this.init();
    }

    // Khởi tạo chatbot
    init() {
        this.createChatbotUI();
        this.attachEventListeners();
        this.showWelcomeMessage();
    }

    // Tạo UI cho chatbot
    createChatbotUI() {
        const chatbotHTML = `
            <!-- Nút mở chatbot -->
            <div id="chatbot-button">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                    <path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm0 14H6l-2 2V4h16v12z"/>
                    <path d="M7 9h2v2H7zm4 0h2v2h-2zm4 0h2v2h-2z"/>
                </svg>
            </div>

            <!-- Container chatbot -->
            <div id="chatbot-container">
                <div id="chatbot-wrapper">
                    <div id="chatbot-header">
                        <h3>💬 Trợ lý ShoppingLearn</h3>
                        <div class="chatbot-controls">
                            <button id="chatbot-minimize">−</button>
                            <button id="chatbot-close">&times;</button>
                        </div>
                    </div>

                    <div id="chatbot-messages"></div>

                    <div id="chatbot-input-area">
                        <input
                            type="text"
                            id="chatbot-input"
                            placeholder="Nhập câu hỏi của bạn..."
                            maxlength="500"
                        />
                        <button id="chatbot-send">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"/>
                            </svg>
                        </button>
                    </div>
                </div>
            </div>
        `;

        document.body.insertAdjacentHTML('beforeend', chatbotHTML);
    }

    // Gắn event listeners
    attachEventListeners() {
        const button = document.getElementById('chatbot-button');
        const minimizeBtn = document.getElementById('chatbot-minimize');
        const closeBtn = document.getElementById('chatbot-close');
        const sendBtn = document.getElementById('chatbot-send');
        const input = document.getElementById('chatbot-input');

        button.addEventListener('click', () => this.toggle());
        minimizeBtn.addEventListener('click', () => this.minimize());
        closeBtn.addEventListener('click', () => this.close());
        sendBtn.addEventListener('click', () => this.sendMessage());

        input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });
    }

    // Toggle chatbot
    toggle() {
        this.isOpen = !this.isOpen;
        const container = document.getElementById('chatbot-container');

        if (this.isOpen) {
            container.classList.add('show');
            document.getElementById('chatbot-input').focus();
        } else {
            container.classList.remove('show');
        }
    }

    // Thu nhỏ chatbot (giống như đóng)
    minimize() {
        this.isOpen = false;
        document.getElementById('chatbot-container').classList.remove('show');
    }

    // Đóng chatbot
    close() {
        this.isOpen = false;
        document.getElementById('chatbot-container').classList.remove('show');
    }

    // Hiển thị welcome message
    showWelcomeMessage() {
        const messagesDiv = document.getElementById('chatbot-messages');
        const welcomeHTML = `
            <div class="welcome-message">
                <h4>Xin chào! 👋</h4>
                <p>Tôi là trợ lý ảo của ShoppingLearn. Tôi có thể giúp bạn:</p>
                <ul style="text-align: left; display: inline-block; margin-top: 10px;">
                    <li>Tìm kiếm sản phẩm</li>
                    <li>Tư vấn thời trang</li>
                    <li>Kiểm tra giá & tồn kho</li>
                    <li>Chính sách đổi trả</li>
                </ul>
            </div>
        `;
        messagesDiv.innerHTML = welcomeHTML;
    }

    // Gửi message
    async sendMessage() {
        const input = document.getElementById('chatbot-input');
        const message = input.value.trim();

        if (!message || this.isTyping) {
            return;
        }

        // Hiển thị message của user
        this.addMessage('user', message);
        input.value = '';

        // Disable input
        this.setInputState(false);
        this.showTypingIndicator();

        try {
            // Gọi API
            const response = await fetch(`${this.apiUrl}/message`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    message: message,
                    sessionId: this.sessionId
                })
            });

            const data = await response.json();

            this.hideTypingIndicator();

            if (data.success) {
                // Hiển thị response từ bot
                this.addMessage('bot', data.reply);
            } else {
                this.addMessage('bot', data.errorMessage || 'Xin lỗi, đã có lỗi xảy ra.');
            }

        } catch (error) {
            console.error('Error sending message:', error);
            this.hideTypingIndicator();
            this.addMessage('bot', 'Xin lỗi, không thể kết nối đến server. Vui lòng thử lại sau.');
        } finally {
            this.setInputState(true);
        }
    }

    // Thêm message vào chat
    addMessage(role, content) {
        const messagesDiv = document.getElementById('chatbot-messages');

        // Xóa welcome message nếu có
        const welcomeMsg = messagesDiv.querySelector('.welcome-message');
        if (welcomeMsg) {
            welcomeMsg.remove();
        }

        const messageHTML = `
            <div class="message ${role}">
                <div class="message-bubble">
                    ${this.escapeHtml(content)}
                </div>
            </div>
        `;

        messagesDiv.insertAdjacentHTML('beforeend', messageHTML);
        this.scrollToBottom();
    }

    // Hiển thị typing indicator
    showTypingIndicator() {
        this.isTyping = true;
        const messagesDiv = document.getElementById('chatbot-messages');
        const typingHTML = `
            <div class="message bot" id="typing-indicator-msg">
                <div class="typing-indicator">
                    <span></span>
                    <span></span>
                    <span></span>
                </div>
            </div>
        `;
        messagesDiv.insertAdjacentHTML('beforeend', typingHTML);
        this.scrollToBottom();
    }

    // Ẩn typing indicator
    hideTypingIndicator() {
        this.isTyping = false;
        const indicator = document.getElementById('typing-indicator-msg');
        if (indicator) {
            indicator.remove();
        }
    }

    // Set trạng thái input
    setInputState(enabled) {
        const input = document.getElementById('chatbot-input');
        const sendBtn = document.getElementById('chatbot-send');

        input.disabled = !enabled;
        sendBtn.disabled = !enabled;
    }

    // Scroll xuống cuối
    scrollToBottom() {
        const messagesDiv = document.getElementById('chatbot-messages');
        messagesDiv.scrollTop = messagesDiv.scrollHeight;
    }

    // Escape HTML để tránh XSS
    escapeHtml(text) {
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return text.replace(/[&<>"']/g, m => map[m]);
    }

    // Lấy hoặc tạo session ID
    getOrCreateSessionId() {
        let sessionId = sessionStorage.getItem('chatbot_session_id');

        if (!sessionId) {
            sessionId = 'session_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
            sessionStorage.setItem('chatbot_session_id', sessionId);
        }

        return sessionId;
    }
}

// Khởi tạo chatbot khi DOM loaded
document.addEventListener('DOMContentLoaded', () => {
    window.chatbot = new Chatbot();
});
