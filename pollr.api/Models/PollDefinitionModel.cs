/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Pollr.Api.Models
{
    [BsonIgnoreExtraElements]
    public class PollDefinition
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId _Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("theme")]
        public string Theme { get; set; }

        [BsonElement("owner")]
        public string Owner { get; set; }

        [BsonElement("isPublished")]
        public bool IsPublished { get; set; } = false;

        [BsonElement("createDate")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [BsonElement("expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [BsonElement("tags")]
        public string[] Tags { get; set; }

        [BsonElement("questions")]
        public QuestionDefinition[] Questions { get; set; }

    }

    public class QuestionDefinition
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("questionText")]
        public string QuestionText { get; set; }

        [BsonElement("hasCorrectAnswer")]
        public Boolean HasCorrectAnswer { get; set; } = false;

        [BsonElement("isDisabled")]
        public Boolean IsDisabled { get; set; } = false;

        [BsonElement("answers")]
        public CandidateAnswer[] Answers { get; set; }
    }

    public class CandidateAnswer
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("answerText")]
        public string AnswerText { get; set; }

        [BsonElement("imagePath")]
        public string ImagePath { get; set; }

        [BsonElement("isCorrectAnswer")]
        public Boolean IsCorrectAnswer { get; set; } = false;

        [BsonElement("isDisabled")]
        public Boolean IsDisabled { get; set; } = false;
    }
}