using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperHeroes.Entities
{
    public class SuperHero
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 5,
            ErrorMessage = "The property {0} should have {1} maximum characters and {2} minimum characters")]
        public string SuperHeroName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        [RegularExpression(@"([0-9]+)",
          ErrorMessage = "Characters are not allowed.")]
        public int Age { get; set; }
    }
}
