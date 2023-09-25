using ManualDataEntryCLI;
using Npgsql;

Console.WriteLine("Enter database location \"localhost or xxx.xxx.xxx.xxx\"");
string? databaseLoc = Console.ReadLine();

Console.WriteLine("Enter password for default database user");
string? databasePass = Console.ReadLine();

var local = "Host=" + databaseLoc + ";Username=postgres;Password=" + databasePass + ";Database=postgres";

using var con = new NpgsqlConnection(local);
try
{
    con.Open();
}
catch (Npgsql.PostgresException)
{
    Console.WriteLine("Unable to establish connection" + Environment.NewLine + "Please check database connection information");
    return;
}

Core mainCore = new Core(con);

bool repeat = false;
do
{
    bool userFound = false;
    string? userName = "";
    Guid? userId = null;
    do
    {
        var foundId = mainCore.findUser();
        userFound = foundId.found;
        userName = foundId.returnName;
        userId = foundId.returnId;
    } while (!userFound);

    bool menuSelected = false;
    int menuOption = 0;
    do
    {
        Console.WriteLine("Please select task from menu below");
        Console.WriteLine("1. View wearable data from specific sleep review");
        Console.WriteLine("2. Insert wearable data to specific sleep review");
        Console.WriteLine("3. Remove wearable data from specific sleep review");
        //Console.WriteLine("4. Insert predefined test wearable data values");
        //Console.WriteLine("5. Insert random test wearable data");
        Console.WriteLine("Please enter 1, 2, or 3");
        try
        {
            menuOption = Convert.ToInt32(Console.ReadLine());

            if (menuOption != 1 && menuOption != 2 && menuOption != 3)
            {
                Console.WriteLine("Please only enter the digit 1, 2, or 3");
                menuSelected = false;
            }
            else
            {
                menuSelected = true;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Please only enter the digit 1, 2, or 3");
            menuSelected = false;
        }
        catch (OverflowException)
        {
            Console.WriteLine("Please only enter the digit 1, 2, or 3");
            menuSelected = false;
        }
    } while (!menuSelected);

    bool dateSelected = false;
    DateTime userDateTime;
    do
    {
        Console.WriteLine("Please enter the date of the sleep you would like to access \"M/d/yyyy\"");
        if (DateTime.TryParse(Console.ReadLine(), out userDateTime))
        {
            bool dateConfirmed = false;
            do
            {
                Console.WriteLine("You have entered the date " + userDateTime.ToString("M/d/yyyy"));
                Console.WriteLine("Is this the correct date that you would like to access? yes/no");
                string? yesNo = Console.ReadLine();
                if (String.IsNullOrEmpty(yesNo))
                {
                    dateConfirmed = false;
                }
                else
                {
                    yesNo = yesNo.ToUpper();
                    if (String.Equals(yesNo, "YES"))
                    {
                        dateConfirmed = true;
                        dateSelected = true;
                    }
                    else if (String.Equals(yesNo, "NO"))
                    {
                        dateConfirmed = true;
                        dateSelected = false;
                    }
                    else
                    {
                        dateConfirmed = false;
                    }
                }
            } while (!dateConfirmed);
        }
        else
        {
            dateSelected = false;
        }
    } while (!dateSelected);

    switch (menuOption)
    {
        case 1:
            mainCore.ViewWearableData(userName, (Guid)userId, userDateTime);
            break;
        case 2:
            mainCore.InsertWearableData(userName, (Guid)userId, userDateTime);
            break;
        case 3:
            mainCore.RemoveWearableData(userName, (Guid)userId, userDateTime);
            break;
        default:
            Console.WriteLine("Error");
            break;
    }

    bool repeatConfirmed = false;
    do
    {
        Console.WriteLine("Would you like to perform another task? yes/no");
        string? yesNo = Console.ReadLine();
        if (String.IsNullOrEmpty(yesNo))
        {
            repeatConfirmed = false;
        }
        else
        {
            yesNo = yesNo.ToUpper();
            if (String.Equals(yesNo, "YES"))
            {
                repeatConfirmed = true;
                repeat = true;
            }
            else if (String.Equals(yesNo, "NO"))
            {
                repeatConfirmed = true;
                repeat = false;
            }
            else
            {
                repeatConfirmed = false;
            }
        }
    } while (!repeatConfirmed);
} while (repeat);