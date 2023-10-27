using static System.String;

namespace Eafctracker.Models {
     public class SalesHistory
 {
    public string UnixTime { get ; set ; } = Empty;
    public int Price { get ; set ; }
    public int Bin { get ; set ; }
    public string Status { get ; set ; } = Empty;
    public string Updated { get ; set ; } = Empty;
   
   
 }
}