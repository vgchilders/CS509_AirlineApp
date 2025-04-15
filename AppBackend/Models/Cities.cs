using System.ComponentModel.DataAnnotations;
namespace AppBackend.Models
{
public class Cities
{
    public int Id { get; set;}
    [Key]
    public string CityName { get; set;} 
    public  float Longitude { get; set;}
    public   float Latitude { get; set;}


}
}