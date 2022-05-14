using System.Text.RegularExpressions;
class InvalidPlayerNameException : Exception // Custom Exceptions
{
    public InvalidPlayerNameException(string name)
        : base(String.Format("Invalid player name: {0}", name)) { }
}
class InvalidAnswerException : Exception
{ 
    public InvalidAnswerException(string answer)
        : base(String.Format("Invalid answer: {0}", answer)) { }
}
class AnswerIsNullException : Exception
{
    public AnswerIsNullException()
        : base(String.Format("Invalid answer (is null)")) { }
}
public abstract class Validate // Sets up specific validation in the check class
{
    protected bool keep_going;
    public abstract int Int(string input, bool keep_going); // Integers
    public abstract string P_N(string input); // Player names
    public abstract string Y_N(string input); // Yes or No
    public abstract string N_N(string input); // Non-null
}
public class Check : Validate
{
    public override int Int(string input, bool keep_going) // Checks input is a valid integer
    {
        int value = 0;
        while (keep_going)
        {
            try { value = Convert.ToInt32(input); keep_going = false; } // Attempt to convert input into an integer
            catch (FormatException) 
            {
                Console.Write("Please enter an integer: ");
                input = Console.ReadLine();
                continue;
            }
        }
        return value; // Returns input string as an integer
    }
    public override string Y_N(string input) // Checks input is valid [Y/N]
    {
        if (input.ToLower() != "y" && input.ToLower() != "n") { throw new InvalidAnswerException(input); }
        return input; // Returns input string as either 'y' or 'n'
    }
    public override string N_N(string input) // Checks input is not null
    {
        if (input.ToLower() == "") { throw new AnswerIsNullException(); }
        return input; // Returns a string
    }
    public override string P_N(string input)
    {
        Regex regex = new Regex("^[a-zA-Z]+$");
        if (!regex.IsMatch(input)) { throw new InvalidPlayerNameException(input); }
        return input;
    }
}
public class Player // Defines and stores players' properties
{
    public string name { get; set; }
    public int score;
    public Player(string aName)
    {
        name= aName;
        score= 0;
    }   
}
public class Die // Defines and stores die properties
{
    public int value;
    public Die()
    {
        value = 0;
    }
}
public class Win // Displays appropriate winning message
{
    public void Print(string name1) {                                              Console.WriteLine("\n\n================== //  Congratulations {0}! You scored 30 first. \\ ==================\n\n", name1); }
    public void Print(string name1, string name2) {                                Console.WriteLine("\n\n============= //  Congratulations {0} and {1}! You reached 30 first  \\ ==============\n\n", name1, name2);}
    public void Print(string name1, string name2, string name3) {                  Console.WriteLine("\n\n========== //  Congratulations {0}, {1} and {2}! You reached 30 first   \\ ===========\n\n", name1, name2, name3); }
    public void Print(string name1, string name2, string name3, string name4) {    Console.WriteLine("\n\n============= //  Congratulations {0}, {1}, {2}, {3}! Four way tie!  \\ ==============\n\n", name1, name2, name3, name4); }
}
 public class Game // Contains main game loop
{
    static void Main() // Entry point
    {
        Intro();
        Validate check = new Check();
        int? p_num = 0;
        while (p_num < 2 || p_num > 4) // Receive user input to specify the number of players
        {
            Console.Write("\nEnter the number of players (2-4):");
            while (true)
            {
                try { p_num = check.Int(check.N_N(Console.ReadLine()), true); break; }
                catch (AnswerIsNullException) { Console.Write("Please enter something: "); }
            }
            
            if (p_num < 2 || p_num > 4) { Console.WriteLine("Enter a number between 2 and 4."); }
        }

        List<Player> p_list = new(); // Generate list of players

        Console.Write("\nEnter name 1: "); // Player 1
        string temp_name;
        while (true)
        {
            try { temp_name = check.P_N(check.N_N(Console.ReadLine())); break; }
            catch (InvalidPlayerNameException) { Console.Write("Player names must not contain numbers or symbols.\nEnter name 1: "); }
            catch (AnswerIsNullException) { Console.Write("Please enter something: "); }
        }
        Player player1 = new(temp_name);
        p_list.Add(player1);

        Console.Write("Enter name 2: "); // Player 2
        while (true)
        {
            try { temp_name = check.P_N(check.N_N(Console.ReadLine())); break; }
            catch (InvalidPlayerNameException) { Console.Write("Player names must not contain numbers or symbols.\nEnter name 2: "); }
            catch (AnswerIsNullException) { Console.Write("Please enter something: "); }
        }
        Player player2 = new(temp_name);
        p_list.Add(player2);
        if (p_num > 2)
        {
            Console.Write("Enter name 3: "); // Player 3
            while (true)
            {
                try { temp_name = check.P_N(check.N_N(Console.ReadLine())); break; }
                catch (InvalidPlayerNameException) { Console.Write("Player names must not contain numbers or symbols.\nEnter name 3: "); }
                catch (AnswerIsNullException) { Console.Write("Please enter something: "); }
            }
            Player player3 = new(temp_name);
            p_list.Add(player3);
            if (p_num == 4)
            {
                Console.Write("Enter name 4: "); // Player 4
                while (true)
                {
                    try { temp_name = check.P_N(check.N_N(Console.ReadLine())); break; }
                    catch (InvalidPlayerNameException) { Console.Write("Player names must not contain numbers or symbols.\nEnter name 4: "); }
                    catch (AnswerIsNullException) { Console.Write("Please enter something: "); }
                }
                Player player4 = new(temp_name);
                p_list.Add(player4);
            }
        }

        List<Die> d_list = new(); // Populate list of die
        Die one = new();
        d_list.Add(one);
        Die two = new();
        d_list.Add(two);
        Die three = new();
        d_list.Add(three);
        Die four = new();
        d_list.Add(four);
        Die five = new();
        d_list.Add(five);

        int turn_count = 0;
        while (true) // Main game loop // Runs until a player reaches 30 score
        {
            if (turn_count > 0) // Display current scores
            {
                Console.WriteLine("\nScores after turn {0}:", turn_count);
                foreach (Player p in p_list) { Console.WriteLine("{0}: {1}", p.name, p.score); }
            }

            List<Player> winners = new();
            bool is_winner = false;
            foreach (Player p in p_list) // Check for winner
            { 
                if (p.score >= 30)
                { 
                    winners.Add(p);
                    is_winner = true;
                }
            }
            Win w = new();
            if (is_winner == true) // Display winner messages
            {
                if (winners.Count == 1) { w.Print(winners[0].name); }
                else if (winners.Count == 2) { w.Print(winners[0].name, winners[1].name); }
                else if (winners.Count == 3) { w.Print(winners[0].name, winners[1].name, winners[2].name); }
                else if (winners.Count == 4) { w.Print(winners[0].name, winners[1].name, winners[2].name, winners[3].name); }

                Console.WriteLine("Play Again? [Y/N]"); // Allow user to play again or quit
                if (check.Y_N(Console.ReadLine().ToLower()) == "y") { Main(); }
                else { Console.WriteLine("\nThankyou for playing! Closing application..."); Thread.Sleep(2000);  Environment.Exit(0); }
            }
            turn_count++;
            Console.WriteLine("\n=============== TURN {0} ===============", turn_count); // Display turn number
            
            foreach (Player p in p_list) // Rotates through players in order 
            {
                Console.WriteLine("\n{0}'s turn... [ENTER]", p.name);
                Console.ReadLine();
                                
                int[] rolls = new int[5]; // rolls[] = [value of dice 1, value of dice 2, etc...]
                for (int i = 0; i < rolls.Length; i++)
                {
                    d_list[i].value = Roll();
                    rolls[i] = d_list[i].value;
                }

                int[] result = Find_frequency(rolls); // Find frequencies //result[] = [Num of matches, freq of 1s, ..., freq of 6s, matching number]

                if (result[0] == 2) // Allow the player to reroll remaining die when a double is the top match
                {
                    Console.WriteLine("0 points awarded");
                    Console.WriteLine("\n Dice | 1 | 2 | 3 | 4 | 5 |");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine(" Roll | {0} | {1} | {2} | {3} | {4} |", d_list[0].value, d_list[1].value, d_list[2].value, d_list[3].value, d_list[4].value);
                    Console.WriteLine("\nYou rolled a double... reroll remaining die? [Y/N]");

                    string answer;
                    while (true)
                    {
                        try
                        {
                            answer = check.Y_N(check.N_N(Console.ReadLine())).ToLower();
                            break;
                        }
                        catch (InvalidAnswerException)
                        {
                            Console.Write("Please enter either Y or N: ");
                        }
                        catch (AnswerIsNullException)
                        {
                            Console.Write("Please enter something: ");
                        }
                    }
                    
                    if (answer == "y")
                    {
                        for (int i = 1; i < 5; i++) 
                        {
                            if (rolls[i] != result[7]) // Only reroll die with values different from the top match's
                            {
                                d_list[i].value = Roll();
                                rolls[i] = d_list[i].value;
                            }
                        }

                        for (int i = 0; i < rolls.Length; i++) { d_list[i].value = rolls[i]; } // Re-find frequencies
                        result = Find_frequency(rolls);
                    }
                }

                
                if (result[0] == 3) { Console.WriteLine("\nTriple!!"); }
                else if (result[0] == 6) { Console.WriteLine("\nQuadruple!!!"); }
                else if (result[0] == 12) { Console.WriteLine("\nQuintuple!!!!"); }

                if (result[0] == 0 || result[0] == 2) { Console.WriteLine("\n0 points awarded."); } // Display score allocation
                else { Console.WriteLine("{0} points awarded.", result[0]); p.score += result[0]; }

                Console.WriteLine("\n Dice | 1 | 2 | 3 | 4 | 5 |");
                Console.WriteLine("---------------------------");
                Console.WriteLine(" Roll | {0} | {1} | {2} | {3} | {4} |", d_list[0].value, d_list[1].value, d_list[2].value, d_list[3].value, d_list[4].value);
            }
        }
    }
    private static void Intro() // Introduces the game // Lays out the rules and instructions
    {
        Console.WriteLine(@"
          Three of a Kind
=============== RULES ===============

1. Players take turns to roll five die.
2. Players are allocated score based on the number of matching rolls:
    a. None:        0
    b. Double:      0
    c. Triple:      3
    d. Quadruple:   6
    e. Quintuple:   12
3. If a player rolls a double, they may choose to reroll the remaining die.
4. The first player to reach a score of 30 wins.");
    }
    private static int Roll() // Rolls die by generating random number between 1-6
    {
        Random rnd = new();
        int val = rnd.Next(1, 6);
        return val;
    }
    private static int[] Find_frequency(int[] rolls) // Finds the freqeuncy that numbers 1-6 have been rolled // Returns as a seperate array
    {
        int[] count = new int[6]; // Holds frequencies
        int[] result = new int[8]; // Combines awarded score, freqeuncies, and the value of the top match

        for (int x = 1; x < 7; x++) { for (int y = 0; y < rolls.Length; y++) { if (rolls[y] == x) { count[x]++; } } } // Generate frequencies

        foreach (int x in count) { result[x + 1] = count[x]; } // Insert frequencies into result array

        bool is_double = false;
        for (int x = 0; x < count.Length; x++) // Allocate scores and return result array
        {
            if (count[x] == 2) { result[7] = x; is_double = true; }
            else if (count[x] == 3) { result[0] = 3; result[7] = x; return result; }
            else if (count[x] == 4) { result[0] = 6; result[7] = x; return result; }
            else if (count[x] == 5) { result[0] = 12; result[7] = x; return result; }
        }
        if (is_double) { result[0] = 2; return result; }
        else { result[0] = 0; return result; }
    }
}