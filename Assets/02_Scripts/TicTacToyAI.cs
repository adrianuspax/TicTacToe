using UnityEngine;

public class TicTacToeAI : MonoBehaviour
{
    // Representação dos jogadores
    private const string PLAYER = "x";
    private const string AI = "o";
    private const string EMPTY = "";

    // Função principal que você chama do seu script de controle do jogo
    // Recebe o estado atual do tabuleiro (array de 9 strings)
    public int GetBestMove(string[] board)
    {
        int bestScore = int.MinValue;
        int move = -1;

        // Itera por todas as células para encontrar a melhor jogada inicial
        for (int i = 0; i < board.Length; i++)
        {
            // Verifica se a célula está disponível
            if (board[i] == EMPTY)
            {
                // Faz a jogada hipotética
                board[i] = AI;

                // Chama o Minimax para ver o resultado dessa jogada
                int score = Minimax(board, 0, false);

                // Desfaz a jogada (backtracking) para limpar o tabuleiro para a próxima iteração
                board[i] = EMPTY;

                // Se o resultado for melhor que o anterior, guardamos este movimento
                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        return move; // Retorna o índice (0-8) da melhor jogada
    }

    // O Algoritmo Recursivo
    private int Minimax(string[] board, int depth, bool isMaximizing)
    {
        // 1. Casos Base (O jogo acabou?)
        string result = CheckWinner(board);

        if (result != null)
        {
            if (result == AI) return 10 - depth; // Subtrair depth faz a IA preferir vitórias rápidas
            if (result == PLAYER) return depth - 10; // Adicionar depth faz a IA preferir derrotas lentas (resistir)
            return 0; // Empate
        }

        // 2. Turno da IA (Maximizar pontuação)
        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == EMPTY)
                {
                    board[i] = AI;
                    int score = Minimax(board, depth + 1, false);
                    board[i] = EMPTY;
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        // 3. Turno do Jogador Humano (Minimizar pontuação da IA)
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == EMPTY)
                {
                    board[i] = PLAYER;
                    int score = Minimax(board, depth + 1, true);
                    board[i] = EMPTY;
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    // Lógica auxiliar para verificar vitória (Hardcoded para 3x3 para performance)
    private string CheckWinner(string[] board)
    {
        // Combinações de vitória (linhas, colunas, diagonais)
        int[,] winningLines = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Linhas
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Colunas
            {0, 4, 8}, {2, 4, 6}             // Diagonais
        };

        for (int i = 0; i < winningLines.GetLength(0); i++)
        {
            int a = winningLines[i, 0];
            int b = winningLines[i, 1];
            int c = winningLines[i, 2];

            if (board[a] != EMPTY && board[a] == board[b] && board[b] == board[c])
            {
                return board[a]; // Retorna "X" ou "O"
            }
        }

        // Verifica empate (se não há espaços vazios)
        var isFull = true;

        foreach (string s in board)
        {
            if (s == EMPTY)
            {
                isFull = false;
                break;
            }
        }

        if (isFull) return "Draw";

        return null; // O jogo continua
    }
}