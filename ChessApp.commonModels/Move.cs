using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.commonModels
{
    public class Move
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Piece { get; set; }
        public string Promotion { get; set; }
        public bool IsCastling { get; set; }
        public bool IsEnPassant { get; set; }
        public bool IsCheck { get; set; }
        public bool IsCheckmate { get; set; }
    }
}
