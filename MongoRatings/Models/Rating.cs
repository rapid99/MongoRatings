using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MongoRatings.Models
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Display(Name = "Created At")]
        [BsonElement]
        public DateTime Created_At { get; set; }
        [Display(Name = "Show Name")]
        [BsonElement]
        public string Show_Name { get; set; }
        [Display(Name = "Season Number")]
        [BsonElement]
        public int Season_Number { get; set; }
        [Display(Name = "Episode Number")]
        [BsonElement]
        public int Episode_Number { get; set; }
        [Display(Name = "Episode Name")]
        [BsonElement]
        public string Episode_Name { get; set; }
        [Display(Name = "Humor Rating")]
        [BsonElement]
        public int Humor_Rating { get; set; }
        [Display(Name = "Story Rating")]
        [BsonElement]
        public int Story_Rating { get; set; }
        [Display(Name = "Best Character")]
        [BsonElement]
        public string Best_Character { get; set; }
        [Display(Name = "Overall Rating")]
        [BsonElement]
        public int Overall_Rating { get; set; }
    }
}
