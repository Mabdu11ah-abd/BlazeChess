using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.commonModels
{
        public class GameState
        {
            public string CurrentFen { get; set; }
            public string CurrentPlayer { get; set; }

            [BsonRepresentation(BsonType.String)]
            public GameStatus Status { get; set; }
            public List<Move> LegalMoves { get; set; } = new();
        }
}
