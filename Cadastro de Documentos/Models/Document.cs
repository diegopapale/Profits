using System.ComponentModel.DataAnnotations;

namespace Cadastro_de_Documentos.Models
{
    public class Document
    {
        public int Id { get; set; }

        // Campos relacionados ao documento
        public string Title { get; set; }
        public string Code { get; set; }
        public string Revision { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
      

        // Propriedade estática para armazenar os documentos em memória
        public static List<Document> InMemoryDocuments { get; set; } = new List<Document>();
    }
}
