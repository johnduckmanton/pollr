﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pollr.Api.Models
{
    public class Poll
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("handle")]
        public string Handle { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "closed";

        [BsonElement("pollDefinitionId")]
        public ObjectId PollDefinitionId { get; set; }

        [BsonElement("pollDate")]
        public DateTime PollDate { get; set; } = DateTime.Now;

        [BsonElement("currentQuestion")]
        public short CurrentQuestion { get; set; } = 1;
               
        [BsonElement("questions")]
        public Question[] Questions { get; set; }

    }

    public class Question
    {
        [BsonElement("questionText")]
        public string QuestionText { get; set; }

        [BsonElement("isDisabled")]
        public Boolean IsDisabled { get; set; } = false;

        [BsonElement("answers")]
        public Answer[] Answers { get; set; }

        [BsonElement("totalVotes")]
        public int TotalVotes { get; set; }
    }

    public class Answer
    {
        [BsonElement("answerText")]
        public string AnswerText { get; set; }

        [BsonElement("imagePath")]
        public string ImagePath { get; set; }

        [BsonElement("isCorrectAnswer")]
        public Boolean IsCorrectAnswer { get; set; } = false;

        [BsonElement("isDisabled")]
        public Boolean IsDisabled { get; set; } = false;


        [BsonElement("voteCount")]
        public int VoteCount { get; set; }


    }
}
