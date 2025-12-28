class AIRecommendationChat {
    constructor() {
        this.currentConversationId = null;
        this.isTyping = false;

        this.init();
    }

    init() {
        this.attachEventListeners();
        this.loadActiveConversation();
    }

    attachEventListeners() {
        // Send message
        document.getElementById('send-btn').addEventListener('click', () => this.sendMessage());
        document.getElementById('chat-input').addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });

        // New conversation
        document.getElementById('new-conversation-btn').addEventListener('click', () => this.startNewConversation());

        // Toggle sidebar (close button)
        const toggleBtn = document.getElementById('toggle-sidebar');
        if (toggleBtn) {
            toggleBtn.addEventListener('click', () => this.toggleSidebar());
        }

        // Open sidebar button
        const openBtn = document.getElementById('open-sidebar-btn');
        if (openBtn) {
            openBtn.addEventListener('click', () => this.toggleSidebar());
        }

        // Conversation selection
        this.attachConversationListeners();
    }

    attachConversationListeners() {
        // Conversation selection
        document.querySelectorAll('.conversation-item').forEach(item => {
            item.addEventListener('click', (e) => {
                if (!e.target.closest('.delete-conv')) {
                    this.loadConversation(item.dataset.id);
                }
            });
        });

        // Delete conversation
        document.querySelectorAll('.delete-conv').forEach(btn => {
            btn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.deleteConversation(btn.dataset.id);
            });
        });
    }

    async sendMessage() {
        const input = document.getElementById('chat-input');
        const message = input.value.trim();

        if (!message || this.isTyping) return;

        // Display user message
        this.addMessage('user', message);
        input.value = '';

        // Disable send button
        this.setInputState(false);

        // Show typing
        this.showTyping();

        try {
            const response = await fetch('/AIRecommendation/SendMessage', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    message: message,
                    conversationId: this.currentConversationId
                })
            });

            const data = await response.json();
            this.hideTyping();

            if (data.success) {
                const isNewConversation = !this.currentConversationId;
                this.currentConversationId = data.conversationId;
                this.addMessage('assistant', data.reply, data.products);

                // Nếu là conversation mới, load lại danh sách conversations
                if (isNewConversation) {
                    await this.reloadConversationsList();
                }
            } else {
                this.addMessage('assistant', data.errorMessage || 'Đã có lỗi xảy ra.');
            }
        } catch (error) {
            this.hideTyping();
            this.addMessage('assistant', 'Không thể kết nối đến server. Vui lòng thử lại.');
            console.error('Error:', error);
        } finally {
            this.setInputState(true);
        }
    }

    addMessage(role, content, products = null) {
        const messagesDiv = document.getElementById('chat-messages');

        // Remove welcome message if exists
        const welcomeMsg = messagesDiv.querySelector('.welcome-message');
        if (welcomeMsg) {
            welcomeMsg.remove();
        }

        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${role}`;

        let html = `<div class="message-bubble">${this.escapeHtml(content)}</div>`;

        if (products && products.length > 0) {
            html += '<div class="product-recommendations">';
            products.forEach(product => {
                html += `
                    <div class="product-card">
                        <img src="${product.image}" alt="${this.escapeHtml(product.name)}" onerror="this.src='/images/default-product.jpg'">
                        <div class="product-card-body">
                            <div class="product-card-title">${this.escapeHtml(product.name)}</div>
                            <div class="product-card-price">${this.formatPrice(product.price)}</div>
                            <div class="product-card-reason">${this.escapeHtml(product.reason)}</div>
                            <div class="product-card-actions">
                                <a href="/Product/Details/${product.id}" class="btn btn-sm btn-outline-primary">
                                    <i class="fa fa-eye"></i> Xem
                                </a>
                                <button class="btn btn-sm btn-primary" onclick="addToCartFromAI(${product.id})">
                                    <i class="fa fa-shopping-cart"></i> Giỏ
                                </button>
                            </div>
                        </div>
                    </div>
                `;
            });
            html += '</div>';
        }

        messageDiv.innerHTML = html;
        messagesDiv.appendChild(messageDiv);
        this.scrollToBottom();
    }

    showTyping() {
        this.isTyping = true;
        const messagesDiv = document.getElementById('chat-messages');
        const typingDiv = document.createElement('div');
        typingDiv.id = 'typing-indicator';
        typingDiv.className = 'message assistant';
        typingDiv.innerHTML = `
            <div class="message-bubble">
                <div class="typing-indicator">
                    <span></span>
                    <span></span>
                    <span></span>
                </div>
            </div>
        `;
        messagesDiv.appendChild(typingDiv);
        this.scrollToBottom();
    }

    hideTyping() {
        this.isTyping = false;
        const typing = document.getElementById('typing-indicator');
        if (typing) typing.remove();
    }

    async loadConversation(conversationId) {
        try {
            const response = await fetch(`/AIRecommendation/GetMessages?conversationId=${conversationId}`);
            const data = await response.json();

            if (data.success) {
                this.currentConversationId = conversationId;
                const messagesDiv = document.getElementById('chat-messages');
                messagesDiv.innerHTML = '';

                data.messages.forEach(msg => {
                    this.addMessage(msg.role, msg.content, msg.products);
                });

                // Update active state
                document.querySelectorAll('.conversation-item').forEach(item => {
                    item.classList.toggle('active', item.dataset.id === conversationId);
                });
            }
        } catch (error) {
            console.error('Error loading conversation:', error);
        }
    }

    async deleteConversation(conversationId) {
        if (!confirm('Bạn có chắc muốn xóa hội thoại này?')) return;

        try {
            const response = await fetch(`/AIRecommendation/DeleteConversation?conversationId=${conversationId}`, {
                method: 'DELETE'
            });

            const data = await response.json();
            if (data.success) {
                location.reload();
            } else {
                alert('Không thể xóa hội thoại. Vui lòng thử lại.');
            }
        } catch (error) {
            console.error('Error deleting conversation:', error);
            alert('Đã có lỗi xảy ra. Vui lòng thử lại.');
        }
    }

    startNewConversation() {
        this.currentConversationId = null;
        const messagesDiv = document.getElementById('chat-messages');
        messagesDiv.innerHTML = `
            <div class="welcome-message">
                <h4>Xin chào! 👋</h4>
                <p>Tôi là trợ lý AI cá nhân hóa của bạn. Hãy cho tôi biết bạn đang tìm kiếm gì:</p>
                <ul>
                    <li>🎯 "Gợi ý cho tôi áo thun nam phong cách casual"</li>
                    <li>👔 "Tư vấn trang phục đi làm cho nữ"</li>
                    <li>👗 "Tìm váy dự tiệc dưới 1 triệu"</li>
                    <li>🧥 "Áo khoác nào phù hợp với thời tiết mùa đông?"</li>
                </ul>
            </div>
        `;

        document.querySelectorAll('.conversation-item').forEach(item => {
            item.classList.remove('active');
        });

        document.getElementById('chat-input').focus();
    }

    toggleSidebar() {
        const sidebar = document.getElementById('conversation-sidebar');
        const openBtn = document.getElementById('open-sidebar-btn');

        sidebar.classList.toggle('collapsed');

        // Hiện/ẩn nút mở sidebar
        if (sidebar.classList.contains('collapsed')) {
            openBtn.style.display = 'block';
        } else {
            openBtn.style.display = 'none';
        }
    }

    async reloadConversationsList() {
        try {
            const response = await fetch('/AIRecommendation/GetConversations');
            const data = await response.json();

            if (data.success && data.conversations) {
                const conversationsList = document.getElementById('conversations-list');
                conversationsList.innerHTML = '';

                data.conversations.forEach(conv => {
                    const convId = conv.id || conv.Id;
                    const convTitle = conv.title || conv.Title;
                    const convUpdated = conv.updatedAt || conv.UpdatedAt;

                    const isActive = convId === this.currentConversationId;
                    const convDiv = document.createElement('div');
                    convDiv.className = `conversation-item ${isActive ? 'active' : ''}`;
                    convDiv.dataset.id = convId;
                    convDiv.innerHTML = `
                        <div class="conv-title">${this.escapeHtml(convTitle)}</div>
                        <div class="conv-date">${this.formatDate(convUpdated)}</div>
                        <button class="delete-conv" data-id="${convId}">
                            <i class="fa fa-trash"></i>
                        </button>
                    `;
                    conversationsList.appendChild(convDiv);
                });

                // Re-attach event listeners
                this.attachConversationListeners();
            }
        } catch (error) {
            console.error('Error reloading conversations:', error);
        }
    }

    formatDate(dateStr) {
        const date = new Date(dateStr);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        return `${day}/${month} ${hours}:${minutes}`;
    }

    loadActiveConversation() {
        const activeConv = document.querySelector('.conversation-item.active');
        if (activeConv) {
            this.loadConversation(activeConv.dataset.id);
        }
    }

    setInputState(enabled) {
        const input = document.getElementById('chat-input');
        const sendBtn = document.getElementById('send-btn');

        input.disabled = !enabled;
        sendBtn.disabled = !enabled;
    }

    scrollToBottom() {
        const messagesDiv = document.getElementById('chat-messages');
        messagesDiv.scrollTop = messagesDiv.scrollHeight;
    }

    escapeHtml(text) {
        if (!text) return '';
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return text.replace(/[&<>"']/g, m => map[m]);
    }

    formatPrice(price) {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(price);
    }
}

// Global function for Add to Cart (you may need to integrate with existing cart system)
function addToCartFromAI(productId) {
    // TODO: Integrate with your existing add to cart functionality
    // For now, just redirect to product details
    window.location.href = `/Product/Details/${productId}`;
}

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    window.aiChat = new AIRecommendationChat();
});
