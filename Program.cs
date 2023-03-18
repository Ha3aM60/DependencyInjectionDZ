//Task 1 start
public interface IAnimals
{
    void Voice(string voice, string nameAnimal);
    void VoiceToFile(string voice, string nameAnimal, string filePath);
    void VoiceFromFile(string filePath);
}

class Animal : IAnimals
{
    public void Voice(string voice,string nameAnimal)
    {
        Console.WriteLine($"{nameAnimal} -> {voice}");
    }
    public void VoiceToFile(string voice,string nameAnimal,string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(nameAnimal);
            writer.WriteLine(voice);
        }
    }
    public void VoiceFromFile(string filePath) 
    {
        using(StreamReader reader = new StreamReader(filePath))
        {
            string name = reader.ReadLine();
            string voice = reader.ReadLine();
            Console.WriteLine($"{name} -> {voice}");

        }
    }
}

public class AnimalController
{
    readonly IAnimals animal;
    public AnimalController(IAnimals animal)
    {
        this.animal = animal;
    }
    public void Talk(string voice, string name)
    {
        this.animal.Voice(voice,name);
    }
    public void TalkToFile(string voice, string name, string filePath)
    {
        this.animal.VoiceToFile(voice,name,filePath);
    }
    public void TalkFromFile(string filePath)
    {
        this.animal.VoiceFromFile(filePath);
    }
}
//Task 1 end



//Task 2 start
public class Figura
{
    public string Name { get; set; }
    public string Viewer { get; set; }
}



public interface IFigureDataWriter
{
    void Write(Figura figura);
}

public class FigureConsoleWriter : IFigureDataWriter
{
    public void Write(Figura figure)
    {
        Console.WriteLine($"Name: {figure.Name} ");
        Console.WriteLine($"View: {figure.Viewer}");
    }
}

public class FigureFileWriter : IFigureDataWriter
{
    private readonly string _filePath;
    public FigureFileWriter(string filePath)
    {
        _filePath = filePath;
    }

    public void Write(Figura figura)
    {
        using (var writer = new StreamWriter(_filePath, true))
        {
            writer.WriteLine($"Name: {figura.Name} ");
            writer.WriteLine($"View: {figura.Viewer}");
            writer.WriteLine();
        }
    }
}



public interface IFigureService
{
    void SaveFigure(Figura figure);
    Figura[] LoadFigure();
}

public class FigureService : IFigureService
{
    private readonly string _filePath;
    public FigureService(string filePath)
    {
        _filePath = filePath;
    }
    public Figura[] LoadFigure()
    {
        var tmp = new Figura[0];

        if (!File.Exists(_filePath))
            return tmp;

        using var reader = new StreamReader(_filePath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var parts = line.Split(',');
            if (parts.Length != 2) continue;

            var tnp = new Figura
            {
                Name = parts[0],
                Viewer = parts[1]
            };

            Array.Resize(ref tmp, tmp.Length + 1);
            tmp[^1] = tnp;
        }
        return tmp;
    }

    public void SaveFigure(Figura tmp)
    {
        using var writer = new StreamWriter(_filePath, true);
        writer.WriteLine($"{tmp.Name},{tmp.Viewer}");
    }
}

public class FigureController
{
    private readonly IFigureService _figureService;

    public FigureController(IFigureService figureService)
    {
        _figureService = figureService;
    }

    public void AddFigure()
    {
        Console.Write("Enter name: ");
        var name = Console.ReadLine();
        Console.Write("Enter view figure: ");
        var view = Console.ReadLine();
        

        var tmp = new Figura
        {
            Name = name,
            Viewer = view
        };

        _figureService.SaveFigure(tmp);
        Console.WriteLine("Figure save");
    }

    public void ListFigures()
    {
        var tmp = _figureService.LoadFigure();
        foreach (var item in tmp)
        {
            Console.WriteLine($"Name: {item.Name}");
            Console.WriteLine($"View: {item.Viewer}");
        }
    }
}
//Task 2 end



//main
class Program
{
    public static void Main()
    {
        //Task 1 end
        AnimalController animal = new AnimalController(new Animal());
        animal.Talk("Meow!!!", "Cat");
        animal.TalkToFile("Meow!!!", "Cat " ,"animals.txt");
        animal.TalkFromFile("animals.txt");
        //Task 1 end


        //Task 2 start
        var filePath = "figure.txt";
        var figureService = new FigureService(filePath);
        var figureController = new FigureController(figureService);
        

        while (true)
        {
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. Add figure");
            Console.WriteLine("2. List figure");
            Console.WriteLine("3. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    figureController.AddFigure();
                    break;
                case "2":
                    figureController.ListFigures();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choise. try again.");
                    break;
            }
            Console.WriteLine();
        }
        //Task 2 end
    }
}