using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    class Day5
    {
        public Day5()
        {
            var lines = File.ReadLines(@"..\..\input\day5.txt").ToList();

            var maxSeatId = 0;

            var foundSeats = new List<Seat>();

            foreach (var line in lines)
            {
                var rowCodes = line.Substring(0, 7);
                var columnCodes = line.Substring(7);

                var row = Decoder(rowCodes, 0, 127);
                var column = Decoder(columnCodes, 0, 7);

                var seatId = row * 8 + column;

                foundSeats.Add(new Seat { Row = row, Column = column, SeatId = seatId });

                if (seatId > maxSeatId)
                {
                    maxSeatId = seatId;
                }

            }

            Console.WriteLine(maxSeatId);

            var seatsWithoutFrontAndBackRows = foundSeats.Where(s => s.Row > 0 && s.Row < 127);

            var orderedSeats = foundSeats.OrderBy(s => s.SeatId);

            var myId = 0;

            for (int i = 1; i < orderedSeats.Count(); i++ )
            {
                var seatA = orderedSeats.ElementAt(i-1);
                var seatB = orderedSeats.ElementAt(i);

                if ((seatB.SeatId - seatA.SeatId) == 2)
                {
                    myId = seatA.SeatId + 1;
                }
            }

            Console.WriteLine(myId);
        }

        internal struct Seat
        {
            public int Row;
            public int SeatId;
            public int Column;
        }


        private int Decoder(string codes, int rangeStart, int rangeEnd)
        {
            
            foreach (var code in codes)
            {
                if (isLower(code))
                {
                    rangeEnd -= (int)Math.Ceiling((decimal)(rangeEnd - rangeStart) / 2);
                }

                if (isUpper(code))
                {
                    rangeStart += (int)Math.Ceiling((decimal)(rangeEnd - rangeStart) / 2);
                }
            }

            return isLower(codes.Last()) ? rangeStart : rangeEnd;
        }

        private bool isLower(char letter) => letter == 'F' || letter == 'L';

        private bool isUpper(char letter) => letter == 'B' || letter == 'R';
    }
}
