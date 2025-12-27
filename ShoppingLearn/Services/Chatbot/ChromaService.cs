using System.Text;
using Newtonsoft.Json;

namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Service quản lý ChromaDB cho Retrieval Augmented Generation (RAG)
    /// ChromaDB lưu trữ embeddings của tài liệu để tìm kiếm ngữ nghĩa
    /// </summary>
    public class ChromaService : IChromaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _chromaUrl;
        private readonly string _collectionName = "fashion_knowledge";

        public ChromaService(IConfiguration configuration)
        {
            _chromaUrl = configuration["Chroma:Url"] ?? "http://localhost:8000";
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Khởi tạo collection trong ChromaDB
        /// </summary>
        public async Task InitializeCollectionAsync()
        {
            try
            {
                var requestBody = new
                {
                    name = _collectionName,
                    metadata = new { description = "Fashion knowledge base" }
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                await _httpClient.PostAsync($"{_chromaUrl}/api/v1/collections", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChromaDB initialization error: {ex.Message}");
            }
        }

        /// <summary>
        /// Đọc và lưu tài liệu từ thư mục Knowledge vào ChromaDB
        /// </summary>
        public async Task IngestDocumentsAsync(string knowledgePath)
        {
            try
            {
                if (!Directory.Exists(knowledgePath))
                {
                    Console.WriteLine($"Knowledge folder not found: {knowledgePath}");
                    return;
                }

                var files = Directory.GetFiles(knowledgePath, "*.*", SearchOption.AllDirectories)
                    .Where(f => f.EndsWith(".txt") || f.EndsWith(".md"));

                var documents = new List<string>();
                var metadatas = new List<object>();
                var ids = new List<string>();

                foreach (var file in files)
                {
                    var content = await File.ReadAllTextAsync(file);
                    var chunks = SplitIntoChunks(content, 500); // Chia nhỏ tài liệu thành chunks

                    for (int i = 0; i < chunks.Count; i++)
                    {
                        documents.Add(chunks[i]);
                        metadatas.Add(new { source = Path.GetFileName(file), chunk = i });
                        ids.Add($"{Path.GetFileNameWithoutExtension(file)}_{i}");
                    }
                }

                if (documents.Any())
                {
                    await AddDocumentsToChromaAsync(documents, metadatas, ids);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ingesting documents: {ex.Message}");
            }
        }

        /// <summary>
        /// Tìm kiếm tài liệu liên quan trong ChromaDB dựa trên query
        /// </summary>
        public async Task<List<string>> SearchAsync(string query, int topK = 3)
        {
            try
            {
                // Sử dụng in-memory storage thay vì ChromaDB để đơn giản
                // Trong production nên dùng ChromaDB thực sự
                return await SearchInMemoryAsync(query, topK);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search error: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// Tìm kiếm đơn giản trong memory (fallback khi không có ChromaDB)
        /// </summary>
        private async Task<List<string>> SearchInMemoryAsync(string query, int topK)
        {
            var results = new List<string>();
            var knowledgePath = Path.Combine(Directory.GetCurrentDirectory(), "Knowledge");

            if (!Directory.Exists(knowledgePath))
                return results;

            var files = Directory.GetFiles(knowledgePath, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".txt") || f.EndsWith(".md"));

            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                var lines = content.Split('\n');

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && IsRelevant(query, line))
                    {
                        results.Add(line.Trim());
                        if (results.Count >= topK)
                            break;
                    }
                }

                if (results.Count >= topK)
                    break;
            }

            return results;
        }

        /// <summary>
        /// Kiểm tra xem một dòng text có liên quan đến query không
        /// </summary>
        private bool IsRelevant(string query, string text)
        {
            var queryWords = query.ToLower().Split(' ');
            var textLower = text.ToLower();

            return queryWords.Any(word => textLower.Contains(word) && word.Length > 3);
        }

        /// <summary>
        /// Chia text thành các chunks nhỏ
        /// </summary>
        private List<string> SplitIntoChunks(string text, int chunkSize)
        {
            var chunks = new List<string>();
            var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            var currentChunk = new StringBuilder();

            foreach (var sentence in sentences)
            {
                if (currentChunk.Length + sentence.Length > chunkSize)
                {
                    if (currentChunk.Length > 0)
                    {
                        chunks.Add(currentChunk.ToString().Trim());
                        currentChunk.Clear();
                    }
                }
                currentChunk.Append(sentence).Append(". ");
            }

            if (currentChunk.Length > 0)
            {
                chunks.Add(currentChunk.ToString().Trim());
            }

            return chunks;
        }

        /// <summary>
        /// Thêm documents vào ChromaDB
        /// </summary>
        private async Task AddDocumentsToChromaAsync(List<string> documents, List<object> metadatas, List<string> ids)
        {
            try
            {
                var requestBody = new
                {
                    documents = documents,
                    metadatas = metadatas,
                    ids = ids
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                await _httpClient.PostAsync(
                    $"{_chromaUrl}/api/v1/collections/{_collectionName}/add",
                    content
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding documents to Chroma: {ex.Message}");
            }
        }
    }
}
