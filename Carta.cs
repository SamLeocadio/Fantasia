public class Carta
{
    public string Nome { get; set; }
    public int Forca { get; set; }
    public int Velocidade { get; set; }
    public int Inteligencia { get; set; }

    public void Exibir()
    {
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Força: {Forca}");
        Console.WriteLine($"Velocidade: {Velocidade}");
        Console.WriteLine($"Inteligência: {Inteligencia}");
    }
}
