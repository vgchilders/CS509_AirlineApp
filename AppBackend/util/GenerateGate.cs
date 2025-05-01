

namespace AppBackend.util
{
 public class GenerateGate{
    public static string Generate(){
        Random _random= new();
        var terminalLetter=(char)('A'+_random.Next(0,5));
        var gatenum=_random.Next(1,31);
        return $"{terminalLetter}{gatenum}";

    }
 }
}
