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

    public DBLayout()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        this.connectionString = $"Database={database};Datasource={host};User={user};Password={password}";
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