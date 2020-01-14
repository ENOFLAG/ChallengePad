using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChallengePad.Models
{
    public class Operation
    {
#pragma warning disable CS8618
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Objective> Objectives { get; set; } = new List<Objective>();
        public List<UploadedFile> Files { get; set; } = new List<UploadedFile>();
#pragma warning restore CS8618

        [NotMapped]
        public IEnumerable<Category> Categories => Objectives
            .GroupBy(o => o.Category)
            .Select(g => g.Aggregate(
                new Category()
                {
                    Name = g.Key
                },
                (category, item) =>
                {
                    category.Objectives.Add(item);
                    return category;
                }));

        [NotMapped] public long SolvedObjectives => Objectives.Sum(o => o.Solved ? 1 : 0);
        [NotMapped] public long SolvedPoints => Objectives.Sum(o => o.Solved ? o.Points : 0);
        [NotMapped] public long TotalPoints => Objectives.Sum(o => o.Points);
        [NotMapped] public long TotalObjectives => Objectives.Count;
    }

    public class Category
    {
#pragma warning disable CS8618
        public string Name { get; set; }
        public List<Objective> Objectives { get; set; } = new List<Objective>();
        public long SolvedObjectives => Objectives.Sum(o => o.Solved ? 1 : 0);
        public long TotalObjectives => Objectives.Count;
#pragma warning restore CS8618
    }
}
