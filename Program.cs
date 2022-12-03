using System.Text;
using ConsoleApp3;
using MySql.Data.MySqlClient;


DBLayout connection = new DBLayout();
var result = connection.getAll("users");
Console.WriteLine("Введите логин и пароль");
string comparerLogin = Console.ReadLine();
string comparerPassword = Console.ReadLine();

if(!connection.compareAll(result, comparerLogin, comparerPassword))
{
    Console.WriteLine("Неправильный логин и пароль. Нужно пройти регистрацию");
}
Console.WriteLine("--------------Регистрация--------------");
Console.WriteLine("Введите логин");
string loginProfile = Console.ReadLine();
Console.WriteLine("Введите пароль");
string passwordProfile = Console.ReadLine();

if (!connection.compareRegistration(result, loginProfile))
{
    connection.registrationProfile("users", loginProfile, passwordProfile);
    Console.WriteLine("Вы успешно зарегистрированы");
}
else
{
    Console.WriteLine("Такой профиль уже существует");
}



