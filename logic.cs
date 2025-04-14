using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Exibe mensagem de boas-vindas
        Console.WriteLine("Bem-vindo ao Super Trunfo!");

        // Pergunta quantos jogadores vão jogar (entre 2 e 4)
        int numJogadores;
        do
        {
            Console.Write("Digite o número de jogadores (2 a 4): ");
        } while (!int.TryParse(Console.ReadLine(), out numJogadores) || numJogadores < 2 || numJogadores > 4);

        // Lê o nome de cada jogador e guarda na lista
        var nomes = new List<string>();
        for (int i = 0; i < numJogadores; i++)
        {
            Console.Write($"Digite o nome do jogador {i + 1}: ");
            nomes.Add(Console.ReadLine());
        }

        // Gera as cartas e embaralha
        var baralho = Baralho.GerarCartas();
        var random = new Random();
        baralho = baralho.OrderBy(c => random.Next()).ToList();

        // Cria uma fila de cartas para cada jogador
        var jogadores = new List<Queue<Carta>>();
        for (int i = 0; i < numJogadores; i++)
            jogadores.Add(new Queue<Carta>());

        // Distribui as cartas entre os jogadores
        for (int i = 0; i < baralho.Count; i++)
            jogadores[i % numJogadores].Enqueue(baralho[i]);

        int rodada = 1;

        // Loop principal do jogo (termina quando só sobra 1 jogador com cartas)
        while (jogadores.Count(j => j.Count > 0) > 1)
        {
            Console.Clear();
            Console.WriteLine($" Rodada {rodada++}");

            // Lista de cartas jogadas na rodada
            var cartasEmJogo = new List<Carta>();
            var jogadoresNaRodada = new List<int>();

            // Cada jogador com cartas entra na rodada
            for (int i = 0; i < jogadores.Count; i++)
            {
                if (jogadores[i].Count > 0)
                {
                    cartasEmJogo.Add(jogadores[i].Dequeue());
                    jogadoresNaRodada.Add(i);
                }
                else
                {
                    cartasEmJogo.Add(null); // Jogador sem carta
                }
            }

            // Mostra a carta do primeiro jogador (quem escolhe o atributo)
            int jogadorDaVez = jogadoresNaRodada[0];
            var cartaDoJogador = cartasEmJogo[jogadorDaVez];

            Console.WriteLine($"\n {nomes[jogadorDaVez]}, esta é a sua carta:");
            cartaDoJogador.Exibir();

            Console.WriteLine("\nQual atributo você quer usar?");
            Console.WriteLine("1 - Força");
            Console.WriteLine("2 - Velocidade");
            Console.WriteLine("3 - Inteligência");

            int escolha;
            while (!int.TryParse(Console.ReadLine(), out escolha) || escolha < 1 || escolha > 3)
            {
                Console.WriteLine("Escolha inválida. Digite 1, 2 ou 3:");
            }

            // Mostra as cartas de todos os jogadores e o valor do atributo escolhido
            int maiorValor = -1;
            int indiceVencedor = -1;
            bool empate = false;

            Console.WriteLine("\n Comparação de cartas:");
            for (int i = 0; i < jogadores.Count; i++)
            {
                var carta = cartasEmJogo[i];
                if (carta == null) continue;

                int valor = ObterValor(carta, escolha);
                Console.WriteLine($"{nomes[i]}: {carta.Nome} - valor escolhido: {valor}");

                if (valor > maiorValor)
                {
                    maiorValor = valor;
                    indiceVencedor = i;
                    empate = false;
                }
                else if (valor == maiorValor)
                {
                    empate = true;
                }
            }

            // Verifica se teve empate ou se alguém venceu
            if (!empate)
            {
                Console.WriteLine($"\n {nomes[indiceVencedor]} venceu esta rodada!");
                foreach (var carta in cartasEmJogo.Where(c => c != null))
                    jogadores[indiceVencedor].Enqueue(carta); // Vencedor pega as cartas
            }
            else
            {
                Console.WriteLine("\n Empate! As cartas da rodada são descartadas.");
            }

            // Mostra quantas cartas cada jogador ainda tem
            Console.WriteLine("\n Cartas restantes:");
            for (int i = 0; i < jogadores.Count; i++)
                Console.WriteLine($"{nomes[i]}: {jogadores[i].Count}");

            // Espera o jogador apertar uma tecla antes da próxima rodada
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        // Fim do jogo: exibe quem venceu
        Console.Clear();
        int vencedorFinal = jogadores.FindIndex(j => j.Count > 0);
        Console.WriteLine($"\n {nomes[vencedorFinal]} venceu o jogo!");
    }

    // Retorna o valor do atributo escolhido
    static int ObterValor(Carta carta, int atributo)
    {
        return atributo switch
        {
            1 => carta.Forca,
            2 => carta.Velocidade,
            3 => carta.Inteligencia,
            _ => 0
        };
    }
}
