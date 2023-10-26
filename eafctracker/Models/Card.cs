

namespace Eafctracker.Models
{
    public class Card
    {
        
        public int Id { get; init; }
        public int FbId { get; set; }
        public int FbDataId { get; set; }
        public string DisplayedName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool Tradable { get; set; } = false;
        public int PcId { get; set; }
        public Pc PcPrices { get; set; }
        public int PsId { get; set; }
        public Ps PsPrices {get;set;}
        public string Position { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public int Raiting { get; set; }

    }
}