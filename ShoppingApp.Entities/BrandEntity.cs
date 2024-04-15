using System;
using System.Collections.Generic;

namespace ShoppingApp.Entities
{
    public class BrandEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the banner image.
        /// </summary>
        public string Name { get; set; }
        public string BrandImage { get; set; }
        

        /// <summary>
        /// Gets or sets a value indicating whether IsDeleted enabled  or not.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the preview images.
        /// </summary>
        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
