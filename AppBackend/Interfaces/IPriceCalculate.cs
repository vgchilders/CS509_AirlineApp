namespace AppBackend.Interfaces
{
 public interface IPricecalculate
 {
  public Task  <double>  CalaculatePrice(string city1,string city2);  
 }

}