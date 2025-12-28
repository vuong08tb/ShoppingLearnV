# 🛍️ ShoppingLearn - E-Commerce Fashion Platform

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)
![Entity Framework](https://img.shields.io/badge/EF_Core-8.0-512BD4?style=for-the-badge)
![AI Powered](https://img.shields.io/badge/AI_Chatbot-Google_Gemini-4285F4?style=for-the-badge&logo=google)

**Website thương mại điện tử thời trang với AI Chatbot thông minh**

[Tính năng](#-tính-năng) •
[Cài đặt](#-cài-đặt-và-cấu-hình) •
[Chatbot AI](#-chatbot-ai) •
[AI Gợi ý SP](#-ai-gợi-ý-sản-phẩm) •
[API](#-api-documentation)

</div>

---

## 📋 Mục lục

### 🚀 Getting Started
- [Giới thiệu](#-giới-thiệu)
- [Tính năng](#-tính-năng)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Cài đặt và Cấu hình](#-cài-đặt-và-cấu-hình)

### 🤖 AI Features
- [Chatbot AI](#-chatbot-ai)
- [AI Gợi ý sản phẩm](#-ai-gợi-ý-sản-phẩm)

### 📚 Technical Documentation
- [Kiến trúc hệ thống](#-kiến-trúc-hệ-thống)
- [Database Schema](#-database-schema)
- [API Documentation](#-api-documentation)

### 🔧 Operations
- [Deployment](#-deployment)
- [Troubleshooting](#-troubleshooting)

### 📝 Misc
- [Changelog](#-changelog)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Giới thiệu

**ShoppingLearn** là một nền tảng thương mại điện tử chuyên về thời trang, được xây dựng bằng ASP.NET Core 8.0 với tích hợp AI Chatbot thông minh sử dụng Google Gemini.

### ✨ Điểm nổi bật

- 🤖 **AI Chatbot** - Trợ lý ảo thông minh với Google Gemini 2.5 Flash
- 🎯 **AI Product Recommendation** - Gợi ý sản phẩm cá nhân hóa dựa trên sở thích
- 🛒 **E-Commerce đầy đủ** - Giỏ hàng, thanh toán, quản lý đơn hàng
- 💳 **Thanh toán đa dạng** - Momo, VNPay, COD
- 📊 **Admin Dashboard** - Quản lý sản phẩm, đơn hàng, thống kê
- 🔐 **Bảo mật cao** - ASP.NET Core Identity, Role-based authorization
- 📱 **Responsive** - Tương thích mọi thiết bị

---

## 🎨 Tính năng

### 🛍️ Khách hàng (Customer)

#### Shopping Features
- ✅ Xem danh sách sản phẩm theo danh mục, thương hiệu
- ✅ Tìm kiếm sản phẩm
- ✅ Lọc sản phẩm theo giá, size, màu sắc
- ✅ Xem chi tiết sản phẩm
- ✅ Đánh giá và review sản phẩm
- ✅ Thêm vào giỏ hàng
- ✅ Quản lý giỏ hàng (thêm, xóa, cập nhật số lượng)

#### Checkout & Payment
- ✅ Thanh toán với nhiều phương thức:
  - 💵 COD (Thanh toán khi nhận hàng)
  - 💳 Momo Wallet
  - 💳 VNPay
- ✅ Áp dụng mã giảm giá (Coupon)
- ✅ Chọn phương thức vận chuyển
- ✅ Theo dõi đơn hàng

#### Account Management
- ✅ Đăng ký, đăng nhập
- ✅ Quản lý thông tin cá nhân
- ✅ Lịch sử đơn hàng
- ✅ Địa chỉ giao hàng

#### 🤖 AI Features
- ✅ **AI Chatbot** - Tư vấn sản phẩm thời trang 24/7
  - Trả lời câu hỏi về giá cả, tồn kho
  - Hướng dẫn chính sách đổi trả, vận chuyển
  - Tìm kiếm sản phẩm theo yêu cầu
- ✅ **AI Product Recommendation** - Gợi ý cá nhân hóa
  - Quản lý sở thích (giới tính, phong cách, màu, size, giá)
  - Chat interface toàn màn hình
  - Lưu lịch sử hội thoại
  - Thêm sản phẩm vào giỏ trực tiếp từ chat
  - Gợi ý thông minh dựa trên AI + RAG

### 👨‍💼 Quản trị (Admin)

#### Product Management
- ✅ CRUD sản phẩm
- ✅ Quản lý danh mục, thương hiệu
- ✅ Upload ảnh sản phẩm
- ✅ Quản lý tồn kho

#### Order Management
- ✅ Xem danh sách đơn hàng
- ✅ Cập nhật trạng thái đơn hàng
- ✅ In hóa đơn
- ✅ Quản lý vận chuyển

#### User Management
- ✅ Quản lý khách hàng
- ✅ Phân quyền (Role management)
- ✅ Xem lịch sử hoạt động

#### Marketing
- ✅ Tạo mã giảm giá
- ✅ Quản lý khuyến mãi
- ✅ Gửi email marketing

#### Analytics & Reports
- ✅ Thống kê doanh thu
- ✅ Báo cáo bán hàng
- ✅ Top sản phẩm bán chạy
- ✅ Phân tích khách hàng

---

## 🛠 Công nghệ sử dụng

### Backend
- **Framework**: ASP.NET Core 8.0 (MVC Pattern)
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server
- **Authentication**: ASP.NET Core Identity
- **API**: RESTful API

### Frontend
- **Template Engine**: Razor Pages
- **CSS Framework**: Bootstrap 5
- **JavaScript**: Vanilla JS, jQuery
- **Icons**: Font Awesome

### AI & Machine Learning
- **LLM**: Google Gemini 2.5 Flash (FREE)
- **RAG**: Retrieval Augmented Generation
- **Vector DB**: ChromaDB (optional)

### Payment Integration
- **Momo**: Momo API
- **VNPay**: VNPay Payment Gateway

### Third-party Libraries
- **Newtonsoft.Json**: JSON processing
- **RestSharp**: HTTP client
- **Microsoft.SemanticKernel**: AI orchestration

---

## 📦 Cài đặt và Cấu hình

### Yêu cầu hệ thống

- ✅ .NET 8.0 SDK
- ✅ SQL Server 2019+
- ✅ Visual Studio 2022 (khuyến nghị) hoặc VS Code
- ✅ Node.js (optional, cho development tools)

### 🚀 Hướng dẫn cài đặt

#### Bước 1: Clone repository

```bash
git clone https://github.com/your-username/ShoppingLearn.git
cd ShoppingLearn
```

#### Bước 2: Restore packages

```bash
dotnet restore
```

#### Bước 3: Cấu hình appsettings.json

Mở [appsettings.json](ShoppingLearn/appsettings.json) và cập nhật:

##### 3.1. Connection String (Bắt buộc)

```json
{
  "ConnectionStrings": {
    "ConnectedDb": "Data Source=YOUR_SERVER;Initial Catalog=Shopping;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
  }
}
```

##### 3.2. Google Gemini API Key (Bắt buộc cho AI features)

**Lấy API Key MIỄN PHÍ:**
1. Truy cập: https://aistudio.google.com/app/apikey
2. Đăng nhập Google Account
3. Click "Create API Key"
4. Copy API key

**Cập nhật config:**
```json
{
  "Gemini": {
    "ApiKey": "PASTE_YOUR_API_KEY_HERE"
  }
}
```

##### 3.3. Payment Gateway (Optional)

**Momo API:**
```json
{
  "MomoAPI": {
    "MomoApiUrl": "https://test-payment.momo.vn/gw_payment/transactionProcessor",
    "SecretKey": "YOUR_SECRET_KEY",
    "AccessKey": "YOUR_ACCESS_KEY",
    "PartnerCode": "MOMO",
    "ReturnUrl": "https://yourdomain.com/Checkout/PaymentCallBack"
  }
}
```

**VNPay:**
```json
{
  "Vnpay": {
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET",
    "BaseUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"
  }
}
```

**AI Recommendation Config:**
```json
{
  "AIRecommendation": {
    "MaxProducts": 5
  }
}
```

#### Bước 4: Tạo Database

```bash
# Tạo database và chạy migrations
dotnet ef database update

# Hoặc sử dụng Package Manager Console trong Visual Studio
Update-Database
```

#### Bước 5: Import Sample Data (Optional)

Mở SQL Server Management Studio và chạy:
- `Database/SampleData.sql` - Sample products data

#### Bước 6: Chạy Application

```bash
# Command line
dotnet run

# Hoặc Visual Studio: Nhấn F5
```

Application sẽ chạy tại: `https://localhost:5065`

#### Bước 7: Tài khoản mặc định (sau khi import sample data)

- **Admin**: admin@example.com / Admin@123
- **User**: user@example.com / User@123

---

## 🤖 Chatbot AI

### 📖 Tổng quan

Chatbot là **widget popup** hỗ trợ khách hàng trả lời câu hỏi chung về sản phẩm, chính sách. Được xây dựng với kiến trúc **RAG (Retrieval Augmented Generation)**:

- **Google Gemini 2.5 Flash** - LLM miễn phí, nhanh
- **ChromaDB** - Vector database cho semantic search
- **SQL Server** - Real-time product data
- **Session-based** - Lưu lịch sử tạm thời trong session

### 🏗️ Kiến trúc Chatbot

```
┌─────────────────────────────────────────────┐
│           USER MESSAGE                       │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│        ChatController (API)                  │
│  POST /api/chat/message                      │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│      ChatbotService (Orchestrator)           │
│  - Intent Detection                          │
│  - Context Aggregation                       │
│  - Session Management                        │
└─────────────────────────────────────────────┘
         │              │              │
         ▼              ▼              ▼
┌──────────────┐ ┌─────────────┐ ┌─────────────┐
│ GeminiSvc    │ │ ChromaSvc   │ │ SqlQuerySvc │
│ (LLM)        │ │ (RAG)       │ │ (Database)  │
└──────────────┘ └─────────────┘ └─────────────┘
         │              │              │
         ▼              ▼              ▼
┌──────────────┐ ┌─────────────┐ ┌─────────────┐
│ Gemini API   │ │ Knowledge   │ │ SQL Server  │
│ (Cloud)      │ │ Base Files  │ │ (Products)  │
└──────────────┘ └─────────────┘ └─────────────┘
```

### Tính năng Chatbot

#### 1. Intent Detection
Tự động phát hiện ý định người dùng:
- 💰 **Price Query** - "Giá bao nhiêu?"
- 📦 **Stock Check** - "Còn hàng không?"
- 🔍 **Product Search** - "Tìm áo thun"
- ❓ **General Question** - "Chính sách đổi trả?"

#### 2. RAG (Retrieval Augmented Generation)
- Tìm kiếm thông tin từ Knowledge Base
- Top-K similarity search
- Context-aware responses

#### 3. Database Integration
- Real-time product queries
- Price check
- Stock availability
- Category information

#### 4. Session Management
- Lưu lịch sử chat theo session
- Context-aware conversation
- Rate limiting: 20 messages/phút

### Cấu trúc Code (SOLID Principles)

```
Services/Chatbot/
├── IGeminiService.cs          # Interface - Gemini API
├── GeminiService.cs           # Implementation
├── IChromaService.cs          # Interface - RAG
├── ChromaService.cs           # Implementation
├── ISqlQueryService.cs        # Interface - Database
├── SqlQueryService.cs         # Implementation
├── IChatbotService.cs         # Interface - Orchestrator
└── ChatbotService.cs          # Implementation
```

**Design Patterns:**
- ✅ Dependency Injection
- ✅ Interface Segregation
- ✅ Single Responsibility
- ✅ Repository Pattern

### 🎯 Tính năng Chatbot

- ✅ **Intent Detection** - Phát hiện ý định người dùng
- ✅ **RAG Search** - Tìm kiếm thông tin từ Knowledge Base
- ✅ **Database Integration** - Truy vấn real-time product data
- ✅ **Session Management** - Lưu lịch sử chat theo session
- ✅ **Rate Limiting** - 20 messages/phút

### 🚀 Cách sử dụng

#### Setup Chatbot

**Bước 1:** Cấu hình Gemini API Key (xem phần [Cài đặt](#-cài-đặt-và-cấu-hình))

**Bước 2:** Chatbot đã được tích hợp sẵn trong [_Layout.cshtml](ShoppingLearn/Views/Shared/_Layout.cshtml)

**Bước 3:** Thử nghiệm
1. Chạy application
2. Mở trang chủ
3. Click nút chat ở góc phải màn hình
4. Thử các câu hỏi:
   - "Có áo thun nam nào giá rẻ không?"
   - "Chính sách đổi trả như thế nào?"
   - "Tìm giúp tôi quần jean nữ"
   - "Thời gian giao hàng bao lâu?"

### 📡 API Endpoints

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | `/api/chat/message` | Gửi message cho chatbot |
| GET | `/api/chat/history/{sessionId}` | Lấy lịch sử chat |
| DELETE | `/api/chat/history/{sessionId}` | Xóa lịch sử chat |
| GET | `/api/chat/health` | Health check |

**Example Request:**
```json
POST /api/chat/message
{
  "message": "Có áo thun nào giá rẻ không?",
  "sessionId": "session_123456"
}
```

**Example Response:**
```json
{
  "reply": "Có ạ! Chúng tôi có Áo Thun Nam Basic Cotton giá chỉ 199.000đ...",
  "sources": [
    "Áo Thun Nam Basic Cotton - Giá: 199.000đ - Tồn kho: 50"
  ],
  "success": true,
  "sessionId": "session_123456"
}
```

### Knowledge Base

Chatbot học từ các tài liệu trong `Knowledge/`:

```
Knowledge/
├── faq.md              # Câu hỏi thường gặp
├── products.txt        # Thông tin sản phẩm
└── style-guide.txt     # Hướng dẫn phối đồ
```

**Để thêm kiến thức mới:**
1. Tạo file `.txt` hoặc `.md` trong `Knowledge/`
2. Viết nội dung theo format dễ đọc
3. Chatbot tự động đọc khi khởi động

---

## 🎯 AI Gợi ý sản phẩm

### 📖 Tổng quan

Tính năng **AI Product Recommendation** là **trang chuyên dụng** cung cấp trải nghiệm tư vấn mua sắm cá nhân hóa, sử dụng AI để phân tích sở thích và gợi ý sản phẩm phù hợp nhất cho từng khách hàng.

### ✨ Điểm nổi bật

- 🎨 **Giao diện toàn màn hình** - Chat interface chuyên nghiệp với sidebar lịch sử
- 🧠 **Cá nhân hóa thông minh** - AI học từ sở thích người dùng (giới tính, phong cách, màu sắc, size, khoảng giá)
- 💾 **Lưu trữ lịch sử** - Toàn bộ hội thoại được lưu vào database
- 🔄 **CRUD Conversations** - Tạo mới, xem, xóa các cuộc hội thoại
- 🛍️ **Product Cards** - Hiển thị sản phẩm đề xuất với ảnh, giá, lý do
- 🔗 **Deep Integration** - Kết hợp Gemini AI + ChromaDB RAG + SQL queries

### 🏗️ Kiến trúc AI Recommendation

```
┌─────────────────────────────────────────────┐
│      USER (với preferences profile)         │
│  Gender, Style, Colors, Size, PriceRange... │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│    AIRecommendationController                │
│  POST /AIRecommendation/SendMessage          │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  ProductRecommendationService (Orchestrator) │
│  - Build personalized prompt                 │
│  - Manage conversation history               │
│  - Extract product keywords                  │
└─────────────────────────────────────────────┘
         │              │              │
         ▼              ▼              ▼
┌──────────────┐ ┌─────────────┐ ┌─────────────┐
│ GeminiSvc    │ │ ChromaSvc   │ │ SqlQuerySvc │
│ (AI Reply)   │ │ (Context)   │ │ (Products)  │
└──────────────┘ └─────────────┘ └─────────────┘
         │              │              │
         ▼              ▼              ▼
┌──────────────┐ ┌─────────────┐ ┌─────────────┐
│ Gemini API   │ │ Knowledge   │ │ SQL Server  │
│ (Reasoning)  │ │ Base (RAG)  │ │ (Products)  │
└──────────────┘ └─────────────┘ └─────────────┘
         │              │              │
         └──────────────┴──────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│     ChatHistoryService (Persistence)         │
│  - Save messages to DB                       │
│  - Manage conversations                      │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│         Database Tables                      │
│  - ChatConversations                         │
│  - ChatMessages                              │
│  - AspNetUsers (with preferences)            │
└─────────────────────────────────────────────┘
```

### 🎯 Tính năng chính

#### 1. User Preferences Management

**Admin có thể quản lý:**
- Vào **Admin → Users → Edit** để cập nhật thông tin khách hàng:
  - 👤 Giới tính (Nam/Nữ/Khác)
  - 🎂 Ngày sinh
  - 🎨 Phong cách ưa thích (Sporty, Casual, Formal, Street, Vintage...)
  - 🌈 Màu sắc yêu thích (danh sách màu cách nhau dấu phẩy)
  - 📏 Size thường mặc (S, M, L, XL, XXL)
  - 💰 Khoảng giá (Budget, Medium, Premium)
  - ❤️ Sở thích (Thể thao, Công sở, Dạo phố...)

**User tự quản lý:**
- Vào **Account → Update Account Info** để cập nhật sở thích của mình

#### 2. AI Chat Interface

**Full-page chat layout với:**
- 📋 **Sidebar** (có thể thu gọn):
  - Danh sách hội thoại
  - Nút tạo hội thoại mới
  - Nút toggle để ẩn/hiện
  - Nút xóa từng conversation

- 💬 **Chat Area**:
  - Welcome message khi mở trang
  - Hiển thị tin nhắn user và AI
  - Typing indicator khi AI đang suy nghĩ
  - Product cards với:
    - Ảnh sản phẩm
    - Tên sản phẩm
    - Giá (định dạng VNĐ)
    - Lý do đề xuất
    - Nút "Thêm vào giỏ hàng"
    - Link xem chi tiết

- ⌨️ **Input Area**:
  - Textarea auto-resize
  - Nút gửi tin nhắn
  - Enter để gửi, Shift+Enter để xuống dòng

#### 3. Personalized Recommendations

AI sử dụng thông tin cá nhân để:
- 🎯 **Lọc sản phẩm theo preferences**:
  - Giới tính → Sản phẩm nam/nữ
  - Phong cách → Category matching
  - Size → Stock filtering
  - Khoảng giá → Price range:
    - Budget: < 500,000đ
    - Medium: 500,000 - 1,500,000đ
    - Premium: > 1,500,000đ

- 🧠 **Build context-aware prompt** với thông tin chi tiết về user
- 📊 **Intelligent product matching**:
  - Keyword extraction từ message
  - Semantic search qua ChromaDB
  - SQL queries với preference filters
  - Tối đa 5 sản phẩm mỗi lần (configurable)

#### 4. Conversation Management

- ✅ **Auto-create conversation** khi user gửi message đầu tiên
- ✅ **Auto-generate title** từ message đầu tiên (tối đa 50 ký tự)
- ✅ **Load conversation history** khi click vào sidebar
- ✅ **Delete conversation** với xác nhận
- ✅ **Soft delete** - Đánh dấu IsDeleted thay vì xóa vĩnh viễn

#### 5. Database Schema Extensions

**ChatConversations Table:**
```sql
CREATE TABLE ChatConversations (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id),
    Title NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    INDEX IX_ChatConversations_UserId (UserId)
);
```

**ChatMessages Table:**
```sql
CREATE TABLE ChatMessages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ConversationId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES ChatConversations(Id),
    Role NVARCHAR(20) NOT NULL,  -- 'user' or 'assistant'
    Content NVARCHAR(MAX) NOT NULL,
    ProductRecommendations NVARCHAR(MAX) NULL,  -- JSON array
    CreatedAt DATETIME2 NOT NULL,
    INDEX IX_ChatMessages_ConversationId (ConversationId)
);
```

**AspNetUsers Extensions:**
```sql
ALTER TABLE AspNetUsers ADD (
    Gender NVARCHAR(MAX) NULL,
    DateOfBirth DATETIME2 NULL,
    PreferredStyle NVARCHAR(MAX) NULL,
    PreferredColors NVARCHAR(MAX) NULL,
    SizePreference NVARCHAR(MAX) NULL,
    PriceRange NVARCHAR(MAX) NULL,
    Interests NVARCHAR(MAX) NULL
);
```

### 🛠️ Services Architecture (SOLID)

```
Services/Chatbot/
├── IChatHistoryService.cs         # Interface - Conversation CRUD
├── ChatHistoryService.cs          # Implementation
├── IProductRecommendationService.cs  # Interface - Main orchestrator
└── ProductRecommendationService.cs   # Implementation
```

**Design Patterns:**
- ✅ Interface Segregation Principle
- ✅ Dependency Injection
- ✅ Repository Pattern (via DataContext)
- ✅ Service Layer Pattern
- ✅ Single Responsibility Principle

### 📡 API Endpoints

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/AIRecommendation` | Trang chính (full-page chat) |
| POST | `/AIRecommendation/SendMessage` | Gửi message, nhận AI reply + products |
| GET | `/AIRecommendation/GetConversations` | Lấy danh sách conversations |
| GET | `/AIRecommendation/GetMessages?conversationId={id}` | Lấy messages của conversation |
| DELETE | `/AIRecommendation/DeleteConversation?conversationId={id}` | Xóa conversation |
| POST | `/AIRecommendation/NewConversation` | Tạo conversation mới |

**Example Request:**
```json
POST /AIRecommendation/SendMessage
{
  "message": "Tìm giúp tôi áo sơ mi công sở",
  "conversationId": null  // null for new conversation
}
```

**Example Response:**
```json
{
  "success": true,
  "reply": "Dựa trên phong cách Formal của bạn, tôi gợi ý những mẫu áo sơ mi sau...",
  "conversationId": "guid-here",
  "products": [
    {
      "id": 123,
      "name": "Áo Sơ Mi Nam Trắng Công Sở",
      "price": 450000,
      "image": "/media/products/ao-so-mi-nam.jpg",
      "slug": "ao-so-mi-nam-trang-cong-so",
      "reason": "Phù hợp phong cách Formal, size M, giá Medium"
    }
  ]
}
```

### ⚙️ Configuration

**appsettings.json:**
```json
{
  "AIRecommendation": {
    "MaxProducts": 5  // Số sản phẩm tối đa mỗi lần gợi ý
  },
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  }
}
```

### 🚀 Cách sử dụng

#### 👤 Cho User:

1. **Đăng nhập** vào tài khoản
2. **Cập nhật sở thích** tại [Account → Update Account Info](ShoppingLearn/Views/Account/UpdateAccount.cshtml)
   - Giới tính, ngày sinh, phong cách, màu sắc yêu thích, size, khoảng giá
3. **Vào trang AI Recommendation** từ menu: Account → AI Gợi ý sản phẩm
4. **Bắt đầu chat** với AI:
   - "Tìm giúp tôi áo khoác mùa đông"
   - "Có quần jean nào phù hợp với tôi không?"
   - "Gợi ý trang phục đi chơi cuối tuần"
5. **Xem sản phẩm đề xuất** với lý do cụ thể
6. **Thêm vào giỏ hàng** trực tiếp từ chat
7. **Quản lý conversations** trong sidebar

#### 👨‍💼 Cho Admin:

1. Vào [Admin → Users → Edit](ShoppingLearn/Areas/Admin/Views/User/Edit.cshtml)
2. Cập nhật **AI Recommendation Preferences** cho khách hàng
3. Lưu thông tin
4. User sẽ nhận được recommendations cá nhân hóa

### 📂 Cấu trúc Files

```
Controllers/
└── AIRecommendationController.cs      # Main controller

Services/Chatbot/
├── IChatHistoryService.cs             # Conversation CRUD
├── ChatHistoryService.cs
├── IProductRecommendationService.cs   # Main orchestrator
└── ProductRecommendationService.cs

Models/
├── ChatConversation.cs                # Conversation entity
├── ChatMessage.cs                     # Message entity
└── AIRecommendation/                  # Request/Response models

Views/AIRecommendation/
└── Index.cshtml                       # Full-page chat UI

Views/Account/
└── UpdateAccount.cshtml               # User preferences

Areas/Admin/Views/User/
└── Edit.cshtml                        # Admin edit preferences

wwwroot/
├── css/ai-recommendation.css          # Styling
└── js/ai-recommendation.js            # Frontend logic
```

### 🆚 So sánh với Chatbot

| Feature | Chatbot (Widget) | AI Recommendation (Full-page) |
|---------|------------------|-------------------------------|
| Display | Popup widget | Full-page dedicated |
| User Preferences | ❌ No | ✅ Yes (7 fields) |
| Personalization | ❌ Generic | ✅ Highly personalized |
| Conversation History | Session-based | ✅ Database persistent |
| Product Display | Text only | ✅ Rich product cards |
| CRUD Conversations | ❌ No | ✅ Yes |
| Add to Cart | ❌ No | ✅ Direct from chat |
| Use Case | Quick Q&A | Shopping consultation |

### ⚠️ Troubleshooting AI Recommendation

| Lỗi | Nguyên nhân | Giải pháp |
|-----|-------------|-----------|
| Không có sản phẩm đề xuất | User chưa cập nhật preferences | Cập nhật preferences tại Account → Update Account Info |
| | Database thiếu sản phẩm matching | Import sample data hoặc thêm sản phẩm |
| | MaxProducts = 0 | Check `AIRecommendation.MaxProducts` trong appsettings.json |
| Sidebar không hiển thị | User chưa đăng nhập | Đăng nhập lại |
| | Database chưa có conversations | Gửi message đầu tiên để tạo conversation |
| Message không gửi được | Gemini API key sai/hết hạn | Kiểm tra API key trong appsettings.json |
| | Services chưa được register | Verify `Program.cs` đã có `AddScoped<IProductRecommendationService>` |
| | Network error | Xem Network tab trong browser DevTools |

---

## 🏗 Kiến trúc hệ thống

### Layered Architecture

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                   │
│  - Controllers (MVC)                         │
│  - Views (Razor)                             │
│  - API Controllers                           │
└─────────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────────┐
│         Business Logic Layer                 │
│  - Services                                  │
│  - Domain Models                             │
│  - Business Rules                            │
└─────────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────────┐
│         Data Access Layer                    │
│  - DataContext (EF Core)                     │
│  - Repository Pattern                        │
│  - Migrations                                │
└─────────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────────┐
│         Database Layer                       │
│  - SQL Server                                │
│  - Tables, Indexes, Constraints              │
└─────────────────────────────────────────────┘
```

### Project Structure

```
ShoppingLearn/
│
├── Areas/
│   └── Admin/                  # Admin area
│       ├── Controllers/        # Admin controllers
│       └── Views/              # Admin views
│
├── Controllers/                # Customer controllers
│   ├── HomeController.cs
│   ├── ProductController.cs
│   ├── CartController.cs
│   ├── CheckoutController.cs
│   └── ChatController.cs       # Chatbot API
│
├── Models/                     # Domain models
│   ├── ProductModel.cs
│   ├── CategoryModel.cs
│   ├── OrderModel.cs
│   └── Chatbot/               # Chatbot models
│
├── Services/                   # Business services
│   ├── Chatbot/               # Chatbot services
│   ├── Momo/                  # Momo payment
│   └── Vnpay/                 # VNPay payment
│
├── Repository/                 # Data access
│   └── DataContext.cs         # EF Core DbContext
│
├── Views/                      # Razor views
│   ├── Shared/
│   ├── Home/
│   ├── Product/
│   └── Cart/
│
├── wwwroot/                    # Static files
│   ├── css/
│   ├── js/
│   ├── images/
│   └── lib/
│
├── Knowledge/                  # Chatbot knowledge base
│   ├── faq.md
│   └── products.txt
│
├── Database/                   # Database scripts
│   └── SampleData.sql
│
├── appsettings.json           # Configuration
└── Program.cs                 # Entry point
```

---

## 📊 Database Schema

### Core Tables

#### Products
```sql
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200),
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2),
    Quantity INT,
    CategoryId INT,
    BrandId INT,
    Image NVARCHAR(500),
    Slug NVARCHAR(200),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    FOREIGN KEY (BrandId) REFERENCES Brands(Id)
);
```

#### Categories
```sql
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Description NVARCHAR(MAX),
    Slug NVARCHAR(100),
    Status INT
);
```

#### Orders
```sql
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    OrderCode NVARCHAR(50),
    UserName NVARCHAR(256),
    TotalAmount DECIMAL(18,2),
    Status INT,
    PaymentMethod NVARCHAR(50),
    ShippingAddress NVARCHAR(MAX),
    CreatedDate DATETIME,
    UpdatedDate DATETIME
);
```

#### OrderDetails
```sql
CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT,
    ProductId INT,
    Quantity INT,
    Price DECIMAL(18,2),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
```

#### ChatConversations (AI Recommendation)
```sql
CREATE TABLE ChatConversations (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId NVARCHAR(450),
    Title NVARCHAR(200),
    CreatedAt DATETIME2,
    UpdatedAt DATETIME2,
    IsDeleted BIT,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);
```

#### ChatMessages (AI Recommendation)
```sql
CREATE TABLE ChatMessages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ConversationId UNIQUEIDENTIFIER,
    Role NVARCHAR(20),  -- 'user' or 'assistant'
    Content NVARCHAR(MAX),
    ProductRecommendations NVARCHAR(MAX),  -- JSON
    CreatedAt DATETIME2,
    FOREIGN KEY (ConversationId) REFERENCES ChatConversations(Id)
);
```

### Relationships

```
Categories 1 ──────────── ∞ Products
Brands 1 ──────────────── ∞ Products
Users 1 ───────────────── ∞ Orders
Users 1 ───────────────── ∞ ChatConversations (NEW)
Orders 1 ──────────────── ∞ OrderDetails
Products 1 ────────────── ∞ OrderDetails
Products 1 ────────────── ∞ Ratings
ChatConversations 1 ───── ∞ ChatMessages (NEW)
```
---

## 🚀 Deployment

### Prerequisites

- Windows Server hoặc Linux Server
- SQL Server
- .NET 8.0 Runtime
- IIS hoặc Nginx

### Build for Production

```bash
# Publish
dotnet publish -c Release -o ./publish

# Files sẽ được tạo trong folder ./publish
```

### IIS Deployment

1. Install .NET 8.0 Hosting Bundle
2. Tạo Application Pool (.NET CLR Version: No Managed Code)
3. Deploy files từ `./publish` vào `wwwroot`
4. Cấu hình Connection String trong `appsettings.json`
5. Restart IIS

### Linux Deployment (Nginx)

```bash
# Install .NET Runtime
wget https://dot.net/v1/dotnet-install.sh
sudo bash dotnet-install.sh --channel 8.0 --runtime aspnetcore

# Copy files
sudo cp -r ./publish /var/www/shoppinglearn

# Configure Nginx
sudo nano /etc/nginx/sites-available/shoppinglearn

# Start service
sudo systemctl start shoppinglearn
sudo systemctl enable shoppinglearn
```

---

## 🔧 Troubleshooting

### 🤖 Chatbot Issues

| Lỗi | Giải pháp |
|-----|-----------|
| Chatbot không hiển thị | Verify CSS/JS đã được include trong [_Layout.cshtml](ShoppingLearn/Views/Shared/_Layout.cshtml) |
| API trả về lỗi 500 | Kiểm tra Gemini API key trong appsettings.json |
| Chatbot trả lời sai | Bổ sung tài liệu vào folder `Knowledge/` |
| Rate limit exceeded | Đợi 1 phút (limit: 20 messages/phút) |

### 🗄️ Database Issues

| Lỗi | Giải pháp |
|-----|-----------|
| Migration errors | `dotnet ef migrations remove` → `dotnet ef migrations add NewMigration` → `dotnet ef database update` |
| Connection failed | Kiểm tra Connection String trong appsettings.json |
| Login failed for user | Đảm bảo SQL Server đang chạy và user có quyền truy cập |

### 💳 Payment Issues

| Lỗi | Giải pháp |
|-----|-----------|
| Momo/VNPay payment failed | Kiểm tra API keys, SecretKey trong appsettings.json |
| Callback không nhận được | Verify ReturnUrl và NotifyUrl đúng format |
| Sandbox errors | Test trong môi trường sandbox trước khi production |

### ⚙️ General Issues

| Lỗi | Giải pháp |
|-----|-----------|
| Port already in use | Đổi port trong [Properties/launchSettings.json](ShoppingLearn/Properties/launchSettings.json) |
| Static files not loading | Xóa cache browser (Ctrl+Shift+Delete) |
| Session expired | Đăng nhập lại |

---

## 📚 API Documentation

### Product APIs

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Product` | Danh sách sản phẩm |
| GET | `/Product/Details/{id}` | Chi tiết sản phẩm |
| GET | `/Category/{slug}` | Sản phẩm theo danh mục |
| GET | `/Brand/{slug}` | Sản phẩm theo thương hiệu |

### Cart APIs

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/Cart/Add` | Thêm vào giỏ hàng |
| POST | `/Cart/Update` | Cập nhật số lượng |
| POST | `/Cart/Remove` | Xóa khỏi giỏ |
| GET | `/Cart` | Xem giỏ hàng |

### Checkout APIs

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/Checkout` | Đặt hàng |
| POST | `/Checkout/PaymentMomo` | Thanh toán Momo |
| POST | `/Checkout/PaymentVnpay` | Thanh toán VNPay |
| GET | `/Checkout/PaymentCallBack` | Momo callback |
| GET | `/Checkout/PaymentCallbackVnpay` | VNPay callback |

### Chatbot APIs

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/chat/message` | Gửi message |
| GET | `/api/chat/history/{sessionId}` | Lịch sử chat |
| DELETE | `/api/chat/history/{sessionId}` | Xóa lịch sử |
| GET | `/api/chat/health` | Health check |

### AI Recommendation APIs

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/AIRecommendation` | Trang chat chính |
| POST | `/AIRecommendation/SendMessage` | Gửi message + nhận AI reply |
| GET | `/AIRecommendation/GetConversations` | Danh sách conversations |
| GET | `/AIRecommendation/GetMessages?conversationId={id}` | Messages của conversation |
| DELETE | `/AIRecommendation/DeleteConversation?conversationId={id}` | Xóa conversation |
| POST | `/AIRecommendation/NewConversation` | Tạo conversation mới |

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding Standards

- Follow C# coding conventions
- Use meaningful variable/method names
- Add comments for complex logic
- Write unit tests for new features
- Follow SOLID principles

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **Trần Hữu Vượng** - *Sinh viên ngành công nghệ thông tin- Đại học Điện Lực*

---

## 🙏 Acknowledgments

- ASP.NET Core Team
- Entity Framework Core Team
- Google Gemini Team
- Bootstrap Team
- Font Awesome
- All contributors

---

## 📞 Contact & Support

- **Email**: support@shoppinglearn.vn
- **Website**: https://shoppinglearn.vn
- **GitHub Issues**: [Create an issue](https://github.com/yourusername/ShoppingLearn/issues)
- **Documentation**: [Full Docs](https://docs.shoppinglearn.vn)

---

<div align="center">

**Made with ❤️ using ASP.NET Core 8.0**

⭐ Star this repo if you find it helpful!

[Back to top](#️-shoppinglearn---e-commerce-fashion-platform)

</div>
