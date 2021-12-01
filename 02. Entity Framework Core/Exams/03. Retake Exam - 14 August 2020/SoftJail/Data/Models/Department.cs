namespace SoftJail.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        public Department()
        {
            this.Cells = new HashSet<Cell>();
        }

        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public virtual ICollection<Cell> Cells { get; set; }
    }
}