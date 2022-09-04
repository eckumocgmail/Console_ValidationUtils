using ApplicationCommon.CommonTypes;

 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCommon.CommonResources
{
    /// <summary>
    /// Модель файлового каталога
    /// </summary>
    public class ResourceDirectory : TypeNode<ResourceDirectory>
    {

        public ResourceDirectory():base(null, null, null)
        {
            Files = new List<ResourceItem>();
        }

        public ResourceDirectory(string name) : base(name, null, null)
        {
            Files = new List<ResourceItem>();
            Name = name;
        }

        [Key]
        public int ID { get; set; }

        public virtual List<ResourceItem> Files { get; set; }
    }
}
