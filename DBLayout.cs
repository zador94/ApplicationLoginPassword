using System.Text;
using System.Xml;
using MySql.Data.MySqlClient;

namespace ConsoleApp3;

public class DBLayout
{
    string host = "localhost"; //имя хоста
    string database = "test"; //имя базы данных
    string user = "root"; //имя пользователя базы данных
    string password = "root"; //пароль

    private string connectionString; //строка соединения

    private MySqlConnection mySqlConnection;
    
    //Добавить описание ко всем методам. Почитать статью об этом - https://habr.com/ru/post/41514/
    public DBLayout()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        this.connectionString = $"Database={database};Datasource={host};User={user};Password={password}";
        //Изучить какие бывают Exception и добавить try catch в соответствии с этими исключениями (ошибки логировать в файл)
        this.mySqlConnection = new MySqlConnection(connectionString);
    }

    public List<Dictionary<string, string>> getAll(string tableName)
    {
        List <Dictionary<string, string>> resultList = new List<Dictionary<string, string>>();
        
        MySqlCommand query = mySqlConnection.CreateCommand();
        query.CommandText = $"SELECT * FROM {tableName}";

        //Открываем поток подключения к базе данных
        try
        {
            mySqlConnection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        //Создаем объект для чтения данных из базы
        MySqlDataReader result = query.ExecuteReader();
        
        int i = 0;
        while (result.Read())
        {
            Dictionary<string, string> queryResult = new Dictionary<string, string>();
            while (true)
            {
                try
                {
                    queryResult.Add(result.GetName(i), result.GetString(i));
                    i++;
                }
                catch (Exception e)
                {
                    i = 0;
                    resultList.Add(queryResult);
                    break;
                }
            }
        }
        mySqlConnection.Close();
        return resultList;
    }

    //Этот метод можно сделать статическим
    public bool compareAll(List <Dictionary<string, string>> resultList, string login, string password)
    {
        foreach (var item in resultList)
        {
            foreach (var innerItem in item)
            {
                if (innerItem.Key == "id")
                {
                    continue;
                }
                //Неверная логика (переделать)
                if (innerItem.Key == "login")
                {
                    if (innerItem.Value == login)
                    {
                        Console.Write("");
                    }
                } 
                if (innerItem.Key == "password")
                {
                    if (innerItem.Value == password)
                    {
                        //Не использовать методы вывода в консоль в данном классе (класс должен быть независимым от метода ввода и вывода)
                        Console.WriteLine("Успешный вход");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void registrationProfile(string nameTable, string loginProfile, string passwordProfile)
    {
        MySqlCommand query = mySqlConnection.CreateCommand();
        query.CommandText = $"INSERT INTO `{nameTable}`(`login`, `password`) VALUES ('{loginProfile}','{passwordProfile}')";

        try
        {
            mySqlConnection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        query.ExecuteNonQuery();
        mySqlConnection.Close();
    }

    public bool compareRegistration(List<Dictionary<string, string>> resultList, string login)
    {
        foreach (var item in resultList)
        {
            foreach (var innerItem in item)
            {
                if (innerItem.Key == "id")
                {
                    continue;
                }
                if (innerItem.Key == "password")
                {
                    continue;
                } 
                if (innerItem.Key == "login")
                {
                    //неверная логика (при такой логике будет регистрировать пользователей с совпадающими логинами)
                    if (innerItem.Value == login)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}