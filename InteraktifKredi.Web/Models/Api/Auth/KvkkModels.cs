using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InteraktifKredi.Web.Models.Api.Auth
{
    /// <summary>
    /// Response model for KVKK text retrieval
    /// </summary>
    public class KvkkTextResponse
    {
        /// <summary>
        /// KVKK document ID
        /// </summary>
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        /// <summary>
        /// KVKK document title/name
        /// </summary>
        [JsonPropertyName("Name")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// KVKK document text content
        /// </summary>
        [JsonPropertyName("Text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// KVKK document version (optional)
        /// </summary>
        [JsonPropertyName("Version")]
        public string Version { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for KVKK approval
    /// </summary>
    public class KvkkApprovalRequest
    {
        /// <summary>
        /// Customer ID
        /// </summary>
        [Required(ErrorMessage = "Customer ID gereklidir")]
        [JsonPropertyName("CustomerId")]
        public int CustomerId { get; set; }

        /// <summary>
        /// KVKK document ID being approved
        /// </summary>
        [Required(ErrorMessage = "KVKK ID gereklidir")]
        [JsonPropertyName("KvkkId")]
        public int KvkkId { get; set; }

        /// <summary>
        /// Approval status (true = approved, false = rejected)
        /// </summary>
        [Required(ErrorMessage = "Onay durumu gereklidir")]
        [JsonPropertyName("isOk")]
        public bool IsOk { get; set; }
    }

    /// <summary>
    /// Response model for KVKK approval save
    /// API returns: {"id": 148, "message": "KVKK onay kaydı oluşturuldu."}
    /// </summary>
    public class KvkkApprovalResponse
    {
        /// <summary>
        /// Generated approval record ID
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Success message from API
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}

