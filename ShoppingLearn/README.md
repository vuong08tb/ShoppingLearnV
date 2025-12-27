# 🛍️ ShoppingLearn - E-Commerce Fashion Platform

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)
![Entity Framework](https://img.shields.io/badge/EF_Core-8.0-512BD4?style=for-the-badge)
![AI Powered](https://img.shields.io/badge/AI_Chatbot-Google_Gemini-4285F4?style=for-the-badge&logo=google)

**Website thương mại điện tử thời trang với AI Chatbot thông minh**

[Tính năng](#-tính-năng) •
[Công nghệ](#-công-nghệ-sử-dụng) •
[Cài đặt](#-cài-đặt) •
[Chatbot AI](#-chatbot-ai) •
[API Documentation](#-api-documentation)

</div>

---

## 📋 Mục lục

- [Giới thiệu](#-giới-thiệu)
- [Tính năng](#-tính-năng)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Cài đặt](#-cài-đặt)
- [Cấu hình](#-cấu-hình)
- [Chatbot AI](#-chatbot-ai)
- [Kiến trúc hệ thống](#-kiến-trúc-hệ-thống)
- [API Documentation](#-api-documentation)
- [Database Schema](#-database-schema)
- [Testing](#-testing)
- [Deployment](#-deployment)
- [Troubleshooting](#-troubleshooting)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Giới thiệu

**ShoppingLearn** là một nền tảng thương mại điện tử chuyên về thời trang, được xây dựng bằng ASP.NET Core 8.0 với tích hợp AI Chatbot thông minh sử dụng Google Gemini.

### ✨ Điểm nổi bật

- 🤖 **AI Chatbot** - Trợ lý ảo thông minh với Google Gemini 2.5 Flash
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

#### 🤖 AI Chatbot
- ✅ Tư vấn sản phẩm thời trang
- ✅ Trả lời câu hỏi về giá cả, tồn kho
- ✅ Hướng dẫn chính sách đổi trả, vận chuyển
- ✅ Tìm kiếm sản phẩm theo yêu cầu
- ✅ Hỗ trợ 24/7

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

## 📦 Cài đặt

### Yêu cầu hệ thống

- ✅ .NET 8.0 SDK
- ✅ SQL Server 2019+
- ✅ Visual Studio 2022 (khuyến nghị) hoặc VS Code
- ✅ Node.js (optional, cho development tools)

### Bước 1: Clone repository

```bash
git clone https://github.com/your-username/ShoppingLearn.git
cd ShoppingLearn
```

### Bước 2: Restore packages

```bash
dotnet restore
```

### Bước 3: Cập nhật Connection String

Mở `appsettings.json` và cập nhật:

```json
{
  "ConnectionStrings": {
    "ConnectedDb": "Data Source=YOUR_SERVER;Initial Catalog=Shopping;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
  }
}
```

### Bước 4: Chạy Migrations

```bash
# Tạo database
dotnet ef database update

# Hoặc sử dụng Package Manager Console trong Visual Studio
Update-Database
```

### Bước 5: Import Sample Data (Optional)

Mở SQL Server Management Studio và chạy:
- `Database/SampleData.sql` - Sample products data

### Bước 6: Chạy application

```bash
# Command line
dotnet run

# Hoặc Visual Studio: Nhấn F5
```

Application sẽ chạy tại: `https://localhost:5065` (hoặc port khác)

---

## ⚙ Cấu hình

### 1. Google Gemini API (Cho Chatbot)

#### Lấy API Key (MIỄN PHÍ)

1. Truy cập: https://aistudio.google.com/app/apikey
2. Đăng nhập Google Account
3. Click "Create API Key"
4. Copy API key

#### Cập nhật appsettings.json

```json
{
  "Gemini": {
    "ApiKey": "PASTE_YOUR_API_KEY_HERE"
  }
}
```

### 2. Momo API (Optional)

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

### 3. VNPay (Optional)

```json
{
  "Vnpay": {
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET",
    "BaseUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"
  }
}
```

---

## 🤖 Chatbot AI

### Tổng quan

Chatbot được xây dựng với kiến trúc **RAG (Retrieval Augmented Generation)**, kết hợp:
- **Google Gemini 2.5 Flash** - LLM miễn phí, nhanh
- **ChromaDB** - Vector database cho semantic search
- **SQL Server** - Real-time product data

### Kiến trúc Chatbot

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

### Setup Chatbot

#### Bước 1: Cấu hình API Key

Xem phần [Cấu hình](#-cấu-hình) ở trên

#### Bước 2: Thêm vào Layout

Mở `Views/Shared/_Layout.cshtml`, thêm trước `</body>`:

```html
<!-- Chatbot CSS -->
<link rel="stylesheet" href="~/css/chatbot.css" />

<!-- Chatbot JavaScript -->
<script src="~/js/chatbot.js"></script>
```

#### Bước 3: Test Chatbot

1. Chạy application
2. Mở trang chủ
3. Click nút chat ở góc phải màn hình
4. Thử các câu hỏi:
   - "Có áo thun nam nào giá rẻ không?"
   - "Chính sách đổi trả như thế nào?"
   - "Tìm giúp tôi quần jean nữ"
   - "Thời gian giao hàng bao lâu?"

### Chatbot API Endpoints

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

### Relationships

```
Categories 1 ──── ∞ Products
Brands 1 ──────── ∞ Products
Users 1 ───────── ∞ Orders
Orders 1 ──────── ∞ OrderDetails
Products 1 ────── ∞ OrderDetails
Products 1 ────── ∞ Ratings
```

---

## 🧪 Testing

### Unit Tests (Recommended)

```csharp
using Xunit;
using Moq;
using ShoppingLearn.Services.Chatbot;

public class ChatbotServiceTests
{
    [Fact]
    public async Task ProcessMessage_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var mockGemini = new Mock<IGeminiService>();
        mockGemini.Setup(x => x.SendMessageAsync(
            It.IsAny<string>(), null, null))
            .ReturnsAsync("Test response");

        var service = new ChatbotService(
            mockGemini.Object,
            Mock.Of<IChromaService>(),
            Mock.Of<ISqlQueryService>(),
            Mock.Of<ILogger<ChatbotService>>()
        );

        var request = new ChatRequest {
            Message = "Hello",
            SessionId = "test"
        };

        // Act
        var response = await service.ProcessMessageAsync(request);

        // Assert
        Assert.True(response.Success);
    }
}
```

### Integration Tests

```bash
# Chạy tests
dotnet test
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

### Chatbot không hiển thị

**Nguyên nhân**: Chưa thêm CSS/JS vào Layout

**Giải pháp**:
```html
<!-- Trong _Layout.cshtml -->
<link rel="stylesheet" href="~/css/chatbot.css" />
<script src="~/js/chatbot.js"></script>
```

### API trả về lỗi 500

**Nguyên nhân**: Sai Gemini API Key

**Giải pháp**:
- Kiểm tra API key trong `appsettings.json`
- Đảm bảo API key còn hoạt động

### Chatbot trả lời sai

**Nguyên nhân**: Knowledge base thiếu thông tin

**Giải pháp**:
- Bổ sung tài liệu vào `Knowledge/`
- Cập nhật sample data trong database

### Migration errors

**Giải pháp**:
```bash
# Remove migrations
dotnet ef migrations remove

# Add new migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Payment Gateway errors

**Giải pháp**:
- Kiểm tra API keys (Momo, VNPay)
- Kiểm tra ReturnUrl và NotifyUrl
- Test trong môi trường sandbox trước

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

- **Your Name** - *Initial work* - [YourGitHub](https://github.com/yourusername)

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

## 🔄 Changelog

### Version 1.0.0 (2025-12-27)
- ✅ Initial release
- ✅ E-commerce core features
- ✅ AI Chatbot with Google Gemini
- ✅ Momo & VNPay integration
- ✅ Admin dashboard
- ✅ Responsive design

---

<div align="center">

**Made with ❤️ using ASP.NET Core 8.0**

⭐ Star this repo if you find it helpful!

[Back to top](#️-shoppinglearn---e-commerce-fashion-platform)

</div>
