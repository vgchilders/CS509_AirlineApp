using Microsoft.EntityFrameworkCore;

//FROM CLASSWORK UPDATE FOR PROJECT SQL DATA

class Con{
    //connection string
    public const string str =  "";
}

class Cdata{
    public int ID {get; set;}
    public string Name {get; set;}
    public string CountryCode {get; set;}
    public string District {get; set;}
    public int Population {get; set;}
}

internal class MyContext : DbContext{
    public DbSet<Cdata> City {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(Con.str);
    }
}

class EntityFrame{
    static void Main(string[] args){

        //ADD
        //using var wc = new MyContext();
        //var c = new C(Name = "Foo", CountryCode = "AFG", District = "BAR", Population = 3);
        //wc.Add(c);
        //wc.SaveChanges();

        //READ
        using (var context = new MyContext()){
            var query = from x in context.City 
                where x.District == "BAR" 
                select x;

            var data = query;
            foreach(var d in data){
                Console.WriteLine(d.ID);
            }
            
        }

        Console.WriteLine("Hello, World!");
    }
}
