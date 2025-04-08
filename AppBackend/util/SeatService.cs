namespace AppBackend.util
{
 public class SeatService
 {
    public static List<string>GenerateSeatMap(int rows=30, string seatletters="ABCDEF")
    {
        var seats = new List<string>();
        for (int row=1;row<=rows;row++)
        {
            foreach(char seat in seatletters)
            {
                seats.Add($"{row}{seat}");
            }
        }

        return seats;
    }
 }   
}