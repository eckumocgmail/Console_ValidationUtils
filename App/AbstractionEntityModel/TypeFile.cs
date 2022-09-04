using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCommon.CommonTypes
{
    public class TypeFile
    {
 

        [NotNull]
        [Required(ErrorMessage = "Необходимо ввести задать тип ресурса (MimeType)")]
        public string Mime { get; set; }
        
        [NotNull]
        [Required(ErrorMessage = "Необходимо указать наименование ресурса")]
        public string Name { get; set; }

        [NotNull]
        [Required(ErrorMessage = "Необходимо ввести бинарные данные ресурса")]
        public byte[] Data { get; set; }


        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Необходимо указать время создания ресурса")]
        public DateTime Changed { get; set; }

    }
}
