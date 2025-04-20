using System.ComponentModel;

abstract class Currency
{
    private int big_part { get; set; }
    private int small_part { get; set; }

    protected int BigPart
    {
        get => big_part;
        set => big_part = value >= 0 ? value : throw new ArgumentException("Значение должно быть неотрицательным");
    }

    protected int SmallPart
    {
        get => small_part;
        set => small_part = (value >= 0 && value <= 99) ? value
               : throw new ArgumentException("Дробная часть должна быть от 0 до 99");
    }

    public abstract void ConvertInRubles();

    public abstract void OutputToScreen();
    public virtual void ConvertToPens()
    { }
    public virtual void ConvertToCents()
    { }

    protected double ReadExchangeRate(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            if (double.TryParse(Console.ReadLine(), out double rate) && rate > 0)
                return rate;
            Console.WriteLine("Ошибка: введите положительное число");
        }
    }

    protected int ReadInt(string message, int min, int max)
    {
        int result = 0;
        while (true)
        {
            Console.WriteLine(message);
            if (max == -1)
            {
                if (int.TryParse(Console.ReadLine(), out result) && result >= min)
                    return result;
                Console.WriteLine($"Ошибка: введите число от {min}");
            }
            else
            {
                if (int.TryParse(Console.ReadLine(), out result) && result >= min && result <= max)
                    return result;
                Console.WriteLine($"Ошибка: введите число от {min} до {max}");
            }
        }
    }
}

interface ICurrency
{
    void ConvertInRubles();
    void OutputToScreen();
}

    class Dollar : Currency, ICurrency
{

    public Dollar() //инициализатор
    {
        BigPart = ReadInt("Введите количество долларов:", 0, -1);
        SmallPart = ReadInt("Введите количество центов:", 0, 99);
    }

    public override void OutputToScreen() //вывод на экран
    {
        Console.WriteLine($"В наличии {BigPart} долларов {SmallPart} центов");
    }

    public override void ConvertInRubles() //конвертация в рубли
    {
        double rate = ReadExchangeRate("Введите текущий курс доллара к рублю:");
        double converted = BigPart * rate + (SmallPart / 100.0) * rate;
        Console.WriteLine($"Эти доллары равны {converted} рублям");
    }

    public override void ConvertToCents() //Перевод в евроценты
    {
        double CourseToCents = ReadExchangeRate("Введите текущий курс доллара к евро");
        double converted = BigPart * 100 * CourseToCents + (SmallPart) * CourseToCents;
        Console.WriteLine($"При курсе доллара к евро {CourseToCents} общая сумма в долларах равна {converted} в евроцентах");
    }

    public override void ConvertToPens() //перевод в пенсы
    {
        double CourseToPens = ReadExchangeRate("Введите текущий курс доллара к фунтам");
        double converted = BigPart * 100 * CourseToPens + (SmallPart) * CourseToPens;
        Console.WriteLine($"При курсе доллара к фунту {CourseToPens} общая сумма в долларах равна {converted} в пенсах");
    }

    ~Dollar() //деструктор
    {
        Console.WriteLine($"Объект Dollar ({BigPart} долларов, {SmallPart} центов) уничтожен.");
    }
}


class Euro: Currency, ICurrency
{

    public Euro() //инициализатор
    {
        BigPart = ReadInt("Введите количество евро:", 0, -1);
        SmallPart = ReadInt("Введите количество евроцентов:", 0, 99);
    }

    public override void OutputToScreen() //вывод на экран
    {
        Console.WriteLine($"В наличии {BigPart} Евро {SmallPart} евроцентов");
    }

    public override void ConvertInRubles() //конвертация в рубли
    {
        double rate = ReadExchangeRate("Введите текущий курс евро к рублю:");
        double converted = BigPart * rate + (SmallPart / 100.0) * rate;
        Console.WriteLine($"Эти евро равны {converted} рублям");
    }

    public override void ConvertToCents() //Перевод в центы
    {
        double CourseToCents = ReadExchangeRate("Введите курс евро к доллару");
        double converted = BigPart * 100 * CourseToCents + (SmallPart) * CourseToCents;
        Console.WriteLine($"При курсе евро к доллару {CourseToCents} общая сумма в евро равна {converted} в центах");
    }

    public override void ConvertToPens() //перевод в пенсы
    {
        double CourseToPens = ReadExchangeRate("Введите курс евро к фунтам");
        double converted = BigPart* 100  * CourseToPens + (SmallPart) * CourseToPens;
        Console.WriteLine($"При курсе евро к фунту {CourseToPens} общая сумма в евро равна {converted} в пенсах");
    }

    ~Euro() //деструктор
    {
        Console.WriteLine($"Объект Euro ({BigPart} евро, {SmallPart} евроцентов) уничтожен.");
    }
}

class Pound: Currency, ICurrency
{

    public Pound() //инициализатор
    {
        BigPart = ReadInt("Введите количество фунтов стерлингов:", 0, -1);
        SmallPart = ReadInt("Введите количество пенсов:", 0, 99);
    }

    public override void OutputToScreen() //вывод на экран
    {
        Console.WriteLine($"В наличии {BigPart} фунтов {SmallPart} пенсов");
    }

    public override void ConvertInRubles() //конвертация в рубли
    {
        double course = ReadExchangeRate("Введите курс фунта стерлинга к рублям");
        double converted = BigPart * course + (SmallPart / 100.0) * course;
        Console.WriteLine($"Эти фунты равны {converted} рублям");
    }

    public override void ConvertToCents() //Перевод в центы
    {
        double CourseToUsCents = ReadExchangeRate("введите курс фунта стерлинга к доллару");
        double CourseToEurCents = ReadExchangeRate("Введите курс фунта стерлинга к евро");
        
        double EurConverted = BigPart * 100 * CourseToEurCents + (SmallPart) * CourseToEurCents;
        Console.WriteLine($"При курсе фунта к евро {CourseToEurCents} общая сумма в фунтах равна {EurConverted} в евроцентах");

        double UsConverted = BigPart * 100 * CourseToUsCents + (SmallPart) * CourseToUsCents;
        Console.WriteLine($"При курсе фунта к доллару {CourseToUsCents} общая сумма в фунтах равна {UsConverted} в центах (доллара)");
    }

    ~Pound() //деструктор
    {
        Console.WriteLine($"Объект Pound ({BigPart} фунтов, {SmallPart} пенсов) уничтожен.");
    }
}

class Purse
{
    private List<Currency> currencies;

    public Purse() // Инициализатор
    {
        currencies = new List<Currency>();
    }

    public void OutputAll()
    { for (int i = 0; i < currencies.Count; i++) {
            currencies[i].OutputToScreen();
        } }

    public void ConvertToRublesAll()
    {
        foreach (Currency cur in  currencies)
        {
            cur.ConvertInRubles();}}

    public void ConvertToPoundAll()
    {
        foreach (Currency cur in currencies)
        { cur.ConvertToPens(); }}

    public void ConvertToCentAll()
    {
        foreach (Currency cur in currencies)
        { cur.ConvertToCents(); }}

    public void AddCurrency(Currency currency)
    {
        currencies.Add(currency);
    }
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Purse purse = new Purse();

            Dollar dollar = new Dollar();
            Euro euro = new Euro();
            Pound pound = new Pound();

            purse.AddCurrency(dollar);
            purse.AddCurrency(euro);
            purse.AddCurrency(pound);

            purse.OutputAll();

            purse.ConvertToRublesAll();
            purse.ConvertToCentAll();
            purse.ConvertToPoundAll();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка в ходе выполнения кода");
            Console.WriteLine(ex.ToString());
        }
    }
}

